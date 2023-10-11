using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Sales.Web.Auth
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(3000);
            var anonimous = new ClaimsIdentity();
            var josemfeUser = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("FirstName", "Jose"),
                    new Claim("LastName", "Fernandez"),
                    new Claim(ClaimTypes.Name, "josemfe@yopmail.com"),
                    new Claim(ClaimTypes.Role, "Admin")

                },
            authenticationType: "test");

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(josemfeUser)));
        }
    }

}
