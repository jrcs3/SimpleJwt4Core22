using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleJwt4Core22.ViewModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleJwt4Core22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public JwtController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("maketoken")]
        [HttpPost]
        public ActionResult Login([FromBody] MakeTokenViewModel model)
        {
            // Simulate login or other failure:
            if (model.Fail)
            {
                return BadRequest("JWT Creation Failure");
            }

            // Get configuration data
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:SigningKey"]));
            int experyInMinutes = Convert.ToInt32(_configuration["Jwt:ExperyInMinutes"]);
            string site = _configuration["Jwt:Site"];

            JwtSecurityToken token = makeToken(model, signingKey, experyInMinutes, site);
            // send it out as 200
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        private static JwtSecurityToken makeToken(MakeTokenViewModel model, SymmetricSecurityKey signingKey, int experyInMinutes, string site)
        {
            // Create claims for any data I want to embed into the JWT
            var claim = new[]
            {
                new Claim("name", model.UserName),
                new Claim("id", model.Id.ToString()),
                new Claim("role", model.Role)
            };

            // Create the sighed token
            var token = new JwtSecurityToken(
                issuer: site,
                audience: site,
                expires: DateTime.UtcNow.AddMinutes(experyInMinutes),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                claims: claim
            );
            return token;
        }
    }
}