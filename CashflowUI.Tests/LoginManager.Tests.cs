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
        private readonly IHttpContextAccessor Accessor;
        private readonly IAccountManager _accManager = new CashFlowUI.Helpers.AccountManager();
        private readonly ILoginManager _logManager;

        public LoginManagerTests()
        {
            var sp = MockServiceProvider();
            Accessor = MockHttpContextAccessor().Object;
            Accessor.HttpContext.RequestServices = sp.Object;
            _logManager = new LoginManager(Accessor, _accManager);

        }

        private Mock<IServiceProvider> MockServiceProvider()
        {
            var authenticationService = new Mock<IAuthenticationService>();

            var sp = new Mock<IServiceProvider>();
            sp.Setup(s => s.GetService(typeof(IAuthenticationService)))
              .Returns(() =>
              {
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
        public void CanLogin_CorrectInputs_ShouldWork()
        {
            var result = _logManager.CanLogin("testUser", "testPassword");

            result.Should().BeTrue();
        }

        [Fact]
        public void CanLogin_WrongPassword_ShouldNotWork()
        {
            var result = _logManager.CanLogin("testUser", "Wrong Password");

            result.Should().BeFalse();
        }

        [Fact]
        public void CanLogin_WrongUser_ShouldNotWork()
        {
            var result = _logManager.CanLogin("Wrong User", "testPassword");

            result.Should().BeFalse();
        }

        [Fact]
        public void CanLogin_WrongUserAndPassword_ShouldNotWork()
        {
            var result = _logManager.CanLogin("Wrong User", "Wrong Password");

            result.Should().BeFalse();
        }

        //[Fact]
        //public async Task SignInUserAsync_ShouldLogin()
        //{
        //    ILoginManager manager = new LoginManager(Accessor);

        //    await manager.SignInUserAsync("testUser");

        //    var result = Accessor.HttpContext?.Request.Cookies[LoginManager.LoginCookieString];

        //    result.Should().NotBe(null);
        //}

        //[Fact]
        //public async void SignOutUserAsync_ShouldLogout()
        //{
        //    ILoginManager manager = new LoginManager(Accessor);
        //    await manager.SignOutUserAsync();
        //    var result = Accessor.HttpContext?.Request.Cookies[LoginManager.LoginCookieString];

        //    result.Should().Be(null);
        //}

    }
}
