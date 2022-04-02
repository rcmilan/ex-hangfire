using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Hangfire.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackgroundJobsController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly Expression<Action> _dateTimeNow = () => Console.WriteLine(DateTime.Now);
        private readonly Expression<Action> _helloWorld = () => Console.WriteLine("Hello World");
        private readonly IRecurringJobManager _recurringJobManager;
        public BackgroundJobsController(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpPost("date")]
        public ActionResult DateTimeNow()
        {
            _backgroundJobClient.Enqueue(_dateTimeNow);

            return Ok();
        }

        [HttpPost("hello")]
        public ActionResult HelloWorld()
        {
            _backgroundJobClient.Enqueue(_helloWorld);

            return Ok();
        }

        [HttpPost("recurring")]
        public ActionResult Recurring()
        {
            _recurringJobManager.AddOrUpdate("recurringDateTimeNow", _dateTimeNow, "0/5 * * * * *");

            return Ok();
        }

        [HttpPost("schedule")]
        public ActionResult Schedule()
        {
            _backgroundJobClient.Schedule(_dateTimeNow, DateTimeOffset.FromUnixTimeSeconds(5));

            return Ok();
        }
    }
}