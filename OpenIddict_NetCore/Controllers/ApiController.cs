using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenIddict_NetCore
{
    public class ApiController : Controller
    {
        [Authorize]
        [HttpGet("~/api/test")]
        public IActionResult GetMessage()
        {
            return Json(new
            {
                Subject = User.GetClaim(OpenIdConnectConstants.Claims.Subject),
                Name = User.Identity.Name
            });
        }
    }
}