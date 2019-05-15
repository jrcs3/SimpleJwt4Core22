using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SimpleJwt4Core22.Controllers;
using SimpleJwt4Core22.ViewModels;
using System;
using Xunit;

namespace SimpleJwt4Core22.Tests
{
    public class JwtControllerUnitTests
    {
        Mock<IConfiguration> _mockConfiguration;
        public JwtControllerUnitTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(x => x[It.Is<string>(s => s == Settings.JWT_SIGNING_KEY_KEY)]).Returns(Settings.JWT_SIGNING_KEY_VALUE);
            _mockConfiguration.Setup(x => x[It.Is<string>(s => s == Settings.JWT_EXPEPRY_KEY)]).Returns(Settings.JWT_EXPEPRY_VALUE);
            _mockConfiguration.Setup(x => x[It.Is<string>(s => s == Settings.JWT_SITE_KEY)]).Returns(Settings.JWT_SITE_VALUE);
        }
        [Fact]
        public void LoginSuccess()
        {
            // Arrange
            MakeTokenViewModel model = new MakeTokenViewModel()
            {
                Id = 7,
                UserName = "dvader",
                Role = "admin"
            };
            var controller = new JwtController(_mockConfiguration.Object);

            // Act
            var result = controller.Login(model);

            // Assert
            var value = ((ObjectResult)result).Value;

            ((ObjectResult)result).StatusCode.Should().Be(200);

            Console.WriteLine(result);
        }
        [Fact]
        public void LoginFail()
        {
            // Arrange
            MakeTokenViewModel model = new MakeTokenViewModel()
            {
                Id = 7,
                UserName = "dvader",
                Role = "admin",
                Fail = true
            };
            var controller = new JwtController(_mockConfiguration.Object);

            // Act
            var result = controller.Login(model);

            // Assert
            ((ObjectResult)result).StatusCode.Should().Be(400);

            Console.WriteLine(result);
        }

    }
}
