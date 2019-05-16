using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace SimpleJwt4Core22.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly JsonSerializerSettings _serializerSettings;
        public ValuesController()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return handleRequest();
        }

        // All of these endpoints do the same thing except for the 
        // authorized roles
        [Route("admin")]
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public IActionResult GetAdmin()
        {
            return handleRequest();
        }

        [Route("super")]
        [Authorize(Policy = "SuperPolicy")]
        [HttpGet]
        public IActionResult GetSuper()
        {
            return handleRequest();
        }

        [Route("either")]
        [Authorize(Policy = "EitherPolicy")]
        [HttpGet]
        public IActionResult GetBoth()
        {
            return handleRequest();
        }

        [Route("open")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetOpen()
        {
            return handleRequest();
        }

        private IActionResult handleRequest()
        {
            // Read the claims that I wrote in JwtController:
            var claims = ((ClaimsIdentity)User.Identity).Claims;
            var id = getClaimByType(claims, "id");
            var name = getClaimByType(claims, "name");
            // In JwtController, I created a claim for "role" so I would expect this to have a value:
            var role = getClaimByType(claims, "role");
            // however, by magic, this one has the value:
            var msRole = getClaimByType(claims, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            var response = new
            {
                id,
                name,
                role,
                msRole
            };
            // Send the claims back in the Response:
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        public static string getClaimByType(IEnumerable<Claim> jwt, string typeKey)
        {
            foreach (var claim in jwt)
            {
                if (claim.Type == typeKey)
                {
                    return claim.Value;
                }
            }
            return string.Empty;
        }
    }
}
