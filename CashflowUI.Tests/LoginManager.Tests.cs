using ModelsLib.ContextRepositoryClasses;
using CashFlowUI.Helpers;
using CashFlowUI.HttpClients;
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
        private readonly IHttpContextAccessor _mockHttpContextAccessor;
        private readonly IAccountManager _accountManager;
        private readonly ILoginManager _loginManager;

        public LoginManagerTests()
        {
            var mockServiceProvider = MockServiceProvider();
            _accountManager = new CashFlowUI.Helpers.AccountManager(MockAccountClient().Object);
            _mockHttpContextAccessor = MockHttpContextAccessor().Object;
            _mockHttpContextAccessor.HttpContext.RequestServices = mockServiceProvider.Object;
            _loginManager = new LoginManager(_mockHttpContextAccessor, _accountManager);
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
        public async Task CanLoginAsync_CorrectInputs_ShouldWorkAsync()
        {
            var result = await _loginManager.CanLoginAsync("testUser", "testPassword");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task CanLoginAsync_WrongPassword_ShouldNotWorkAsync()
        {
            var result = await _loginManager.CanLoginAsync("testUser", "Wrong Password");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task CanLoginAsync_WrongUser_ShouldNotWorkAsync()
        {
            var result = await _loginManager.CanLoginAsync("Wrong User", "testPassword");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task CanLoginAsync_WrongUserAndPassword_ShouldNotWorkAsync()
        {
            var result = await _loginManager.CanLoginAsync("Wrong User", "Wrong Password");

            result.Should().BeFalse();
        }

        private Mock<IAccountClient> MockAccountClient()
        {
            var sampleAccounts = GenerateAccountSamples();

            var accountMock = new Mock<IAccountClient>();
            accountMock.Setup(x => x.CreateAccountAsync(It.IsAny<Account>())).Returns(Task.FromResult("Created the account!"));
            accountMock.Setup(x => x.DeleteAccountAsync(It.IsAny<int>())).Returns(Task.FromResult("Deleted the account!"));
            accountMock.Setup(x => x.UpdateAccountAsync(It.IsAny<Account>())).Returns(Task.FromResult("Updated the account!"));
            accountMock.Setup(x => x.GetAccountByIdAsync(It.IsAny<int>())).Returns((int id) => Task.FromResult(sampleAccounts.FirstOrDefault(a => a.Id == id)));
            accountMock.Setup(x => x.GetAllAccountsAsync()).Returns(Task.FromResult(sampleAccounts));

            return accountMock;
        }


        private IEnumerable<Account> GenerateAccountSamples()
        {
            List<Account> samples = new()
            {
                new Account { Id = 1, UserName = "manager", UserPassword = "Boss123", UserRole = "Manager"},
                new Account { Id = 2, UserName = "employee", UserPassword = "Employee123", UserRole = "Employee"},
                new Account { Id = 3, UserName = "testUser", UserPassword = "testPassword", UserRole = "TestRole" }
            };

            return samples;
        }

    }
}
