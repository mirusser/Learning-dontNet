using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfireController : ControllerBase
    {
        #region Fire-and-forget jobs
        [HttpPost]
        [Route("Welcome")]
        public IActionResult Welcome(string userName)
        {
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeMail(userName));

            return Ok($"Job Id: {jobId}, completed. Welcome mail sent!");
        }

        public void SendWelcomeMail(string userName)
        {
            //TODO: add logic to mail to user
            Console.WriteLine($"Welcome to our application, {userName}");
        }
        #endregion

        #region Delayed jobs
        [HttpPost]
        [Route("delayedWelcome")]
        public IActionResult DelayedWelcome(string userName)
        {
            var jobId = BackgroundJob.Schedule(() => SendDelayedWelcomeMail(userName), TimeSpan.FromMinutes(1));
            return Ok($"Job id: {jobId} completed. Delayed welcome mail sent!");
        }

        public void SendDelayedWelcomeMail(string userName)
        {
            Console.WriteLine($"Welcome to our application, {userName}");
        }
        #endregion

        #region Recurring jobs
        [HttpPost]
        [Route("invoice")]
        public IActionResult Invoice(string userName)
        {
            RecurringJob.AddOrUpdate(() => SendInvoiceMail(userName), Cron.Monthly);
            return Ok($"Recurrin job scheduled. Invoice mail will be mailed monthly for {userName}");
        }

        public void SendInvoiceMail(string userName)
        {
            Console.WriteLine($"Here is your invoice, {userName}");
        }
        #endregion

        #region Continuations
        [HttpPost]
        [Route("unsubscribe")]
        public IActionResult Unsubscribe(string userName)
        {
            var jobId = BackgroundJob.Enqueue(() => UnsubscribeUser(userName));
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine($"Sent Confirmation Mail to {userName}"));
            return Ok($"Unsubscribed");
        }

        public void UnsubscribeUser(string userName)
        {
            //Logic to Unsubscribe the user
            Console.WriteLine($"Unsubscribed {userName}");
        }
        #endregion
    }
}
