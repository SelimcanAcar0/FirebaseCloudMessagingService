using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using FirebaseCloudMessagingService.model;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;

namespace FirebaseCloudMessagingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationServiceController : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> SendNotificationAsync([FromBody] NotificationModel request)
        {
            try
            {
                if (FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile("config\\fcm.json")
                    });
                }

                var message = new Message
                {
                    Notification = new Notification
                    {
                        Title = request.title,
                        Body = request.body

                    },
                    Token = "device_token"
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                return Ok(response);
            }
            catch (FirebaseMessagingException ex)
            {
                return BadRequest($"Error sending notification: {ex.Message}");
            }
        }
    }
}
