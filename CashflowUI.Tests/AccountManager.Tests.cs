using AccountModelsLib.Models;
using CashFlowUI.Helpers;
using CashFlowUI.HttpClients;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CashflowUI.Tests
{
    public class AccountManager
    {
        private readonly IAccountManager _accountManager;
        public AccountManager()
        {
            _accountManager = new CashFlowUI.Helpers.AccountManager(MockAccountClient().Object);
        }

        [Theory]
        [InlineData("testUser", "TestRole")]
        [InlineData("manager", "manager")]
        public async Task GetUserRoleAsync_ValidUser_ShouldFindRoleAsync(string user, string expected)
        {
            var result = await _accountManager.GetUserRoleAsync(user);

            result.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetUserRoleAsync_InvalidUser_ShouldNotFindRoleAsync()
        {
            var result = await _accountManager.GetUserRoleAsync("Invalid User");

            result.Should().BeNull();
        }

        [Fact]
        public async Task ValidateLoginInfoAsync_WrongPassword_ShouldNotWorkAsync()
        {
            var result = await _accountManager.ValidateLoginInfoAsync("testUser", "Wrong Password");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateLoginInfoAsync_WrongUser_ShouldNotWorkAsync()
        {
            var result = await _accountManager.ValidateLoginInfoAsync("Wrong User", "testPassword");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateLoginInfoAsync_WrongUserAndPassword_ShouldNotWorkAsync()
        {
            var result = await _accountManager.ValidateLoginInfoAsync("Wrong User", "Wrong Password");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetAccountByUserNameAsync_ValidUser_ShouldBeFound()
        {
            var userName = "manager";

            var result = await _accountManager.GetAccountByUserNameAsync(userName);

            var expected = new AccountModel() { Id = 1, UserCredentialName = "manager", UserCredentialPassword = "Boss123", Role = "Manager" };

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAccountByUserNameAsync_InvalidUser_ShouldNotBeFound()
        {
            var userName = "Wrong UserName";

            var result = await _accountManager.GetAccountByUserNameAsync(userName);

            result.Should().BeNull();
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
