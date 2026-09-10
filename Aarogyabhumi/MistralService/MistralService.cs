using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Shopinv.MistralService
{
    /// <summary>
    /// Existing Mistral proxy service, now completed.
    /// Responsibility: take a free-form spoken transcript (Hindi / Hinglish / English)
    /// plus lightweight page context, and ask Mistral to return a STRICT JSON intent
    /// object that the client-side voice executor understands.
    ///
    /// Nothing about the app's existing business logic, controllers, routes or DB is
    /// touched here. This service only does Natural Language -> Intent JSON translation.
    /// </summary>
    public class MistralService
    {
        private static readonly HttpClient _shared = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(20)
        };

        private readonly HttpClient _http;

        // Existing constructor (kept so any existing wiring still compiles).
        public MistralService(HttpClient http)
        {
            _http = http ?? _shared;
        }

        // Convenience constructor used by the controller.
        public MistralService() : this(_shared) { }

        // System prompt: defines the strict JSON schema the model must emit.
        // SHORT mode is used for runtime command parsing (fast, deterministic).
        private const string SystemPrompt =
@"You are the intent parser for the voice assistant of an Indian e-commerce + MLM web portal.
The user speaks in Hindi, Hinglish or English and may make spelling / pronunciation mistakes.
Read the transcript and the page context, then reply with ONE JSON object and NOTHING ELSE.
No markdown, no backticks, no explanation.

IMPORTANT — language: detect the language of the TRANSCRIPT and write the ""speak"" field in the
SAME language. Hindi input -> Hindi speak. English input -> English speak. Hinglish -> Hinglish.

JSON schema (use only these keys):
{
  ""action"": one of [""navigate"",""click"",""fill"",""filter"",""cart_add"",""cart_qty"",""cart_remove"",""cart_view"",""coupon"",""checkout"",""place_order"",""speak"",""unknown""],
  ""target"": string,        // page/menu name, control label, button text, or product name (lowercase, spelling normalized to English)
  ""value"": string,         // value to type, quantity, coupon code, address/payment choice, etc.
  ""qty"": number,           // numeric quantity if the user said one, else 0
  ""position"": string,      // optional: ""first"" | ""last"" | """" — for ""remove first product"" style commands
  ""page"": string,          // optional: for filter/navigate, the report/page name (e.g. ""my orders"", ""ticket list"")
  ""entities"": {            // optional extracted filters for grids/reports
     ""memberId"":"""", ""name"":"""", ""party"":"""", ""product"":"""", ""mobile"":"""",
     ""status"":"""", ""dateFrom"":"""", ""dateTo"":"""", ""voucher"":"""", ""invoice"":"""", ""amount"":""""
  },
  ""speak"": string          // a short bilingual confirmation to read back to the user (Hinglish)
}

Mapping hints (do not invent routes; just normalize the target name):
- ""cart kholo / view cart / show my cart"" -> action ""cart_view""
- ""X ko cart me daalo / add N X to cart / X add karo"" -> action ""cart_add"", target=product, qty=N
- ""quantity badhao / increase quantity to N"" -> action ""cart_qty"", value=""plus"" or qty=N
- ""quantity ghatao / decrease"" -> action ""cart_qty"", value=""minus""
- ""pehla product hatao / remove first product / X remove karo"" -> action ""cart_remove"", position or target
- ""coupon X lagao / apply coupon X"" -> action ""coupon"", value=code
- ""checkout / proceed to checkout / aage badho"" -> action ""checkout""
- ""order place karo / place order / proceed to pay / pay karo"" -> action ""place_order""
- LOGIN: ""login / log in / sign in / submit / continue"" -> action ""navigate"", target=""login""
  (the client knows whether to open the login page or click the login button based on context)
- A page that needs login (orders, cart, wallet, profile, checkout, commission, kyc) -> action
  ""navigate"" with target=page; the client handles the login-first redirect automatically.
- GRID / REPORT FILTERING (important): to find/filter rows in any report (orders, tickets/
  complaints, wallet, coupons, level/commission report, members), return action ""filter""
  with value=the thing to find and page=the report name. Examples:
    ""order number 835968 dikhao / show order 835968"" -> filter, value=""835968"", page=""my orders""
    ""30 april wale orders"" -> filter, value=""30 apr"", page=""my orders""
    ""complaint 10010 dikhao / ticket 10010"" -> filter, value=""10010"", page=""ticket list""
    ""show outstanding of ABC Traders"" -> filter, value=""ABC Traders"", entities.party=""ABC Traders""
    ""filter active members / show pending wallet requests"" -> filter, value=""active""/""pending"", entities.status set
    ""clear filter / filter hatao / sab dikhao"" -> filter, value=""clear""
- ""ledger report kholo / open my direct report / open commission report"" -> action ""navigate"" (target=page)
- naam/field bharna (e.g. ""mobile number 98765 likho"") -> action ""fill"", target=field, value=...
- A button by its visible text -> action ""click"", target=button text
Always fill the speak field. If unsure, action=""unknown"" with a helpful speak.";

        /// <summary>
        /// Returns the model's raw assistant content, which (per the system prompt) is a
        /// strict JSON intent object. The controller validates and forwards it to the client.
        /// </summary>
        public async Task<string> ExtractCommand(string userText, string pageContext = "")
        {
            // Legacy .NET (4.5.2) does not negotiate TLS 1.2 by default — Mistral requires it.
            try { ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; } catch { }

            var apiKey = ConfigurationManager.AppSettings["MistralApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                return BuildSpeak("Voice service configured nahi hai. Web.config me MistralApiKey set karein.");

            var serializer = new JavaScriptSerializer();

            var userContent =
                "PAGE CONTEXT: " + (pageContext ?? "") + "\n" +
                "TRANSCRIPT: " + (userText ?? "");

            var payload = new
            {
                model = "mistral-small-latest",
                temperature = 0.1,
                max_tokens = 400,
                messages = new object[]
                {
                    new { role = "system", content = SystemPrompt },
                    new { role = "user", content = userContent }
                }
            };

            var json = serializer.Serialize(payload);

            using (var req = new HttpRequestMessage(HttpMethod.Post, "https://api.mistral.ai/v1/chat/completions"))
            {
                req.Headers.TryAddWithoutValidation("Authorization", "Bearer " + apiKey);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                try
                {
                    response = await _http.SendAsync(req).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return BuildSpeak("Network error: " + ex.Message);
                }

                var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    // Surface a readable message to the client instead of a raw 401/500 blob.
                    return BuildSpeak("Mistral API error (" + (int)response.StatusCode + "). API key / quota check karein.");
                }

                // Pull choices[0].message.content out of the chat completion envelope.
                try
                {
                    var parsed = serializer.Deserialize<Dictionary<string, object>>(body);
                    var choices = parsed != null && parsed.ContainsKey("choices") ? parsed["choices"] as System.Collections.ArrayList : null;
                    if (choices != null && choices.Count > 0)
                    {
                        var first = choices[0] as Dictionary<string, object>;
                        var message = first != null && first.ContainsKey("message") ? first["message"] as Dictionary<string, object> : null;
                        var content = message != null && message.ContainsKey("content") ? message["content"] as string : null;
                        if (!string.IsNullOrWhiteSpace(content))
                            return CleanJsonFence(content);
                    }
                }
                catch { /* fall through */ }

                return BuildSpeak("Command samajh nahi aaya. Dobara boliye.");
            }
        }

        // Mistral occasionally wraps JSON in ```json fences despite instructions — strip them.
        private static string CleanJsonFence(string s)
        {
            s = s.Trim();
            if (s.StartsWith("```"))
            {
                int firstBrace = s.IndexOf('{');
                int lastBrace = s.LastIndexOf('}');
                if (firstBrace >= 0 && lastBrace > firstBrace)
                    s = s.Substring(firstBrace, lastBrace - firstBrace + 1);
            }
            return s;
        }

        private static string BuildSpeak(string message)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(new
            {
                action = "speak",
                target = "",
                value = "",
                qty = 0,
                position = "",
                entities = new { },
                speak = message
            });
        }
    }
}
