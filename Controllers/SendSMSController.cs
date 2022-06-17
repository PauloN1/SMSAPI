using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Model;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace API.Controllers
{
    [Route("[controller]")]
    public class SendSMSController : Controller
    {
        private readonly IConfiguration _configuration; 

        
        public SendSMSController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("send")]

        public ActionResult<ReturnCode> Send(string Number, string Message)
        {
            
             string accountSID  = _configuration["Twilio:AccountSID"];
             string authToken = _configuration["Twilio:AuthToken"];
             string AuthNumber = _configuration["Twilio:AuthNumber"];
             string msg = "";
             int statusCode= 0;
             TwilioClient.Init(accountSID, authToken);
             
            try
            {
                MessageResource response = MessageResource.Create(
                    body: Message,
                    from: new Twilio.Types.PhoneNumber(AuthNumber),
                    to: new Twilio.Types.PhoneNumber("+27" + Number)
                );
                msg = "Message sent successful to: +27" + Number;
                statusCode = 200;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                statusCode = 500;
            }

            return new ReturnCode{
                StatusCode = statusCode,
                Message = msg
            };

        }
    }
}