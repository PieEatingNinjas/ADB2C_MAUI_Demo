using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using MyB2CAppApi.Services;
using System.Security.Claims;

namespace MyB2CAppApi.Controllers
{
    [ApiController]
    [Authorize]
    [RequiredScope("api.read")]
    [Route("[controller]")]
    public class UserSubscriptionsController : ControllerBase
    {
        private readonly ILogger<UserSubscriptionsController> _logger;

        public UserSubscriptionsController(ILogger<UserSubscriptionsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetUserSubscriptions")]
        public string[] Get()
        {
            var service = new UserSubscriptionService(); //should be injected
            return service.GetUserSubscriptions(HttpContext.User.FindFirstValue(ClaimConstants.ObjectId)!);
        }
    }
}