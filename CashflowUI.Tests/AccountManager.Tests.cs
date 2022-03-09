using ModelsLib.ContextRepositoryClasses;
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

            var expected = new Account() { Id = 1, UserName = "manager", UserPassword = "Boss123", UserRole = "Manager" };

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
