using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CashFlowUI.Helpers
{
    public class LoginManager : ILoginManager
    {
        public const string LoginCookieString = "MyLoginCookie";

        private readonly List<string[]> Users = new()
        {
            new string[]{ "manager", "Boss123", "manager" },
            new string[]{ "employee", "Employee123", "staff" },
            new string[]{ "testUser", "testPassword", "testRole" }
        };

        private readonly HttpContext _httpContext;

        public LoginManager(IHttpContextAccessor httpContextAccessor) =>
            _httpContext = httpContextAccessor.HttpContext;

        public async Task<bool> SignInUserAsync(string user)
        {
            var principal = CreateClaimPrincipal(user);
            await _httpContext.SignInAsync("MyLoginCookie", principal);

            return true;
        }

        public async Task<bool> SignOutUserAsync()
        {

            await _httpContext.SignOutAsync(LoginCookieString);

            return true;
        }

        private ClaimsPrincipal CreateClaimPrincipal(string user)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user)
                    };

            ClaimsIdentity userIdentity = new(claims, LoginCookieString);
            return new ClaimsPrincipal(userIdentity);
        }

        public bool ValidateLoginInfo(string user, string password)
        {
            try
            {
                return Users.Single(u => u[0] == user && u[1] == password).Any();
            }
            catch
            {
                return false;
            }            
        }

        public string GetUserRole(string user)
        {
            return Users.First(u => u[0] == user)[2];
        }
    }
}
