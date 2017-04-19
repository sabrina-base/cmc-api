using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WashMyCar.API.Data;
using WashMyCar.API.Models;

namespace WashMyCar.Api.Providers
{
    public class LocalAuthorizationProvider : OAuthAuthorizationServerProvider
    {
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var db = new WashMyCarDataContext();
            var store = new UserStore<User>(db);

            using (var manager = new UserManager<User>(store))
            {
                var user = manager.Find(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "Incorrect username or password");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                foreach(var role in user.Roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, db.Roles.Find(role.RoleId).Name));
                }

                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "username", user.UserName
                    },
                    {
                        "emailAddress", user.Email
                    },
                    {
                        "roles", string.Join(",", user.Roles.ToArray().Select(r => db.Roles.Find(r.RoleId).Name))
                    }
                });

                var ticket = new AuthenticationTicket(identity, props);

                context.Validated(ticket);
            }

            
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}
