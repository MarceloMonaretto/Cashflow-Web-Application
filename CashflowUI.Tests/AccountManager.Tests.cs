using CashFlowUI.Helpers;
using FluentAssertions;
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
        private readonly IAccountManager _accManager = new CashFlowUI.Helpers.AccountManager();
        public AccountManager()
        {

        }

        [Theory]
        [InlineData("testUser", "testRole")]
        [InlineData("manager", "manager")]
        public void GetUserRole_ValidUser_ShouldFindRole(string user, string expected)
        {
            var result = _accManager.GetUserRole(user);

            result.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public void GetUserRole_InvalidUser_ShouldNotFindRole()
        {
            var result = _accManager.GetUserRole("Invalid User");

            result.Should().BeNull();
        }

        [Fact]
        public void ValidateLoginInfo_WrongPassword_ShouldNotWork()
        {
            var result = _accManager.ValidateLoginInfo("testUser", "Wrong Password");

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateLoginInfo_WrongUser_ShouldNotWork()
        {
            var result = _accManager.ValidateLoginInfo("Wrong User", "testPassword");

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateLoginInfo_WrongUserAndPassword_ShouldNotWork()
        {
            var result = _accManager.ValidateLoginInfo("Wrong User", "Wrong Password");

            result.Should().BeFalse();
        }
    }
}
