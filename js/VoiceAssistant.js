///* ============================================================
// *  VoiceAssistant.js  -  Admin Voice Assistant (Hinglish)
// *  Project: Basic-MLM-ADMIN  (WebForms, VB.NET)
// *
// *  Pipeline:
// *    mic -> Web Speech API (hi-IN) -> mode-detect (short/full)
// *        -> POST VoiceAI.ashx (Mistral) -> JSON intent
// *        -> executor (navigate/fill/submit/read/clear/home/logout/help)
// *        -> SpeechSynthesis feedback (Hinglish)
// *
// *  Live lists (menus/fields/buttons) are scanned from the CURRENT
// *  page DOM on every request, so it works on any of the ~500 pages
// *  without per-page config.
// * ============================================================ */
//(function () {
//    "use strict";

//    /* ---------------- CONFIG ---------------- */
//    var CFG = {
//        endpoint: "VoiceAI.ashx",       // same-folder handler
//        lang: "hi-IN",                  // recognition language (Hinglish works well on hi-IN)
//        ttsLang: "hi-IN",               // synthesis language
//        homeUrl: "Home.aspx",
//        logoutUrl: "logout.aspx",
//        dateFormat: "dd/MM/yyyy",       // how dictated dates ("today"/"aaj") are written into fields
//        speak: true                     // spoken feedback on/off
//    };

//    /* canonical action buttons (from the Mistral prompt spec) */
//    var ACTION_BUTTONS = [
//        "Search", "Show All", "Show Detail", "View All", "Advanced Search",
//        "Export To Excel", "Export To CSV", "Print All Pages", "Print Current Page",
//        "Approve", "ApproveAll", "Reject", "RejectAll", "Send Sms",
//        "Confirm", "Save", "Paid", "Verification", "Back"
//    ];

//    /* ---------------- STATE ---------------- */
//    var recog = null, listening = false, busy = false;

//    /* ============================================================
//     *  UI  (built entirely in JS so the master needs only a <script>)
//     * ============================================================ */
//    function buildUI() {
//        if (document.getElementById("va-root")) return;

//        var css = ''
//            + '#va-root{position:fixed;right:22px;bottom:22px;z-index:99999;font-family:Arial,Helvetica,sans-serif;}'
//            + '#va-mic{width:60px;height:60px;border-radius:50%;border:none;cursor:pointer;'
//            + 'background:#00408e;color:#fff;font-size:26px;box-shadow:0 6px 18px rgba(0,0,0,.3);'
//            + 'transition:transform .15s,background .2s;}'
//            + '#va-mic:hover{transform:scale(1.06);}'
//            + '#va-mic.listening{background:#d9534f;animation:va-pulse 1.1s infinite;}'
//            + '@keyframes va-pulse{0%{box-shadow:0 0 0 0 rgba(217,83,79,.6);}70%{box-shadow:0 0 0 12px rgba(217,83,79,0);}100%{box-shadow:0 0 0 0 rgba(217,83,79,0);}}'
//            + '#va-toast{position:absolute;right:0;bottom:74px;max-width:280px;background:#00408e;color:#fff;'
//            + 'border-radius:10px;box-shadow:0 8px 24px rgba(0,0,0,.25);padding:9px 12px;font-size:13px;'
//            + 'line-height:1.4;opacity:0;transform:translateY(6px);pointer-events:none;transition:opacity .2s,transform .2s;}'
//            + '#va-toast.show{opacity:1;transform:translateY(0);}';

//        var st = document.createElement("style");
//        st.type = "text/css";
//        st.appendChild(document.createTextNode(css));
//        document.getElementsByTagName("head")[0].appendChild(st);

//        var root = document.createElement("div");
//        root.id = "va-root";
//        root.innerHTML =
//            '<div id="va-toast"></div>'
//          + '<button id="va-mic" title="Voice Assistant - bol kar command dijiye">&#127908;</button>';
//        document.body.appendChild(root);

//        // floating mic directly starts / stops listening
//        document.getElementById("va-mic").onclick = toggleListen;
//    }

//    var _toastTimer = null;
//    function showToast(t) {
//        var e = document.getElementById("va-toast");
//        if (!e || !t) return;
//        e.textContent = t;
//        e.classList.add("show");
//        clearTimeout(_toastTimer);
//        _toastTimer = setTimeout(function () { e.classList.remove("show"); }, 4500);
//    }
//    function setStatus(t) { showToast(t); }
//    function setSaid(t)   { if (t) showToast('"' + t + '"'); }
//    function showPanel()  { /* no panel anymore; feedback via toast */ }

//    /* ============================================================
//     *  SPEECH:  recognition + synthesis
//     * ============================================================ */
//    function getRecog() {
//        var SR = window.SpeechRecognition || window.webkitSpeechRecognition;
//        if (!SR) return null;
//        var r = new SR();
//        r.lang = CFG.lang;
//        r.interimResults = false;
//        r.maxAlternatives = 1;
//        r.continuous = false;
//        return r;
//    }

//    function isSecureOrigin() {
//        if (window.isSecureContext) return true;            // https or localhost
//        var h = location.hostname;
//        return (h === "localhost" || h === "127.0.0.1" || h === "::1");
//    }

//    function toggleListen() {
//        showPanel();
//        if (listening) { stopListen(); return; }

//        // Web Speech API only works on HTTPS or localhost. Plain http://<ip|domain> -> "not-allowed".
//        if (!isSecureOrigin()) {
//            var m = "Microphone ke liye HTTPS zaroori hai. Abhi site HTTP par chal rahi hai (" + location.protocol
//                  + "//" + location.host + "). Kripya site ko HTTPS par kholiye, ya localhost par test kijiye.";
//            setStatus(m); say("Microphone ke liye HTTPS zaroori hai.");
//            return;
//        }

//        if (!recog) recog = getRecog();
//        if (!recog) {
//            setStatus("Is browser me voice support nahi hai. Kripya Google Chrome ka istemaal kijiye.");
//            say("Yeh browser voice support nahi karta. Kripya Chrome ka istemaal kijiye.");
//            return;
//        }

//        // Request mic permission explicitly first -> cleaner prompt + precise errors.
//        setStatus("Microphone ki permission maang raha hoon…");
//        ensureMicPermission(function (err) {
//            if (err) {
//                var nm = err.name || "";
//                if (nm === "NotAllowedError" || nm === "SecurityError") {
//                    setStatus("Microphone ki permission band hai. Address bar ke icon par click karke Site settings me jaaiye, "
//                            + "Microphone ko Allow kijiye, aur phir page reload kijiye.");
//                } else if (nm === "NotFoundError" || nm === "DevicesNotFoundError") {
//                    setStatus("Koi microphone nahi mila. Microphone connect ya enable karke dobara koshish kijiye.");
//                } else {
//                    setStatus("Microphone access nahi mila (" + (nm || "error") + "). Kripya dobara koshish kijiye.");
//                }
//                return;
//            }
//            startRecognition();
//        });
//    }

//    function startRecognition() {
//        try { recog.start(); } catch (e) { /* already started */ }
//        listening = true;
//        document.getElementById("va-mic").classList.add("listening");
//        setStatus("Sun raha hoon, boliye…");
//        setSaid("");

//        recog.onresult = function (ev) {
//            var txt = ev.results[0][0].transcript || "";
//            setSaid(txt);
//            handleCommand(txt);
//        };
//        recog.onerror = function (ev) {
//            var code = ev.error || "error";
//            var msg;
//            switch (code) {
//                case "not-allowed":
//                case "service-not-allowed":
//                    msg = "Microphone band hai. Address bar ke icon par click karke Site settings me Microphone ko "
//                        + "Allow kijiye aur page reload kijiye. (Site HTTPS ya localhost par honi chahiye.)";
//                    break;
//                case "no-speech":
//                    msg = "Kuch sunai nahi diya. Microphone ke paas spasht bol kar dobara koshish kijiye.";
//                    break;
//                case "audio-capture":
//                    msg = "Microphone nahi mila. Kripya jaanch lijiye ki microphone connect aur enable hai.";
//                    break;
//                case "network":
//                    msg = "Network samasya hai. Internet connection jaanch kar dobara boliye.";
//                    break;
//                case "aborted":
//                    msg = "Sunna ruk gaya. Microphone button dobara dabaiye.";
//                    break;
//                default:
//                    msg = "Sun nahi paaya (" + code + "). Kripya dobara koshish kijiye.";
//            }
//            setStatus(msg);
//        };
//        recog.onend = function () {
//            listening = false;
//            document.getElementById("va-mic").classList.remove("listening");
//        };
//    }

//    function stopListen() {
//        try { if (recog) recog.stop(); } catch (e) {}
//        listening = false;
//        document.getElementById("va-mic").classList.remove("listening");
//    }

//    // Trigger a clean mic-permission prompt. cb(err) -> err is null on grant.
//    function ensureMicPermission(cb) {
//        if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
//            cb(null); // can't pre-check on this browser; let recognition try directly
//            return;
//        }
//        navigator.mediaDevices.getUserMedia({ audio: true }).then(function (stream) {
//            // we only needed the permission grant; release the mic immediately
//            try { stream.getTracks().forEach(function (t) { t.stop(); }); } catch (e) {}
//            cb(null);
//        }).catch(function (err) {
//            cb(err || new Error("denied"));
//        });
//    }

//    function say(text) {
//        if (!CFG.speak || !window.speechSynthesis) return;
//        try {
//            window.speechSynthesis.cancel();
//            var u = new SpeechSynthesisUtterance(text);
//            u.lang = CFG.ttsLang;
//            u.rate = 1;
//            window.speechSynthesis.speak(u);
//        } catch (e) {}
//    }

//    /* ============================================================
//     *  DOM SCAN:  build live menus / fields / buttons lists
//     * ============================================================ */

//    // strip WebForms server prefixes + Hungarian notation, humanize an id/name
//    function humanize(id) {
//        if (!id) return "";
//        var s = id.split("$").pop();          // ctl00$Content$txtX -> txtX
//        s = s.split("_").pop();               // ctl00_Content_txtX -> txtX
//        s = s.replace(/^(txt|ddl|dl|lbl|btn|chk|rdo|rb|cb|hdn|hf|img|lnk|lb|gv|rpt)/i, "");
//        s = s.replace(/([a-z0-9])([A-Z])/g, "$1 $2"); // camelCase -> spaced
//        s = s.replace(/[_\-]+/g, " ").trim();
//        return s;
//    }

//    function labelFor(el) {
//        // 1) explicit <label for=id>
//        if (el.id) {
//            var lab = document.querySelector('label[for="' + cssEsc(el.id) + '"]');
//            if (lab && txt(lab)) return txt(lab);
//        }
//        // 2) aria-label / placeholder / title
//        if (el.getAttribute("aria-label")) return el.getAttribute("aria-label").trim();
//        if (el.placeholder) return el.placeholder.trim();
//        if (el.title) return el.title.trim();
//        // 3) nearest preceding label-ish text in same cell/row
//        var cell = closest(el, "td, th, .form-group, .col-md-6, .col-md-4, .col-md-3, .row, div");
//        if (cell) {
//            var prev = cell.previousElementSibling;
//            if (prev && txt(prev) && txt(prev).length < 40) return txt(prev);
//            var lab2 = cell.querySelector("label, b, strong, span");
//            if (lab2 && txt(lab2) && txt(lab2).length < 40) return txt(lab2);
//        }
//        // 4) humanized id/name
//        return humanize(el.id || el.name);
//    }

//    function scanFields() {
//        var out = [];
//        var els = document.querySelectorAll(
//            'input[type=text],input[type=number],input[type=tel],input[type=email],' +
//            'input[type=date],input[type=search],input[type=password],input:not([type]),' +
//            'select,textarea');
//        for (var i = 0; i < els.length; i++) {
//            var el = els[i];
//            if (closest(el, "#va-root")) continue;    // skip our own command box
//            if (!isVisible(el)) continue;
//            if (/^__/.test(el.name || "")) continue; // skip __VIEWSTATE etc.
//            var lab = labelFor(el);
//            if (!lab) continue;
//            out.push({ el: el, label: lab });
//        }
//        return out;
//    }

//    function scanButtons() {
//        var out = [];
//        var els = document.querySelectorAll(
//            'input[type=submit],input[type=button],input[type=image],button,a.btn,a[role=button]');
//        for (var i = 0; i < els.length; i++) {
//            var el = els[i];
//            if (closest(el, "#va-root")) continue;    // skip our own mic/run/close buttons
//            if (!isVisible(el)) continue;
//            var label = (el.value || el.alt || el.title || txt(el) || "").trim();
//            if (!label) label = humanize(el.id || el.name);
//            if (!label) continue;
//            out.push({ el: el, label: label });
//        }
//        // also expose canonical action names so Mistral can map intent words
//        // even when the actual control text differs slightly.
//        return out;
//    }

//    function scanMenus() {
//        var out = [];
//        var anchors = document.querySelectorAll('#menu a[href], #sidebar-menu a[href], .site_title[href]');
//        for (var i = 0; i < anchors.length; i++) {
//            var a = anchors[i];
//            var href = a.getAttribute("href");
//            if (!href || href === "#") continue;
//            var label = txt(a) || humanize(href.replace(/\.aspx.*$/i, ""));
//            if (!label) continue;
//            out.push({ el: a, label: label, href: href });
//        }
//        return out;
//    }

//    function listLabels(arr) {
//        var seen = {}, res = [];
//        for (var i = 0; i < arr.length; i++) {
//            var l = arr[i].label;
//            if (l && !seen[l.toLowerCase()]) { seen[l.toLowerCase()] = 1; res.push(l); }
//        }
//        return res;
//    }

//    /* ============================================================
//     *  COMMAND FLOW
//     * ============================================================ */
//    function detectMode(raw) {
//        // fill/value commands -> full ; navigate/search/etc -> short
//        return /bharo|fill|likho|set|naam|mobile|email|address|date|amount|daalo/i.test(raw)
//            ? "full" : "short";
//    }

//    function handleCommand(raw) {
//        if (busy) return;
//        raw = (raw || "").trim();
//        if (!raw) return;

//        // quick offline shortcuts (no AI round-trip needed)
//        if (/^(help|madad|kya kar sakte)/i.test(raw)) { return doHelp(); }

//        // offline fill: "subject test" / "subject mein test likho" -> fill instantly
//        var localFields = scanFields();
//        var lf = tryLocalFill(raw, localFields);
//        if (lf) {
//            if (setValue(lf.el, normalizeValue(lf.value))) {
//                setStatus("Bhar diya: " + lf.label + " = " + lf.value);
//                say(lf.label + " bhar diya.");
//            } else {
//                setStatus(lf.label + " bhar nahi paaya.");
//            }
//            return;
//        }

//        busy = true;
//        setStatus("Samajh raha hoon…");

//        var menus = scanMenus(), fields = scanFields(), buttons = scanButtons();

//        var body = "raw=" + encodeURIComponent(raw)
//                 + "&mode=" + encodeURIComponent(detectMode(raw))
//                 + "&menus=" + encodeURIComponent(listLabels(menus).join("\n"))
//                 + "&fields=" + encodeURIComponent(listLabels(fields).join("\n"))
//                 + "&buttons=" + encodeURIComponent(listLabels(buttons).concat(ACTION_BUTTONS).join("\n"));

//        postForm(CFG.endpoint, body, function (err, text) {
//            busy = false;
//            if (err) { setStatus("Server se connection nahi ho paya."); say("Server se connection nahi ho paya."); return; }
//            var data = safeJson(text);
//            if (!data) { setStatus("Jawab samajh nahi aaya."); say("Maaf kijiye, main samajh nahi paaya."); return; }
//            execute(data, { menus: menus, fields: fields, buttons: buttons });
//        });
//    }

//    function execute(data, ctx) {
//        var intent = (data.intent || "").toLowerCase();
//        switch (intent) {
//            case "navigate": return doNavigate(data.target, ctx.menus);
//            case "fill":     return doFill(data.items || [], ctx.fields);
//            case "submit":   return doSubmit(data.button, ctx.buttons);
//            case "read":     return doRead(ctx.fields);
//            case "clear":    return doClear(data.field, ctx.fields);
//            case "home":     return go(CFG.homeUrl, "Home khol raha hoon.");
//            case "logout":   return go(CFG.logoutUrl, "Logout kar raha hoon.");
//            case "help":     return doHelp();
//            default:
//                var msg = data.message || "Samajh nahi aaya, dobara boliye.";
//                setStatus(msg); say(msg);
//        }
//    }

//    /* ---- intent handlers ---- */
//    function doNavigate(target, menus) {
//        if (!target) { setStatus("Aapko kahan jaana hai?"); return; }
//        var m = bestMatch(target, menus);
//        if (m && m.href) { go(m.href, m.label + " khol raha hoon."); }
//        else { setStatus('"' + target + '" naam ka menu nahi mila.'); say(target + " naam ka menu nahi mila."); }
//    }

//    function doFill(items, fields) {
//        if (!items.length) { setStatus("Kya bharna hai, kripya bataiye?"); return; }
//        var done = [];
//        for (var i = 0; i < items.length; i++) {
//            var it = items[i];
//            var f = bestMatch(it.field, fields);
//            if (!f) continue;
//            if (setValue(f.el, normalizeValue(it.value))) done.push(f.label);
//        }
//        if (done.length) { setStatus("Bhar diya: " + done.join(", ")); say(done.join(", ") + " bhar diya gaya hai."); }
//        else { setStatus("Koi matching field nahi mili."); say("Koi matching field nahi mili."); }
//    }

//    function doSubmit(buttonText, buttons) {
//        var b;
//        if (buttonText && buttonText.trim()) {
//            b = bestMatch(buttonText, buttons);
//        } else {
//            // empty -> first primary submit on page
//            b = buttons[0];
//        }
//        if (b && b.el) {
//            setStatus((b.label || "Button") + " button click kar raha hoon.");
//            say((b.label || "Button") + " kar raha hoon.");
//            try { b.el.click(); } catch (e) { setStatus("Button click nahi ho paya."); }
//        } else {
//            setStatus('"' + (buttonText || "") + '" naam ka button nahi mila.');
//            say("Koi matching button nahi mila.");
//        }
//    }

//    function doRead(fields) {
//        var parts = [];
//        for (var i = 0; i < fields.length && parts.length < 12; i++) {
//            var v = readValue(fields[i].el);
//            if (v) parts.push(fields[i].label + ": " + v);
//        }
//        if (!parts.length) { setStatus("Is page par koi bhari hui field nahi mili."); say("Koi value nahi mili."); return; }
//        var s = parts.join(", ");
//        setStatus(s); say(s);
//    }

//    function doClear(field, fields) {
//        if (!field || field.toLowerCase() === "all") {
//            for (var i = 0; i < fields.length; i++) setValue(fields[i].el, "");
//            setStatus("Poora form khali kar diya gaya hai."); say("Poora form khali kar diya gaya hai.");
//            return;
//        }
//        var f = bestMatch(field, fields);
//        if (f) { setValue(f.el, ""); setStatus(f.label + " field khali kar diya."); say(f.label + " khali kar diya."); }
//        else { setStatus("Koi matching field nahi mili."); say("Koi matching field nahi mili."); }
//    }

//    function doHelp() {
//        var msg = "Aap keh sakte hain: koi page kholo, kisi field me value bharo, search karo, "
//                + "Excel me export karo, approve ya reject karo, form khali karo, home par chalo, ya logout karo.";
//        setStatus(msg); say(msg);
//    }

//    /* ---- value helpers ---- */
//    function normalizeValue(v) {
//        if (v == null) return "";
//        v = ("" + v).trim();
//        if (/^(today|aaj|abhi|aj)$/i.test(v)) return formatDate(new Date());
//        if (/^(kal|tomorrow)$/i.test(v))      { var d = new Date(); d.setDate(d.getDate() + 1); return formatDate(d); }
//        if (/^(yesterday|beeta kal)$/i.test(v)) { var y = new Date(); y.setDate(y.getDate() - 1); return formatDate(y); }
//        return v;
//    }

//    function formatDate(d) {
//        var dd = ("0" + d.getDate()).slice(-2);
//        var mm = ("0" + (d.getMonth() + 1)).slice(-2);
//        var yy = d.getFullYear();
//        if (CFG.dateFormat === "MM/dd/yyyy") return mm + "/" + dd + "/" + yy;
//        if (CFG.dateFormat === "dd-MM-yyyy") return dd + "-" + mm + "-" + yy;
//        if (CFG.dateFormat === "yyyy-MM-dd") return yy + "-" + mm + "-" + dd;
//        return dd + "/" + mm + "/" + yy; // default dd/MM/yyyy
//    }

//    function setValue(el, val) {
//        if (!el) return false;
//        var tag = (el.tagName || "").toLowerCase();
//        if (tag === "select") {
//            var matched = false;
//            for (var i = 0; i < el.options.length; i++) {
//                if (norm(el.options[i].text) === norm(val) ||
//                    (val && norm(el.options[i].text).indexOf(norm(val)) >= 0)) {
//                    el.selectedIndex = i; matched = true; break;
//                }
//            }
//            fire(el, "change");
//            return matched;
//        }
//        el.value = val;
//        fire(el, "input"); fire(el, "change"); fire(el, "keyup");
//        return true;
//    }

//    function readValue(el) {
//        var tag = (el.tagName || "").toLowerCase();
//        if (tag === "select") return el.options.length ? el.options[el.selectedIndex].text : "";
//        return el.value || "";
//    }

//    /* ============================================================
//     *  FUZZY MATCH  (Levenshtein + token overlap)
//     * ============================================================ */
//    function bestMatch(query, arr) {
//        if (!query || !arr || !arr.length) return null;
//        var q = norm(query), best = null, bestScore = -1;
//        for (var i = 0; i < arr.length; i++) {
//            var cand = norm(arr[i].label);
//            var score = simScore(q, cand);
//            if (score > bestScore) { bestScore = score; best = arr[i]; }
//        }
//        // require a reasonable threshold to avoid wild guesses
//        return bestScore >= 0.45 ? best : null;
//    }

//    // Score a single candidate string against all field labels.
//    function scoreField(text, fields) {
//        var q = norm(text), best = null, lbl = "", bestScore = 0;
//        for (var i = 0; i < fields.length; i++) {
//            var sc = simScore(q, norm(fields[i].label));
//            if (sc > bestScore) { bestScore = sc; best = fields[i].el; lbl = fields[i].label; }
//        }
//        return { el: best, label: lbl, score: bestScore };
//    }

//    // Offline fill parser. Handles BOTH:
//    //   "subject mein test likho"   (connector + optional verb)
//    //   "subject test"              (bare: <field> <value>, no verb)
//    // Returns {el, label, value} or null. Runs before any AI round-trip so it
//    // works instantly and even without a Mistral key.
//    function tryLocalFill(raw, fields) {
//        if (!fields || !fields.length) return null;
//        var s = (raw || "").trim();
//        if (!s) return null;

//        // Never treat navigation / page / logout commands as a fill.
//        if (/\b(kholo|kolo|khol|open|jaao|jao|chalo|chalu|dikhao|show|page|logout|home|ghar)\b/i.test(s)) return null;

//        // Strip trailing helper verbs, then bail if the whole thing is just a
//        // button action word (e.g. "search karo", "save", "show all dikhao").
//        var helperStripped = s.replace(/\s*(kar do|kardo|karo|kar|kijiye|dijiye|do|dikha do|dikhao)\s*$/i, "").trim();
//        if (/^(search|save|reset|cancel|update|delete|approve|approveall|reject|rejectall|print|print all pages|print current page|export|export to excel|export to csv|submit|confirm|paid|verification|back|show all|show detail|view all|advanced search|send sms)$/i.test(helperStripped)) {
//            return null;
//        }

//        var fillVerb = /\s*(likho|likhe|likh do|likhna|likhdo|bharo|bhar do|bhardo|daalo|dalo|daal do|type karo|type kar|type|fill karo|fill|set karo|set|enter karo|enter)\s*$/i;
//        var hadVerb = fillVerb.test(s);
//        var core = s.replace(fillVerb, "").trim();
//        if (!core) return null;

//        // 1) connector form: "<field> mein/me/par/= <value>"
//        var conn = core.match(/^(.*?)\s+(?:mein|me|men|par|pe|=|:)\s+(.+)$/i);
//        if (conn) {
//            var cf = scoreField(conn[1].trim(), fields);
//            if (cf.el && cf.score >= 0.5) return { el: cf.el, label: cf.label, value: conn[2].trim() };
//        }

//        // 2) bare form: first k words = field, remaining = value
//        var words = core.split(/\s+/);
//        if (words.length >= 2) {
//            var bestEl = null, bestLbl = "", bestScore = 0, bestVal = "";
//            for (var k = 1; k < words.length; k++) {
//                var cand = words.slice(0, k).join(" ");
//                var m = scoreField(cand, fields);
//                if (m.score > bestScore) {
//                    bestScore = m.score; bestEl = m.el; bestLbl = m.label;
//                    bestVal = words.slice(k).join(" ");
//                }
//            }
//            var threshold = hadVerb ? 0.55 : 0.62;   // looser when a fill-verb was spoken
//            if (bestEl && bestScore >= threshold && bestVal) {
//                return { el: bestEl, label: bestLbl, value: bestVal };
//            }
//        }
//        return null;
//    }

//    function simScore(a, b) {
//        if (!a || !b) return 0;
//        if (a === b) return 1;
//        if (b.indexOf(a) >= 0 || a.indexOf(b) >= 0) return 0.9;
//        // token overlap
//        var ta = a.split(" "), tb = b.split(" "), hit = 0;
//        for (var i = 0; i < ta.length; i++) if (tb.indexOf(ta[i]) >= 0) hit++;
//        var overlap = hit / Math.max(ta.length, tb.length);
//        // normalized edit distance
//        var lev = 1 - (levenshtein(a, b) / Math.max(a.length, b.length));
//        return Math.max(overlap, lev);
//    }

//    function levenshtein(a, b) {
//        var m = a.length, n = b.length, d = [], i, j;
//        for (i = 0; i <= m; i++) d[i] = [i];
//        for (j = 0; j <= n; j++) d[0][j] = j;
//        for (i = 1; i <= m; i++)
//            for (j = 1; j <= n; j++)
//                d[i][j] = Math.min(d[i - 1][j] + 1, d[i][j - 1] + 1,
//                          d[i - 1][j - 1] + (a.charAt(i - 1) === b.charAt(j - 1) ? 0 : 1));
//        return d[m][n];
//    }

//    /* ============================================================
//     *  small utils
//     * ============================================================ */
//    function go(url, msg) { if (msg) { setStatus(msg); say(msg); } setTimeout(function () { window.location.href = url; }, 350); }
//    function txt(el) { return (el && (el.textContent || el.innerText) || "").replace(/\s+/g, " ").trim(); }
//    function norm(s) { return ("" + (s || "")).toLowerCase().replace(/[^a-z0-9\u0900-\u097F ]+/g, "").replace(/\s+/g, " ").trim(); }
//    function fire(el, type) {
//        try {
//            var ev;
//            if (typeof Event === "function") ev = new Event(type, { bubbles: true });
//            else { ev = document.createEvent("HTMLEvents"); ev.initEvent(type, true, true); }
//            el.dispatchEvent(ev);
//        } catch (e) {}
//    }
//    function isVisible(el) {
//        if (!el) return false;
//        if (el.type === "hidden") return false;
//        if (el.disabled) return false;
//        return !!(el.offsetWidth || el.offsetHeight || el.getClientRects().length);
//    }
//    function closest(el, sel) {
//        while (el && el.nodeType === 1) { if (matches(el, sel)) return el; el = el.parentNode; }
//        return null;
//    }
//    function matches(el, sel) {
//        var f = el.matches || el.msMatchesSelector || el.webkitMatchesSelector;
//        return f ? f.call(el, sel) : false;
//    }
//    function cssEsc(s) { return ("" + s).replace(/([ #;?%&,.+*~':"!^$\[\]()=>|\/@])/g, "\\$1"); }
//    function safeJson(t) { try { return JSON.parse(t); } catch (e) { return null; } }

//    function postForm(url, body, cb) {
//        var xhr = new XMLHttpRequest();
//        xhr.open("POST", url, true);
//        xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
//        xhr.onreadystatechange = function () {
//            if (xhr.readyState !== 4) return;
//            if (xhr.status >= 200 && xhr.status < 300) cb(null, xhr.responseText);
//            else cb(new Error("HTTP " + xhr.status), xhr.responseText);
//        };
//        xhr.send(body);
//    }

//    /* ---------------- BOOT ---------------- */
//    if (document.readyState === "loading")
//        document.addEventListener("DOMContentLoaded", buildUI);
//    else
//        buildUI();

//})();