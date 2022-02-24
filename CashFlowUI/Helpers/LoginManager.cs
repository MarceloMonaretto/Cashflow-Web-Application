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

        public bool CanLogin(string user, string password)
        {
            return _accManager.ValidateLoginInfo(user, password);
        }

        public async Task SignInUserAsync(string user)
        {
            var principal = CreateClaimPrincipal(user);
            await _httpContext.SignInAsync(LoginCookieString, principal);
        }

        public async Task SignOutUserAsync()
        {

            await _httpContext.SignOutAsync(LoginCookieString);
        }

        private ClaimsPrincipal CreateClaimPrincipal(string user)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user),
                        new Claim(ClaimTypes.Role, _accManager.GetUserRole(user)),
                    };

            ClaimsIdentity userIdentity = new(claims, LoginCookieString);
            return new ClaimsPrincipal(userIdentity);
        }
    }
}
