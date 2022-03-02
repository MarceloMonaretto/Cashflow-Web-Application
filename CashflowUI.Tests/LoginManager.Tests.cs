using AccountModelsLib.Models;
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
        private readonly IAccountManager _accManager;
        private readonly ILoginManager _logManager;

        public LoginManagerTests()
        {
            var mockServiceProvider = MockServiceProvider();
            _accManager = new CashFlowUI.Helpers.AccountManager(MockAccountClient().Object);
            _mockHttpContextAccessor = MockHttpContextAccessor().Object;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _mockHttpContextAccessor.HttpContext.RequestServices = mockServiceProvider.Object;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            _logManager = new LoginManager(_mockHttpContextAccessor, _accManager);
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
            var result = await _logManager.CanLoginAsync("testUser", "testPassword");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task CanLoginAsync_WrongPassword_ShouldNotWorkAsync()
        {
            var result = await _logManager.CanLoginAsync("testUser", "Wrong Password");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task CanLoginAsync_WrongUser_ShouldNotWorkAsync()
        {
            var result = await _logManager.CanLoginAsync("Wrong User", "testPassword");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task CanLoginAsync_WrongUserAndPassword_ShouldNotWorkAsync()
        {
            var result = await _logManager.CanLoginAsync("Wrong User", "Wrong Password");

            result.Should().BeFalse();
        }

        private Mock<IAccountClient> MockAccountClient()
        {
            var sampleAccounts = GenerateAccountSamples();

            var accountMock = new Mock<IAccountClient>();
            accountMock.Setup(x => x.CreateAccountAsync(It.IsAny<AccountModel>())).Returns(Task.FromResult("Created the account!"));
            accountMock.Setup(x => x.DeleteAccountAsync(It.IsAny<int>())).Returns(Task.FromResult("Deleted the account!"));
            accountMock.Setup(x => x.UpdateAccountAsync(It.IsAny<AccountModel>())).Returns(Task.FromResult("Updated the account!"));
            accountMock.Setup(x => x.GetAccountByIdAsync(It.IsAny<int>())).Returns((int id) => Task.FromResult(sampleAccounts.FirstOrDefault(a => a.Id == id)));
            accountMock.Setup(x => x.GetAllAccountsAsync()).Returns(Task.FromResult(sampleAccounts));

            return accountMock;
        }


        private IEnumerable<AccountModel> GenerateAccountSamples()
        {
            List<AccountModel> samples = new()
            {
                new AccountModel { Id = 1, UserCredentialName = "manager", UserCredentialPassword = "Boss123", Role = "Manager"},
                new AccountModel { Id = 2, UserCredentialName = "employee", UserCredentialPassword = "Employee123", Role = "Employee"},
                new AccountModel { Id = 3, UserCredentialName = "testUser", UserCredentialPassword = "testPassword", Role = "TestRole" }
            };

            return samples;
        }

    }
}
