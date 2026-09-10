using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Shopinv.MistralService;

namespace Shopinv.Controllers
{
    /// <summary>
    /// Existing voice controller, extended.
    /// Index() is unchanged. Command() is the single server endpoint the global
    /// voice-assistant.js calls. It does NOT touch any business logic — it only
    /// converts a spoken transcript into a validated intent JSON and returns it.
    /// </summary>
    public class AIAssistantController : Controller
    {
        // GET: AIAssistant  (unchanged)
        public ActionResult Index()
        {
            return View();
        }

        // POST: /AIAssistant/Command
        // body: { transcript: "...", context: "...optional page context..." }
        [HttpPost]
        public async Task<ActionResult> Command(string transcript, string context)
        {
            if (string.IsNullOrWhiteSpace(transcript))
                return JsonIntent(Speak("Kuch sunai nahi diya. Dobara boliye."));

            string raw;
            try
            {
                var service = new MistralService.MistralService();
                raw = await service.ExtractCommand(transcript, context);
            }
            catch (Exception ex)
            {
                return JsonIntent(Speak("Server error: " + ex.Message));
            }

            // Validate that we actually got an intent object. If parsing fails, degrade
            // gracefully to a spoken message rather than throwing to the client.
            var serializer = new JavaScriptSerializer();
            try
            {
                var obj = serializer.Deserialize<Dictionary<string, object>>(raw);
                if (obj == null || !obj.ContainsKey("action"))
                    return JsonIntent(Speak("Command samajh nahi aaya. Dobara koshish karein."));

                // Echo the original transcript back so the UI can display it.
                obj["transcript"] = transcript;
                return new ContentResult
                {
                    Content = serializer.Serialize(obj),
                    ContentType = "application/json"
                };
            }
            catch
            {
                return JsonIntent(Speak("Command parse nahi ho saka. Dobara boliye."));
            }
        }

        private ContentResult JsonIntent(object intent)
        {
            return new ContentResult
            {
                Content = new JavaScriptSerializer().Serialize(intent),
                ContentType = "application/json"
            };
        }

        private static object Speak(string message)
        {
            return new
            {
                action = "speak",
                target = "",
                value = "",
                qty = 0,
                position = "",
                entities = new { },
                speak = message
            };
        }
    }
}
