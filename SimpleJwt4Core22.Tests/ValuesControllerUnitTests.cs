using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using SimpleJwt4Core22.Controllers;
using SimpleJwt4Core22.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace SimpleJwt4Core22.Tests
{
    public class ValuesControllerUnitTests
    {
        Mock<IConfiguration> _mockConfiguration;
        public ValuesControllerUnitTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(x => x[It.Is<string>(s => s == "Jwt:SigningKey")]).Returns("To be or not to be, that's the question!");
            _mockConfiguration.Setup(x => x[It.Is<string>(s => s == "Jwt:ExperyInMinutes")]).Returns("1");
            _mockConfiguration.Setup(x => x[It.Is<string>(s => s == "Jwt:Site")]).Returns("http://example.com");
        }

        [Fact]
        public void GetAdminAsAdminSuccess()
        {
            var model = new MakeTokenViewModel()
            {
                Id = 1,
                UserName = "darthv@deathstar.mil",
                Role = "admin"
            };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _mockConfiguration.Object["Jwt:SigningKey"]));
            int experyInMinutes = Convert.ToInt32(_mockConfiguration.Object["Jwt:ExperyInMinutes"]);
            string site = _mockConfiguration.Object["Jwt:Site"];

            JwtSecurityToken token = makeToken(model, signingKey, experyInMinutes, site);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            var controller = new ValuesController();
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {encodedJwt}";

            var result = controller.GetAdmin();

            ((ObjectResult)result).StatusCode.Should().Be(200);

            System.Diagnostics.Debug.WriteLine(result.ToString());
        }

        [Fact]
        public void GetAdminAsSuperFail()
        {
            var model = new MakeTokenViewModel()
            {
                Id = 1,
                UserName = "darthv@deathstar.mil",
                Role = "super"
            };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _mockConfiguration.Object["Jwt:SigningKey"]));
            int experyInMinutes = Convert.ToInt32(_mockConfiguration.Object["Jwt:ExperyInMinutes"]);
            string site = _mockConfiguration.Object["Jwt:Site"];

            JwtSecurityToken token = makeToken(model, signingKey, experyInMinutes, site);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            var user = new ClaimsPrincipal(new ClaimsIdentity(makeClaimList(model)));


            var controller = new ValuesController();
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = user
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {encodedJwt}";

            var result = controller.GetAdmin();

            ((ObjectResult)result).StatusCode.Should().Be(200);

            System.Diagnostics.Debug.WriteLine(result.ToString());
        }


        private static JwtSecurityToken makeToken(MakeTokenViewModel model, SymmetricSecurityKey signingKey, int experyInMinutes, string site)
        {
            // Create claims for any data I want to embed into the JWT
            IEnumerable<Claim> claim = makeClaimList(model);

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

        private static IEnumerable<Claim> makeClaimList(MakeTokenViewModel model)
        {
            return new[]
            {
                new Claim("name", model.UserName),
                new Claim("id", model.Id.ToString()),
                new Claim("role", model.Role)
            };
        }
    }
}
