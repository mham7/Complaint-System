using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;

[ApiController]
[Route("api/whatsapp")]
public class WhatsappController : ControllerBase
{
    private static readonly ConcurrentDictionary<string, UserSession> Sessions = new();
    private static readonly string[] Questions =
    {
        "What's your name?",
        "How old are you?",
        "Where are you from?",
        "What’s your profession?",
        "What’s your favorite hobby?"
    };

    [HttpPost]
    public async Task<IActionResult> ReceiveMessage()
    {
        var response = new MessagingResponse();
        var requestBody = new Dictionary<string, string>();

        foreach (var key in Request.Form.Keys)
        {
            requestBody[key] = Request.Form[key];
        }

        // Log request details (replace with your logging method)

        // Check if media is present
        if (Request.Form.ContainsKey("NumMedia") && int.TryParse(Request.Form["NumMedia"], out int numMedia) && numMedia > 0)
        {
            for (int i = 0; i < numMedia; i++)
            {
                string key = $"MediaUrl{i}";
                if (Request.Form.ContainsKey(key))
                {
                    Console.WriteLine($"Received Media: {Request.Form[key]}");
                }
            }
        }

        response.Message("Received your message!");

        return Content(response.ToString(), "text/xml");
    }


    //[HttpPost]
    //public IActionResult ReceiveMessage([FromForm] string From, [FromForm] string Body)
    //{
    //    var response = new MessagingResponse();
    //    string body = Body.Trim();

    //    if (!Sessions.ContainsKey(From))
    //    {
    //        Sessions[From] = new UserSession();
    //        response.Message(Questions[0]);
    //    }
    //    else
    //    {
    //        var session = Sessions[From];

    //        if (session.Step < Questions.Length)
    //        {
    //            session.Answers.Add($"{Questions[session.Step]}: {body}");
    //            session.Step++;

    //            if (session.Step < Questions.Length)
    //            {
    //                response.Message(Questions[session.Step]);
    //            }
    //            else
    //            {
    //                response.Message($"Thanks! Here are your answers:\n\n{string.Join("\n", session.Answers)}");
    //                Sessions.TryRemove(From, out _);
    //            }
    //        }
    //        else
    //        {
    //            response.Message("You have completed the survey. Send 'Hi' to start again.");
    //            Sessions.TryRemove(From, out _);
    //        }
    //    }

    //    return Content(response.ToString(), "text/xml");
    //}
}

public class UserSession
{
    public int Step { get; set; } = 0;
    public List<string> Answers { get; set; } = new();
}
