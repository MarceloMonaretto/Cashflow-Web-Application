using CashFlowUI.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CashflowUI.Tests
{
    public class LoginManagerTests
    {
        public readonly IHttpContextAccessor Accessor;

        public LoginManagerTests()
        {
            var sp = MockServiceProvider();
            Accessor = MockHttpContextAccessor().Object;
            Accessor.HttpContext.RequestServices = sp.Object;

        }

        private Mock<IServiceProvider> MockServiceProvider()
        {
            var authenticationService = new Mock<IAuthenticationService>();

            var sp = new Mock<IServiceProvider>();
            sp.Setup(s => s.GetService(typeof(IAuthenticationService)))
              .Returns(() => {
                  return authenticationService.Object;
              });

            return sp;
        }

        private Mock<IHttpContextAccessor> MockHttpContextAccessor()
        {
            Mock<IHttpContextAccessor> mockAccessor = new();
            mockAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
            return mockAccessor;           
        }

        [Fact]
        public void ValidateLoginInfo_CorrectInputs_ShouldWork()
        {
            ILoginManager manager = new LoginManager(Accessor);

            var result = manager.ValidateLoginInfo("testUser", "testPassword");

            result.Should().BeTrue();
        }

        [Fact]
        public void ValidateLoginInfo_WrongPassword_ShouldNotWork()
        {
            ILoginManager manager = new LoginManager(Accessor);

            var result = manager.ValidateLoginInfo("testUser", "Wrong Password");

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateLoginInfo_WrongUser_ShouldNotWork()
        {
            ILoginManager manager = new LoginManager(Accessor);

            var result = manager.ValidateLoginInfo("Wrong User", "testPassword");

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateLoginInfo_WrongUserAndPassword_ShouldNotWork()
        {
            ILoginManager manager = new LoginManager(Accessor);

            var result = manager.ValidateLoginInfo("Wrong User", "Wrong Password");

            result.Should().BeFalse();
        }

    }
}
