/*
 * voice-assistant.js  —  Global AI Voice Assistant for the Aarogyabhumi / Shopinv portal
 * ---------------------------------------------------------------------------------------
 * DESIGN GOALS
 *  - Drop-in: included once in the layout, it self-injects a floating mic on EVERY page.
 *  - Zero per-page config: it discovers menus, links, forms, grids and cart controls live.
 *  - Reuses existing app behaviour: it never re-implements business logic. It clicks the
 *    same buttons / calls the same JS functions a real user would (CheckLoginPopUp,
 *    UpdateProdQty, DeleteRow, checkcoupon, prceedCheckOut, SaveOtherAddressDetail, ...).
 *  - Hindi / Hinglish / English understanding via the server intent parser
 *    (/AIAssistant/Command -> Mistral), with a deterministic client-side fallback so basic
 *    navigation still works if the server is unreachable.
 *
 * No existing IDs, routes, names, APIs or DB logic are modified anywhere.
 */
(function () {
    "use strict";

    if (window.__voiceAssistantLoaded) return;       // guard against double-injection
    window.__voiceAssistantLoaded = true;

    // ----------------------------------------------------------------- config / state
    var CFG = {
        // window.__VA_CMD_URL / __VA_BASE are injected by the Razor partial so the
        // assistant works at the site root AND inside an IIS virtual directory / sub-app
        // (e.g. https://site.com/shop/) on the live server, not just on localhost.
        commandUrl: (window.__VA_CMD_URL || "/AIAssistant/Command"),
        lang: "hi-IN",
        ttsLang: "hi-IN",
        matchThreshold: 0.55          // 0..1 similarity floor for fuzzy navigation
    };
    // App base path (no trailing slash). "/" at root, "/shop" in a virtual directory.
    var BASE = (window.__VA_BASE || "/").replace(/\/+$/, "");
    function appUrl(p) { return BASE + p; }   // prefix a root-relative route with the app base
    var recognition = null, listening = false, menuCache = null;
    var lastLang = "hi";          // detected language of the latest command: "hi" | "en" | "hinglish"
    var navigating = false;       // guard against firing multiple navigations at once
    var lastTranscript = "", lastTranscriptAt = 0;   // de-dupe identical recognitions

    // ----------------------------------------------------------------- known routes
    // Static alias map used (a) as a hint and (b) as the offline fallback. Keys are
    // normalized English/Hinglish phrases; values are real controller/action URLs that
    // already exist in this project. Add new aliases here only — never new pages.
    // auth:true => protected page (login required). If not logged in, the assistant routes
    // through Login first and returns here automatically after a successful login.
    var ROUTE_MAP = [
        { url: "/Home/Index",                aliases: ["home", "homepage", "ghar", "mukhya page", "dashboard", "main page", "shop", "dukan"] },
        { url: "/ViewCart/ViewCart",         auth: true, aliases: ["cart", "my cart", "view cart", "cart dikhao", "cart kholo", "tokri", "basket", "bag", "kart"] },
        { url: "/ViewCart/Wishlist",         auth: true, aliases: ["wishlist", "favourites", "pasand", "saved items"] },
        { url: "/CheckOut/CheckOut",         auth: true, aliases: ["checkout", "check out", "billing", "payment page", "payment"] },
        { url: "/Home/Myorders",             auth: true, aliases: ["my orders", "orders", "order", "mere order", "order history", "purchase", "purchase history", "my order"] },
        { url: "/Home/MyCoupon",             auth: true, aliases: ["my coupon", "coupons", "coupon", "mere coupon"] },
        { url: "/Home/WalletDetail",         auth: true, aliases: ["wallet", "wallet report", "wallet detail", "balance", "wallet dikhao", "reward"] },
        { url: "/Home/Levelwisereport",      auth: true, aliases: ["level wise direct report", "direct report", "my direct report", "level report", "direct seller report", "genealogy", "tree", "business", "team"] },
        { url: "/Home/LevelIncome",          auth: true, aliases: ["commission report", "level income", "income report", "commission", "earning", "reports", "report"] },
        { url: "/Home/MemberProfile",        auth: true, aliases: ["profile", "my profile", "member profile", "account profile", "account", "member", "my account"] },
        { url: "/Home/UserKyc",              auth: true, aliases: ["kyc", "update kyc", "my kyc", "kyc update", "kyc page"] },
        { url: "/CategoryList/CategoryList", aliases: ["category", "categories", "products", "all products", "shop by category", "category list", "product"] },
        { url: "/Account/Login",             aliases: ["login", "log in", "sign in", "enter", "signin"] },
        { url: "/Account/SignUp",            aliases: ["sign up", "register", "registration", "naya account", "signup"] },
        { url: "/Account/ChangePassword",    auth: true, aliases: ["change password", "password change", "reset password"] },
        { url: "/Account/RaiseComplaint",    auth: true, aliases: ["raise complaint", "complaint", "ticket", "support", "shikayat", "help"] },
        { url: "/Account/TicketList",        auth: true, aliases: ["ticket list", "my tickets", "complaints list", "complaint list"] },
        { url: "/CheckOrder/CheckOrder",     aliases: ["check order", "track order", "order status", "check status", "tracking"] },
        { url: "/Home/About",                aliases: ["about", "about us", "hamare bare me"] },
        { url: "/Home/Contact",              aliases: ["contact", "contact us", "sampark"] },
        { url: "/Home/PrivacyPolicy",        aliases: ["privacy policy", "privacy"] },
        { url: "/Home/ReturnPolicy",         aliases: ["return policy", "return"] },
        { url: "/Home/ShippingPolicy",       aliases: ["shipping policy", "shipping"] },
        { url: "/Account/LogOff",            aliases: ["logout", "log out", "sign out", "log off"] }
    ];

    // ======================================================================= UTIL
    function normalize(s) {
        return (s || "")
            .toString()
            .toLowerCase()
            .normalize("NFD").replace(/[\u0300-\u036f]/g, "")  // strip diacritics
            .replace(/[^a-z0-9\u0900-\u097F ]/g, " ")          // keep latin + devanagari
            .replace(/\s+/g, " ")
            .trim();
    }

    function levenshtein(a, b) {
        a = a || ""; b = b || "";
        var m = a.length, n = b.length;
        if (!m) return n; if (!n) return m;
        var prev = new Array(n + 1), cur = new Array(n + 1), i, j;
        for (j = 0; j <= n; j++) prev[j] = j;
        for (i = 1; i <= m; i++) {
            cur[0] = i;
            for (j = 1; j <= n; j++) {
                var cost = a.charCodeAt(i - 1) === b.charCodeAt(j - 1) ? 0 : 1;
                cur[j] = Math.min(prev[j] + 1, cur[j - 1] + 1, prev[j - 1] + cost);
            }
            var t = prev; prev = cur; cur = t;
        }
        return prev[n];
    }

    // English Soundex — helps match phonetic mishears ("ledger" ~ "lejer").
    function soundex(s) {
        s = (s || "").toUpperCase().replace(/[^A-Z]/g, "");
        if (!s) return "";
        var codes = { B: 1, F: 1, P: 1, V: 1, C: 2, G: 2, J: 2, K: 2, Q: 2, S: 2, X: 2, Z: 2, D: 3, T: 3, L: 4, M: 5, N: 5, R: 6 };
        var first = s[0], out = first, prev = codes[first] || 0, i;
        for (i = 1; i < s.length && out.length < 4; i++) {
            var c = codes[s[i]] || 0;
            if (c && c !== prev) out += c;
            if (s[i] !== "H" && s[i] !== "W") prev = c;
        }
        return (out + "000").slice(0, 4);
    }

    // Similarity 0..1 combining edit distance, token overlap and phonetics.
    function similarity(query, candidate) {
        var q = normalize(query), c = normalize(candidate);
        if (!q || !c) return 0;
        if (c === q) return 1;
        if (c.indexOf(q) >= 0 || q.indexOf(c) >= 0) return 0.9;   // partial / substring

        var maxLen = Math.max(q.length, c.length);
        var lev = 1 - (levenshtein(q, c) / maxLen);

        var qt = q.split(" "), ct = c.split(" "), hit = 0;
        qt.forEach(function (w) { if (w && ct.indexOf(w) >= 0) hit++; });
        var tok = qt.length ? hit / qt.length : 0;

        var snd = (soundex(q) && soundex(q) === soundex(c)) ? 0.7 : 0;

        return Math.max(lev * 0.6 + tok * 0.4, tok, snd);
    }

    // Returns { item, score } for the best matching candidate, or null.
    function bestMatch(query, candidates, getText, floor) {
        floor = floor == null ? CFG.matchThreshold : floor;
        var best = null;
        for (var i = 0; i < candidates.length; i++) {
            var text = getText ? getText(candidates[i]) : candidates[i];
            if (!text) continue;
            // a candidate's "text" may be a list of aliases
            var texts = Array.isArray(text) ? text : [text];
            for (var k = 0; k < texts.length; k++) {
                var sc = similarity(query, texts[k]);
                if (!best || sc > best.score) best = { item: candidates[i], score: sc, matched: texts[k] };
            }
        }
        return best && best.score >= floor ? best : null;
    }

    // ======================================================================= LANGUAGE
    // Detect the language of a spoken command. Devanagari => Hindi. Otherwise look for
    // common romanized-Hindi markers => Hinglish. Else => English.
    function detectLang(text) {
        var t = text || "";
        if (/[\u0900-\u097F]/.test(t)) return "hi";
        var n = normalize(t);
        var hinglish = /\b(kholo|dikhao|daalo|dalo|karo|kar|hatao|badhao|ghatao|kar do|chahiye|mujhe|mera|meri|kaise|nahi|haan|wala|wale|kitna|page|kholna|laga|lagao|dekho|chalu|band)\b/;
        if (hinglish.test(n)) return "hinglish";
        return "en";
    }

    // Pick a message in the user's language. Pass English, Hindi, Hinglish variants.
    function say(en, hi, hing) {
        var msg = lastLang === "hi" ? hi : (lastLang === "en" ? en : (hing || hi));
        speak(msg);
        return msg;
    }

    // ======================================================================= TTS
    function speak(text) {
        if (!text || !("speechSynthesis" in window)) return;
        try {
            window.speechSynthesis.cancel();
            var u = new SpeechSynthesisUtterance(text);
            // Speak English replies with an English voice; Hindi/Hinglish with hi-IN.
            u.lang = (lastLang === "en") ? "en-IN" : "hi-IN";
            var voices = window.speechSynthesis.getVoices();
            var want = (lastLang === "en") ? /en[-_]?(IN|GB|US)|english/i : /hi|IN/i;
            var v = voices.filter(function (x) { return want.test(x.lang) || want.test(x.name || ""); })[0];
            if (v) u.voice = v;
            u.rate = 1.0;
            window.speechSynthesis.speak(u);
        } catch (e) { /* TTS optional */ }
    }

    // ======================================================================= AUTH / LOGIN
    // Logged-in if the page exposes a Log Out / LogOff link (header shows it only when signed in).
    function isLoggedIn() {
        return !!document.querySelector('a[href*="LogOff" i], a[href*="Logout" i], a[href*="LogOut" i]');
    }
    function onLoginPage() {
        return /\/account\/login/i.test(location.pathname);
    }
    // Click the existing Login submit button (no new logic, just submit the existing form).
    function clickLoginButton() {
        var btn = document.querySelector('form button[type="submit"], form input[type="submit"]')
            || (findClickable("login") || {}).item
            || (findClickable("sign in") || {}).item;
        if (btn) { btn.click(); return true; }
        return false;
    }

    // Single, guarded navigation. Prevents multiple/duplicate redirects (performance).
    function safeNavigate(url, delay) {
        if (navigating) return;
        navigating = true;
        setTimeout(function () { location.href = url; }, delay || 250);
    }

    // ======================================================================= DOM discovery
    // Cache visible navigation anchors; refresh on DOM mutation (MutationObserver),
    // so dynamically rendered menus are picked up without per-page wiring.
    function collectMenuLinks() {
        if (menuCache) return menuCache;
        var anchors = Array.prototype.slice.call(document.querySelectorAll("a[href]"));
        menuCache = anchors
            .map(function (a) {
                // Product cards are often image-only links: the product name lives in the
                // child <img alt> / title, not in textContent. Fall back to those so voice
                // commands like "add green shots to cart" can find the product anywhere.
                var img = a.querySelector("img");
                var label = (a.textContent || "").trim();
                if (!label && img) label = (img.getAttribute("alt") || img.getAttribute("title") || "").trim();
                if (!label) label = (a.getAttribute("aria-label") || a.getAttribute("title") || "").trim();
                return { el: a, text: label, href: a.href };
            })
            .filter(function (l) {
                var href = l.el.getAttribute("href") || "";
                return l.text.length > 1 && href && href.indexOf("javascript:") !== 0 && href !== "#";
            });
        return menuCache;
    }
    (function watchDom() {
        if (!("MutationObserver" in window)) return;
        var mo = new MutationObserver(function () { menuCache = null; });
        var start = function () { mo.observe(document.body, { childList: true, subtree: true }); };
        if (document.body) start(); else document.addEventListener("DOMContentLoaded", start);
    })();

    // Find a form control by label / placeholder / name / id / aria-label.
    // Split "DeliveryAddressID" / "txt_member_name" into "delivery address id ...".
    function wordify(s) {
        return (s || "")
            .replace(/([a-z])([A-Z])/g, "$1 $2")
            .replace(/[_\-]+/g, " ")
            .toLowerCase();
    }

    // All the text we can associate with a form control: <label for>, a wrapping <label>,
    // nearby text, placeholder/aria/name/id (id split into words). Used for fuzzy matching.
    function controlLabelText(el, labelFor) {
        var bits = [];
        if (labelFor[el.id]) bits.push(labelFor[el.id]);
        var wrap = el.closest ? el.closest("label") : null;
        if (wrap) bits.push((wrap.textContent || "").trim());
        // sibling / parent text (checkboxes often have label text right after them)
        var p = el.parentNode;
        if (p) bits.push((p.textContent || "").trim().slice(0, 60));
        bits.push(el.getAttribute("placeholder") || "");
        bits.push(el.getAttribute("aria-label") || "");
        bits.push(el.getAttribute("title") || "");
        bits.push(wordify(el.getAttribute("name") || ""));
        bits.push(wordify(el.id || ""));
        return bits.filter(Boolean);
    }

    function findControl(target) {
        var controls = Array.prototype.slice.call(
            document.querySelectorAll("input:not([type=hidden]):not([type=submit]):not([type=button]), select, textarea")
        ).filter(isVisible);

        var labelFor = {};
        Array.prototype.slice.call(document.querySelectorAll("label[for]")).forEach(function (l) {
            labelFor[l.getAttribute("for")] = (l.textContent || "").trim();
        });

        return bestMatch(target, controls, function (c) { return controlLabelText(c, labelFor); }, 0.45);
    }

    // Select a dropdown by the spoken VALUE itself (not the control's label): find the
    // <select> that has an option best matching `value`, set it, fire change.
    // Far more reliable for things like "self pickup" on #DeliveryAddressID.
    function selectDropdownByValue(value, targetHint) {
        var selects = Array.prototype.slice.call(document.querySelectorAll("select")).filter(isVisible);
        if (!selects.length) return false;
        // Expand common spoken synonyms so "self collect" matches an option "Self PickUP", etc.
        var candidates = [value];
        var nv = normalize(value);
        if (/self collect|self pick|pickup|pick up|collect/.test(nv)) candidates.push("self pickup", "pickup");
        if (/home|courier|office|deliver/.test(nv)) candidates.push("home delivery", "courier", "delivery");
        if (/gateway|online|card|upi|net bank/.test(nv)) candidates.push("payment gateway");
        if (/wallet|shopping wallet/.test(nv)) candidates.push("shopping wallet", "wallet");
        var best = null;
        selects.forEach(function (sel) {
            Array.prototype.slice.call(sel.options).forEach(function (o) {
                var sc = 0;
                candidates.forEach(function (cv) { sc = Math.max(sc, similarity(cv, o.text)); });
                if (o.value === "") sc *= 0.3;   // de-prioritise "Select ..." placeholder
                if (!best || sc > best.score) best = { sel: sel, opt: o, score: sc };
            });
        });
        if (best && best.score >= 0.4) {
            best.sel.value = best.opt.value;
            ["input", "change", "blur"].forEach(function (ev) { best.sel.dispatchEvent(new Event(ev, { bubbles: true })); });
            speak(best.opt.text + (lastLang === "en" ? " selected." : " select kiya."));
            return true;
        }
        return false;
    }

    // Check / uncheck a checkbox or radio found by nearby label text or id/name.
    function setCheckboxByText(target, on) {
        var boxes = Array.prototype.slice.call(document.querySelectorAll("input[type=checkbox], input[type=radio]")).filter(isVisible);
        if (!boxes.length) return false;
        var labelFor = {};
        Array.prototype.slice.call(document.querySelectorAll("label[for]")).forEach(function (l) {
            labelFor[l.getAttribute("for")] = (l.textContent || "").trim();
        });
        var m = bestMatch(target, boxes, function (b) { return controlLabelText(b, labelFor); }, 0.4);
        if (!m) return false;
        var box = m.item;
        box.checked = on;
        ["click", "input", "change"].forEach(function (ev) {
            if (ev === "click" && box.checked !== on) return;   // avoid toggling back
            box.dispatchEvent(new Event(ev, { bubbles: true }));
        });
        speak(on ? "Tick kar diya." : "Untick kar diya.");
        return true;
    }

    // Find a clickable button / link by visible text.
    function findClickable(target) {
        var els = Array.prototype.slice.call(
            document.querySelectorAll("button, a, input[type=submit], input[type=button], [role=button]")
        ).filter(isVisible);
        return bestMatch(target, els, function (e) {
            return (e.textContent || e.value || e.getAttribute("aria-label") || "").trim();
        }, 0.5);
    }

    function isVisible(el) {
        if (!el) return false;
        var r = el.getBoundingClientRect();
        var st = window.getComputedStyle(el);
        return r.width > 0 && r.height > 0 && st.visibility !== "hidden" && st.display !== "none";
    }

    // Fire the same events a real user interaction would, so existing
    // onchange / blur / validation handlers run untouched.
    function setValueAndFire(el, value) {
        var tag = el.tagName.toLowerCase();
        if (tag === "select") {
            var opts = Array.prototype.slice.call(el.options);
            var m = bestMatch(value, opts, function (o) { return o.text; }, 0.4);
            if (m) el.value = m.item.value;
        } else if (el.type === "checkbox" || el.type === "radio") {
            el.checked = !/false|no|nahi|band|off|0/.test(normalize(value));
        } else {
            el.value = value;
        }
        ["input", "change", "blur", "keyup"].forEach(function (ev) {
            el.dispatchEvent(new Event(ev, { bubbles: true }));
        });
    }

    // ======================================================================= CART helpers
    // Locate a cart-page row by spoken product name (or first/last).
    function findCartRow(target, position) {
        var rows = Array.prototype.slice.call(document.querySelectorAll("#cartTable tbody tr, #cartTable tr"))
            .filter(function (tr) { return tr.querySelector(".btn-remove, .btn-plus"); });
        if (!rows.length) return null;
        if (position === "first") return rows[0];
        if (position === "last") return rows[rows.length - 1];
        if (!target) return rows[0];
        var m = bestMatch(target, rows, function (tr) { return (tr.textContent || "").trim(); }, 0.4);
        return m ? m.item : rows[0];
    }

    function clickN(el, n) { for (var i = 0; i < (n || 1) && el; i++) el.click(); }

    // ======================================================================= EXECUTOR
    function execute(intent) {
        if (!intent || !intent.action) { speak("Command samajh nahi aaya."); return; }
        var t = intent.target || "", v = intent.value || "", qty = parseInt(intent.qty, 10) || 0;
        if (intent.speak) flash(intent.transcript, intent.speak);

        switch (intent.action) {

            case "navigate": {
                var entry = resolveRouteEntry(t);
                var url = entry ? entry.url : resolveRoute(t);

                // --- Smart login: if target is the Login page ---
                if (entry && /\/Account\/Login/i.test(entry.raw)) {
                    if (onLoginPage()) {           // already here -> just submit the existing form
                        say("Logging in.", "लॉगिन कर रहे हैं।", "Login kar rahe hain.");
                        clickLoginButton();
                    } else {
                        say("Opening login.", "लॉगिन खोल रहे हैं।", "Login khol rahe hain.");
                        safeNavigate(entry.url);
                    }
                    break;
                }

                // --- Protected page while logged out: go to Login, come back after login ---
                if (entry && entry.auth && !isLoggedIn()) {
                    sessionStorage.setItem("va_afterLogin", entry.url);
                    say("Please login first, then I'll open it.",
                        "पहले लॉगिन कीजिए, फिर मैं खोल दूँगा।",
                        "Pehle login kijiye, phir khol dunga.");
                    safeNavigate(appUrl("/Account/Login"));
                    break;
                }

                // If the user said a record id / date / status while "navigating" to a report
                // we are already on, treat it as a FILTER instead of reloading the page.
                var fval = filterValueFrom(intent);
                if (fval && url && samePage(url) && document.querySelector("table tr")) {
                    var nf = filterTableRows(fval);
                    speak(nf > 0 ? (nf + " result mile.") : ("Record nahi mila: " + fval));
                    break;
                }
                if (url) {
                    if (fval && !samePage(url)) sessionStorage.setItem("va_pendingFilter", JSON.stringify({ val: fval }));
                    speak(intent.speak || "Khol rahe hain");
                    safeNavigate(url, 300);
                }
                else { say("Page not found: " + t, "यह पेज नहीं मिला: " + t, "Yeh page nahi mila: " + t); }
                break;
            }

            case "click": {
                // On the Login page, "login / sign in / submit / continue" submits the form.
                if (onLoginPage() && /login|sign in|submit|continue|enter/.test(normalize(t + " " + v))) {
                    say("Logging in.", "लॉगिन कर रहे हैं।", "Login kar rahe hain.");
                    if (clickLoginButton()) break;
                }
                // A checkbox/radio request ("terms agree karo", "return policy tick karo")?
                if (/agree|terms|condition|policy|return|tick|check|checkbox|same as|self decl/.test(normalize(t + " " + v))) {
                    if (setCheckboxByText(t || v, true)) break;
                }
                var c = findClickable(t);
                if (c) { c.item.click(); }
                else if (setCheckboxByText(t, true)) { /* fell back to checkbox */ }
                else { say("Button not found: " + t, "बटन नहीं मिला: " + t, "Button nahi mila: " + t); }
                break;
            }

            case "fill": {
                var nt = normalize(t), nv = normalize(v);
                // 1) Dropdown: spoken value (or target) matches a <select> option.
                if (/delivery|pickup|pick up|payment|type|method|select|gateway|wallet/.test(nt + " " + nv)) {
                    if (selectDropdownByValue(v || t, t)) break;
                }
                // 2) Checkbox / radio by label.
                if (/agree|terms|condition|policy|return|same as|self decl|tick|check/.test(nt)) {
                    var on = !/false|no|nahi|uncheck|untick|hatao|remove|off/.test(nv);
                    if (setCheckboxByText(t, on)) break;
                }
                // 3) Plain field — but if no labelled field matches, try a dropdown by value.
                var ctrl = findControl(t);
                if (ctrl) { setValueAndFire(ctrl.item, v); }
                else if (selectDropdownByValue(v || t, t)) { /* matched a dropdown */ }
                else if (setCheckboxByText(t, true)) { /* matched a checkbox */ }
                else { speak("Field nahi mila: " + t); }
                break;
            }

            case "filter": {
                applyFilter(t, v, intent.entities, intent);
                break;
            }

            case "cart_view":
                location.href = appUrl("/ViewCart/ViewCart");
                break;

            case "cart_add":
                cartAdd(t, qty);
                break;

            case "cart_qty":
                cartQty(t, v, qty, intent.position);
                break;

            case "cart_remove": {
                var row = findCartRow(t, intent.position);
                var rm = row && row.querySelector(".btn-remove");
                if (rm) rm.click(); else speak("Product nahi mila cart me.");
                break;
            }

            case "coupon": {
                var box = document.getElementById("TxtCoupon");
                if (box) {
                    setValueAndFire(box, v);
                    if (typeof window.checkcoupon === "function") window.checkcoupon();
                    else { var ap = document.getElementById("CouponApplied"); if (ap) ap.click(); }
                } else { speak("Coupon box is page par nahi hai. Pehle cart kholiye."); }
                break;
            }

            case "checkout": {
                if (typeof window.prceedCheckOut === "function") {
                    var btn = document.querySelector('[onclick^="prceedCheckOut"]');
                    if (btn) btn.click(); else window.prceedCheckOut(null, 0, 0);
                } else { location.href = appUrl("/CheckOut/CheckOut"); }
                break;
            }

            case "place_order": {
                // Reuse the existing checkout / payment controls — no new order flow.
                // 1) Checkout page: "Proceed To Pay" -> SaveOtherAddressDetail(...)
                // 2) Payment page: the "Pay" button.
                var save = document.querySelector('[onclick^="SaveOtherAddressDetail"]');
                if (save) {
                    speak("Proceed to pay. Confirm dialog par OK dabaiye.");
                    save.click();
                } else {
                    var any = findClickable(v || "proceed to pay")
                        || findClickable("proceed to pay")
                        || findClickable("place order")
                        || findClickable("pay")
                        || findClickable("proceed");
                    if (any) any.item.click(); else speak("Order/Pay button is page par nahi mila.");
                }
                break;
            }

            case "speak":
                speak(intent.speak || "Theek hai.");
                break;

            default:
                speak(intent.speak || "Yeh command samajh nahi aayi.");
        }
    }

    // Resolve a spoken page name to a real URL: server hint -> static map -> live menu.
    function resolveRoute(target) {
        var e = resolveRouteEntry(target);
        if (e) return e.url;
        var link = bestMatch(target, collectMenuLinks(), function (l) { return l.text; }, 0.5);
        return link ? link.item.href : null;
    }
    // Like resolveRoute but returns { url, auth } from ROUTE_MAP (so we know if login is needed).
    function resolveRouteEntry(target) {
        var m = bestMatch(target, ROUTE_MAP, function (r) { return r.aliases.concat([r.url]); });
        if (m) return { url: appUrl(m.item.url), auth: !!m.item.auth, raw: m.item.url };
        return null;
    }

    // Does a resolved URL point at the page we're already on?
    function samePage(url) {
        try {
            var p = url.split("?")[0].toLowerCase();
            var cur = location.pathname.toLowerCase();
            // compare last path segment(s) so /Account/TicketList matches regardless of base
            var seg = p.replace(/\/+$/, "").split("/").slice(-2).join("/");
            return seg && cur.indexOf(seg) >= 0;
        } catch (e) { return false; }
    }

    // Pull a likely filter value out of an intent: explicit value, an entity, or a
    // number / date spoken in the transcript ("order 835968", "30 april").
    function filterValueFrom(intent) {
        if (!intent) return "";
        var v = (intent.value || "").trim();
        if (v && !/^(clear|all|sab|reset)$/.test(normalize(v))) return v;
        var e = pickEntity(intent.entities);
        if (e) return e;
        var tr = intent.transcript || "";
        var num = (tr.match(/\b\d{3,}\b/) || [])[0];
        if (num) return num;
        return "";
    }

    // Months map so spoken "30 april" matches a table cell like "30-Apr-2026".
    var MONTHS = { january: "jan", february: "feb", march: "mar", april: "apr", may: "may",
        june: "jun", july: "jul", august: "aug", september: "sep", october: "oct",
        november: "nov", december: "dec" };
    function monthNorm(s) {
        s = normalize(s);
        for (var full in MONTHS) if (s.indexOf(full) >= 0) s = s.replace(new RegExp(full, "g"), MONTHS[full]);
        return s;
    }

    function pickEntity(entities) {
        if (!entities) return "";
        return entities.memberId || entities.invoice || entities.voucher || entities.mobile ||
            entities.name || entities.party || entities.product || entities.status ||
            entities.dateFrom || entities.amount || "";
    }

    // Guess the report page for a value spoken from the dashboard (so "order 12345 dikhao"
    // works from anywhere). Returns a route URL or null.
    function guessReportPage(text) {
        var n = normalize(text);
        if (/order|myorder|orders/.test(n)) return "/Home/Myorders";
        if (/ticket|complaint|shikayat/.test(n)) return "/Account/TicketList";
        if (/wallet/.test(n)) return "/Home/WalletDetail";
        if (/coupon/.test(n)) return "/Home/MyCoupon";
        if (/commission|income/.test(n)) return "/Home/LevelIncome";
        if (/direct|level/.test(n)) return "/Home/Levelwisereport";
        return null;
    }

    // Main filter entry point. column = column/page hint, value = value to filter by.
    function applyFilter(column, value, entities, intent) {
        var val = value || pickEntity(entities);
        // "clear / sab dikhao / filter hatao" -> reset
        if (val && /^(clear|all|sab|sabhi|reset|hatao|saaf|remove)$/.test(normalize(val))) {
            clearTableFilter(); speak("Filter hata diya."); return;
        }
        if (!val) { clearTableFilter(); speak("Filter value nahi mili."); return; }

        // 1) If the current page already shows a data table, filter HERE (never navigate away).
        //    Otherwise, jump to the right report page first and apply the filter on load
        //    (dashboard se ek hi command: navigate + filter).
        var hasTable = !!pickReportTable();
        if (!hasTable) {
            var pageHint = (intent && intent.page) ? resolveRoute(intent.page) : null;
            if (!pageHint) {
                var g = guessReportPage((column || "") + " " + ((intent && intent.transcript) || "") + " " + ((entities && entities.status) || ""));
                if (g) pageHint = appUrl(g);
            }
            if (pageHint && !samePage(pageHint)) {
                sessionStorage.setItem("va_pendingFilter", JSON.stringify({ val: val }));
                speak("Report khol kar filter laga rahe hain.");
                location.href = pageHint;
                return;
            }
        }

        // 2) DataTables / named search box, if present (server-side or built-in search)
        var box = document.querySelector(".dataTables_filter input")
            || document.querySelector('input[type=search]')
            || document.querySelector('input[name*="search" i], input[id*="search" i], input[placeholder*="search" i]');
        if (box) {
            setValueAndFire(box, val);
            box.dispatchEvent(new KeyboardEvent("keyup", { bubbles: true, key: "Enter", keyCode: 13 }));
            return;
        }

        // 3) Generic client-side row filter on the report table (no search box needed).
        var n = filterTableRows(val);
        speak(n > 0 ? (n + " result mile.") : ("Koi record nahi mila: " + val));
    }

    // Show only rows whose text matches `val`; hide the rest. Works on any HTML table.
    // Returns the number of matching rows. Adds a small banner with a Clear button.
    function filterTableRows(val) {
        var table = pickReportTable();
        if (!table) { speak("Is page par koi table nahi mila."); return 0; }
        var q = monthNorm(val);
        var qtokens = q.split(" ").filter(Boolean);
        var rows = Array.prototype.slice.call(table.querySelectorAll("tbody tr"));
        if (!rows.length) rows = Array.prototype.slice.call(table.querySelectorAll("tr")).filter(function (r) {
            return !r.querySelector("th");   // skip header row
        });

        var matchCount = 0, firstMatch = null;
        rows.forEach(function (r) {
            var txt = monthNorm(r.textContent || "");
            var hit = txt.indexOf(q) >= 0 || (qtokens.length > 0 && qtokens.every(function (t) { return txt.indexOf(t) >= 0; }));
            r.style.display = hit ? "" : "none";
            r.classList.toggle("va-hl", hit);
            if (hit) { matchCount++; if (!firstMatch) firstMatch = r; }
        });

        showFilterBanner(val, matchCount);
        if (firstMatch && firstMatch.scrollIntoView) firstMatch.scrollIntoView({ behavior: "smooth", block: "center" });
        return matchCount;
    }

    // Largest visible table = the report grid.
    function pickReportTable() {
        var tables = Array.prototype.slice.call(document.querySelectorAll("table")).filter(isVisible);
        if (!tables.length) return null;
        tables.sort(function (a, b) { return b.querySelectorAll("tr").length - a.querySelectorAll("tr").length; });
        return tables[0];
    }

    function clearTableFilter() {
        var table = pickReportTable();
        if (table) Array.prototype.slice.call(table.querySelectorAll("tr")).forEach(function (r) {
            r.style.display = ""; r.classList.remove("va-hl");
        });
        var b = document.getElementById("va-filter-banner");
        if (b) b.parentNode.removeChild(b);
    }

    function showFilterBanner(val, count) {
        var b = document.getElementById("va-filter-banner");
        if (!b) {
            b = document.createElement("div");
            b.id = "va-filter-banner";
            (pickReportTable() ? pickReportTable().parentNode : document.body).insertBefore(b, pickReportTable());
        }
        b.innerHTML = "";
        var span = document.createElement("span");
        span.textContent = "🔎 Filter: \u201C" + val + "\u201D — " + count + " result";
        var btn = document.createElement("button");
        btn.type = "button"; btn.textContent = "Clear"; btn.className = "va-clear";
        btn.addEventListener("click", clearTableFilter);
        b.appendChild(span); b.appendChild(btn);
    }

    // Run a pending filter after navigating to a report page (dashboard -> report -> filter).
    function runPendingFilter() {
        var raw = sessionStorage.getItem("va_pendingFilter");
        if (!raw) return;
        sessionStorage.removeItem("va_pendingFilter");
        try {
            var p = JSON.parse(raw);
            if (p && p.val) setTimeout(function () {
                var n = filterTableRows(p.val);
                speak(n > 0 ? (n + " result mile.") : ("Record nahi mila: " + p.val));
            }, 700);
        } catch (e) { }
    }

    // Add to cart. On a product page we drive the existing CheckLoginPopUp flow;
    // elsewhere we open the matching product first.
    function cartAdd(product, qty) {
        var qtyInput = document.getElementById("Qty");
        var addLink = document.querySelector('[href^="javascript:CheckLoginPopUp"]')
            || document.querySelector('a[onclick*="CheckLoginPopUp"]');

        if (qtyInput && (addLink || typeof window.CheckLoginPopUp === "function")) {
            if (qty && qty > 0) {
                var inc = document.getElementById("increase");
                var cur = parseInt(qtyInput.value, 10) || 1;
                if (inc && qty > cur) clickN(inc, qty - cur);
                else { qtyInput.value = qty; setValueAndFire(qtyInput, String(qty)); }
            }
            if (addLink) addLink.click();
            else window.CheckLoginPopUp("DetailBuy");
            // After adding, take the user to the cart so they can see it / continue
            // (as requested: "add to cart hone ke baad us page par bhi jaaye").
            speak("Cart me add kar diya. Cart khol rahe hain.");
            setTimeout(function () { location.href = appUrl("/ViewCart/ViewCart"); }, 1600);
            return;
        }

        // Not on a product page -> find the product link (matches product-card image alt
        // text too) and open it; the add then fires automatically via runPendingAdd().
        var link = bestMatch(product, collectMenuLinks(), function (l) { return l.text; }, 0.4);
        if (link) {
            sessionStorage.setItem("va_pendingAdd", JSON.stringify({ qty: qty || 1 }));
            speak(product + " khol rahe hain.");
            location.href = link.item.href;
        } else {
            speak("Product nahi mila: " + product + ". Pehle product page kholiye.");
        }
    }

    function cartQty(product, dir, qty, position) {
        var row = findCartRow(product, position);
        if (!row) { speak("Cart row nahi mila."); return; }
        var plus = row.querySelector(".btn-plus"), minus = row.querySelector(".btn-minus");
        var input = row.querySelector("input.quantity, input[id^='qtyText']");

        if (qty && qty > 0 && input) {                       // "set quantity to N"
            var cur = parseInt(input.value, 10) || 1;
            if (qty > cur && plus) clickN(plus, qty - cur);
            else if (qty < cur && minus) clickN(minus, cur - qty);
        } else if (/minus|ghat|kam|decrease|reduce/.test(normalize(dir))) {
            if (minus) minus.click();
        } else {                                              // default: increase by one
            if (plus) plus.click();
        }
    }

    // Run a pending add-to-cart after navigating to a product page.
    function runPendingAdd() {
        var raw = sessionStorage.getItem("va_pendingAdd");
        if (!raw) return;
        sessionStorage.removeItem("va_pendingAdd");
        try {
            var p = JSON.parse(raw);
            if (document.getElementById("Qty")) setTimeout(function () { cartAdd("", p.qty); }, 900);
        } catch (e) { }
    }

    // ======================================================================= SERVER
    function sendToServer(transcript) {
        var ctx = buildContext();
        flash(transcript, "Soch rahe hain...");
        var form = "transcript=" + encodeURIComponent(transcript) + "&context=" + encodeURIComponent(ctx);

        fetch(CFG.commandUrl, {
            method: "POST",
            headers: { "Content-Type": "application/x-www-form-urlencoded" },
            body: form,
            credentials: "same-origin"
        })
            .then(function (r) { return r.json(); })
            .then(function (intent) { execute(intent); })
            .catch(function () { execute(fallbackParse(transcript)); });   // offline fallback
    }

    // A small page-context string helps the model disambiguate (current page + a few links).
    function buildContext() {
        var links = collectMenuLinks().slice(0, 25).map(function (l) { return l.text; });
        var onCart = !!document.getElementById("cartTable");
        var onProduct = !!document.getElementById("Qty");
        var onCheckout = !!document.getElementById("DeliveryAddressID");
        return "path=" + location.pathname +
            "; onCart=" + onCart + "; onProduct=" + onProduct + "; onCheckout=" + onCheckout +
            "; links=" + links.join(", ");
    }

    // Deterministic client fallback for core navigation/cart when the server is down.
    function fallbackParse(text) {
        var n = normalize(text);
        if (/clear filter|filter hatao|filter saaf|sab dikhao|sabhi dikhao|reset filter/.test(n)) return { action: "filter", value: "clear", speak: "Filter hata rahe hain." };
        if (/cart.*(kholo|dikhao|view|show)|view cart|my cart/.test(n)) return { action: "cart_view", speak: "Cart khol rahe hain." };
        if (/checkout|check out|aage badho/.test(n)) return { action: "checkout", speak: "Checkout." };
        if (/place order|proceed to pay|order.*(place|kar do|kar)|pay/.test(n)) return { action: "place_order", speak: "Proceed kar rahe hain." };
        if (/(increase|badhao|plus).*quantity|quantity.*(increase|badhao)/.test(n)) return { action: "cart_qty", value: "plus", speak: "Quantity badha rahe hain." };
        if (/(decrease|ghatao|kam|minus).*quantity|quantity.*(decrease|ghatao)/.test(n)) return { action: "cart_qty", value: "minus", speak: "Quantity ghata rahe hain." };
        if (/remove first|pehla.*hatao|first product.*remove/.test(n)) return { action: "cart_remove", position: "first", speak: "Pehla product hata rahe hain." };
        // "order 835968 dikhao" / "complaint 10010" -> filter (a number + a report keyword)
        var num = (n.match(/\b\d{3,}\b/) || [])[0];
        if (num && /(order|ticket|complaint|invoice|voucher|member|wallet)/.test(n)) {
            return { action: "filter", value: num, transcript: text, speak: num + " dhoondh rahe hain." };
        }
        var r = bestMatch(n, ROUTE_MAP, function (x) { return x.aliases; }, 0.5);
        if (r) return { action: "navigate", target: r.matched, speak: "Khol rahe hain." };
        return { action: "speak", speak: "Server unavailable. Sirf basic navigation chal rahi hai." };
    }

    // ======================================================================= SPEECH IN
    function initRecognition() {
        var SR = window.SpeechRecognition || window.webkitSpeechRecognition;
        if (!SR) { return null; }
        var r = new SR();
        r.lang = CFG.lang;
        r.interimResults = false;
        r.maxAlternatives = 1;
        r.continuous = false;
        r.onstart = function () { listening = true; setMicState(true); };
        r.onend = function () { listening = false; setMicState(false); };
        r.onerror = function (e) { listening = false; setMicState(false); flash("", "Mic error: " + e.error); };
        r.onresult = function (e) {
            var transcript = (e.results[0][0].transcript || "").trim();
            if (!transcript) return;
            // Performance: ignore an identical command repeated within 1.5s (echo / double fire).
            var now = Date.now();
            if (transcript === lastTranscript && (now - lastTranscriptAt) < 1500) return;
            lastTranscript = transcript; lastTranscriptAt = now;
            lastLang = detectLang(transcript);     // so replies come back in the same language
            sendToServer(transcript);
        };
        return r;
    }

    function toggleMic() {
        if (!recognition) recognition = initRecognition();
        if (!recognition) { alert("Aapka browser voice recognition support nahi karta. Chrome use karein."); return; }
        if (listening) { recognition.stop(); }
        else { try { recognition.start(); } catch (e) { } }
    }

    // ======================================================================= UI (FAB)
    function injectUI() {
        if (document.getElementById("va-fab")) return;

        var style = document.createElement("style");
        style.textContent =
            // --- Floating mic button: indigo -> violet -> fuchsia, glassy ring, gentle float ---
            "#va-fab{position:fixed;right:22px;bottom:22px;width:62px;height:62px;border-radius:50%;border:none;" +
            "cursor:pointer;z-index:2147483000;display:flex;align-items:center;justify-content:center;overflow:visible;" +
            "background:linear-gradient(135deg,#6366f1 0%,#8b5cf6 55%,#d946ef 100%);" +
            "box-shadow:0 8px 26px rgba(124,58,237,.5),0 0 0 4px rgba(255,255,255,.14);" +
            "animation:va-float 3.2s ease-in-out infinite;transition:transform .18s cubic-bezier(.34,1.56,.64,1);}" +
            "#va-fab:hover{transform:scale(1.1) rotate(-5deg);}" +
            "#va-fab:active{transform:scale(.93);}" +
            // ambient idle ring
            "#va-fab::before{content:'';position:absolute;inset:-5px;border-radius:50%;" +
            "border:2px solid rgba(139,92,246,.40);}" +
            // the mic glyph
            "#va-fab .va-mic{width:28px;height:28px;fill:#fff;position:relative;z-index:2;" +
            "filter:drop-shadow(0 1px 2px rgba(0,0,0,.28));}" +
            "@keyframes va-float{0%,100%{transform:translateY(0)}50%{transform:translateY(-5px)}}" +
            // --- LISTENING: red gradient, ripple rings, equalizer bars replace the mic ---
            "#va-fab.listening{background:linear-gradient(135deg,#ef4444,#f43f5e,#fb7185);animation:none;}" +
            "#va-fab.listening::before,#va-fab.listening::after{content:'';position:absolute;inset:0;border-radius:50%;" +
            "border:2px solid rgba(244,63,94,.6);animation:va-ripple 1.4s ease-out infinite;}" +
            "#va-fab.listening::after{animation-delay:.7s;}" +
            "@keyframes va-ripple{0%{transform:scale(1);opacity:.7}100%{transform:scale(2.15);opacity:0}}" +
            "#va-fab.listening .va-mic{display:none;}" +
            ".va-eq{display:none;align-items:flex-end;gap:3px;height:24px;position:relative;z-index:2;}" +
            "#va-fab.listening .va-eq{display:flex;}" +
            ".va-eq i{width:3px;background:#fff;border-radius:2px;transform-origin:bottom;" +
            "animation:va-bar .9s ease-in-out infinite;}" +
            ".va-eq i:nth-child(1){height:9px;animation-delay:0s}" +
            ".va-eq i:nth-child(2){height:17px;animation-delay:.16s}" +
            ".va-eq i:nth-child(3){height:24px;animation-delay:.32s}" +
            ".va-eq i:nth-child(4){height:15px;animation-delay:.48s}" +
            ".va-eq i:nth-child(5){height:10px;animation-delay:.22s}" +
            "@keyframes va-bar{0%,100%{transform:scaleY(.35)}50%{transform:scaleY(1)}}" +
            // toast
            "#va-toast{position:fixed;right:22px;bottom:96px;max-width:300px;background:#111827;color:#fff;" +
            "padding:10px 14px;border-radius:12px;font:13px/1.4 system-ui,Arial,sans-serif;z-index:2147483000;" +
            "box-shadow:0 6px 20px rgba(0,0,0,.3);opacity:0;transform:translateY(8px);transition:.2s;pointer-events:none;}" +
            "#va-toast.show{opacity:1;transform:translateY(0);}" +
            "#va-toast b{color:#a5b4fc;}" +
            // filtered-row highlight + filter banner
            "tr.va-hl{background:rgba(79,70,229,.10) !important;}" +
            "#va-filter-banner{display:flex;align-items:center;justify-content:space-between;gap:12px;" +
            "margin:0 0 10px;padding:8px 14px;background:#eef2ff;border:1px solid #c7d2fe;border-radius:10px;" +
            "font:600 13px/1.3 system-ui,Arial,sans-serif;color:#3730a3;}" +
            "#va-filter-banner .va-clear{border:none;background:#4f46e5;color:#fff;border-radius:8px;" +
            "padding:5px 12px;cursor:pointer;font:600 12px system-ui,Arial;}" +
            "#va-filter-banner .va-clear:hover{background:#4338ca;}" +
            "@media (prefers-reduced-motion:reduce){#va-fab{animation:none}.va-eq i{animation:none}}";
        document.head.appendChild(style);

        var fab = document.createElement("button");
        fab.id = "va-fab";
        fab.type = "button";
        fab.title = "Voice Assistant (Hindi / Hinglish / English)";
        fab.setAttribute("aria-label", "Voice Assistant");
        fab.innerHTML =
            '<svg class="va-mic" viewBox="0 0 24 24" aria-hidden="true">' +
            '<path d="M12 14a3 3 0 0 0 3-3V6a3 3 0 0 0-6 0v5a3 3 0 0 0 3 3z"/>' +
            '<path d="M17 11a5 5 0 0 1-10 0H5a7 7 0 0 0 6 6.92V21H8v2h8v-2h-3v-3.08A7 7 0 0 0 19 11h-2z"/>' +
            '</svg>' +
            '<span class="va-eq"><i></i><i></i><i></i><i></i><i></i></span>';
        fab.addEventListener("click", toggleMic);
        document.body.appendChild(fab);

        var toast = document.createElement("div");
        toast.id = "va-toast";
        document.body.appendChild(toast);
    }

    var toastTimer = null;
    function flash(heard, msg) {
        var toast = document.getElementById("va-toast");
        if (!toast) return;
        toast.innerHTML = (heard ? "<b>\u201C" + escapeHtml(heard) + "\u201D</b><br>" : "") + escapeHtml(msg || "");
        toast.classList.add("show");
        clearTimeout(toastTimer);
        toastTimer = setTimeout(function () { toast.classList.remove("show"); }, 4000);
    }
    function escapeHtml(s) { return (s || "").replace(/[&<>"]/g, function (c) { return ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;" })[c]; }); }
    function setMicState(on) { var f = document.getElementById("va-fab"); if (f) f.classList.toggle("listening", !!on); }

    // After a successful login, return to the page the user originally asked for.
    function runAfterLogin() {
        var dest = sessionStorage.getItem("va_afterLogin");
        if (dest && isLoggedIn() && !onLoginPage()) {
            sessionStorage.removeItem("va_afterLogin");
            safeNavigate(dest, 200);
        } else if (dest && isLoggedIn() && onLoginPage()) {
            // still on login page but already authenticated in another tab — clear stale flag
            sessionStorage.removeItem("va_afterLogin");
        }
    }

    // ======================================================================= BOOTSTRAP
    function boot() { injectUI(); runPendingAdd(); runPendingFilter(); runAfterLogin(); }
    if (document.readyState === "loading") document.addEventListener("DOMContentLoaded", boot);
    else boot();
    // Warm up TTS voices (Chrome loads them async).
    if ("speechSynthesis" in window) window.speechSynthesis.getVoices();
})();
