using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CashFlowUI.Helpers
{
    public class LoginManager : ILoginManager
    {
        public const string LoginCookieString = "MyLoginCookie";

        private readonly HttpContext _httpContext;
        private readonly IAccountManager _accManager;

        public LoginManager(IHttpContextAccessor httpContextAccessor, IAccountManager accManager)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _accManager = accManager;
        }

        public async Task<bool> CanLoginAsync(string user, string password)
        {
            return await _accManager.ValidateLoginInfoAsync(user, password);
        }

        public async Task SignInUserAsync(string user)
        {
            var principal = await CreateClaimPrincipalAsync(user);
            await _httpContext.SignInAsync(LoginCookieString, principal);
        }

        public async Task SignOutUserAsync()
        {

            await _httpContext.SignOutAsync(LoginCookieString);
        }

        private async Task<ClaimsPrincipal> CreateClaimPrincipalAsync(string user)
        {

            var userRole = await _accManager.GetUserRoleAsync(user);
            if (userRole == null)
            {
                userRole = "Viewer";
            }

            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user),
                        new Claim(ClaimTypes.Role, userRole)
                    };

            ClaimsIdentity userIdentity = new(claims, LoginCookieString);
            return new ClaimsPrincipal(userIdentity);
        }
    }
}
