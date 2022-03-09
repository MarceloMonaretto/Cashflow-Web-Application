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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace CashflowUI.Tests
{
    public class RolesManagerTests 
    {
        private readonly IHttpContextAccessor _mockHttpContextAccessor;
        private readonly IRolesManager _rolesManager;

        public RolesManagerTests()
        {
            var mockServiceProvider = MockServiceProvider();
            _mockHttpContextAccessor = MockHttpContextAccessor().Object;
            _mockHttpContextAccessor.HttpContext.RequestServices = mockServiceProvider.Object;
            _rolesManager = new RolesManager(_mockHttpContextAccessor, MockRoleClient().Object);
        }


        [Theory]
        [InlineData("Manager", "Sample Command 1")]
        [InlineData("Manager", "Sample Command 2")]
        [InlineData("Employee", "Sample Command 1")]
        public async Task VerifyRoleCommandPermission_ValidRoles_ShouldHavePermissionAsync(string roleName, string commandName)
        {
            var result = await _rolesManager.VerifyRoleCommandPermissionAsync(roleName, commandName);

            result.Should().Be(true);
        }

        [Theory]
        [InlineData("Manager", "Non Existent Command")]
        [InlineData("Employee", "Sample Command 2")]
        [InlineData("SimpleViewer", "Sample Command 1")]
        public async Task VerifyRoleCommandPermission_NotValidRoles_ShouldNotHavePermissionAsync(string roleName, string commandName)
        {
            var result = await _rolesManager.VerifyRoleCommandPermissionAsync(roleName, commandName);

            result.Should().Be(false);
        }


        [Theory]
        [InlineData("Manager", "Sample Menu 1")]
        [InlineData("Manager", "Sample Menu 2")]
        [InlineData("Employee", "Sample Menu 1")]
        public async Task VerifyRoleMenuPermission_ValidRoles_ShouldHavePermissionAsync(string roleName, string menuName)
        {
            var result = await _rolesManager.VerifyRoleMenuPermissionAsync(roleName, menuName);

            result.Should().Be(true);
        }

        [Theory]
        [InlineData("Manager", "Non Existent Menu")]
        [InlineData("Employee", "Sample Menu 2")]
        [InlineData("SimpleViewer", "Sample Menu 1")]
        public async Task VerifyRoleMenuPermission_NotValidRoles_ShouldNotHavePermissionAsync(string roleName, string menuName)
        {
            var result = await _rolesManager.VerifyRoleMenuPermissionAsync(roleName, menuName);

            result.Should().Be(false);
        }

















        private Mock<IRoleClient> MockRoleClient()
        {
            var sampleRoles = GenerateRoleSamples();

            var roleMock = new Mock<IRoleClient>();
            roleMock.Setup(x => x.CreateRoleAsync(It.IsAny<Role>())).Returns(Task.FromResult("Created the role!"));
            roleMock.Setup(x => x.DeleteRoleAsync(It.IsAny<string>())).Returns(Task.FromResult("Deleted the role!"));
            roleMock.Setup(x => x.UpdateRoleAsync(It.IsAny<Role>())).Returns(Task.FromResult("Updated the role!"));
            roleMock.Setup(x => x.GetRoleByNameAsync(It.IsAny<string>())).Returns((string roleName) => Task.FromResult(sampleRoles.FirstOrDefault(a => a.RoleName == roleName)));
            roleMock.Setup(x => x.GetAllRolesAsync()).Returns(Task.FromResult(sampleRoles));

            return roleMock;
        }

        private IEnumerable<Role> GenerateRoleSamples()
        {
            List<Role> samples = new()
            {
                new Role { Id = 1, RoleName = "Manager", MenuAccessPermissions = "Sample Menu 1;Sample Menu 2", CommandAccessPermissions = "Sample Command 1;Sample Command 2" },
                new Role { Id = 2, RoleName = "Employee", MenuAccessPermissions = "Sample Menu 1", CommandAccessPermissions = "Sample Command 1" },
                new Role { Id = 3, RoleName = "SimpleViewer", MenuAccessPermissions = "", CommandAccessPermissions = "" }
            };

            return samples;
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
    }
}
