using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LBHFSSPortalAPI.V1.Handlers
{
    public class BasicAuthorisationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ISessionsGateway _sessionsGateway;
        private readonly IUsersGateway _usersGateway;

        public BasicAuthorisationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ISessionsGateway sessionsGateway,
            IUsersGateway usersGateway)
            : base(options, logger, encoder, clock)
        {
            _sessionsGateway = sessionsGateway;
            _usersGateway = usersGateway;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // skip authentication if endpoint has [AllowAnonymous] attribute
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();

            if (!Request.Headers.ContainsKey("AccessToken"))
                return AuthenticateResult.Fail("Missing Authorisation Header");

            User user = null;

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["AccessToken"]).ToString();
                var session = _sessionsGateway.GetSessionByToken(authHeader);
                if (session.User == null)
                    return AuthenticateResult.Fail("User not currently logged in");
                user = session.User;
            }
             catch
             {
                 return AuthenticateResult.Fail("Error identifying user session");
             }
            if (user == null)
                return AuthenticateResult.Fail("Invalid Username or Password");
            if (!user.UserRoles.Any())
                return AuthenticateResult.Fail("No roles specified for user");
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserRoles.FirstOrDefault().Role.Name),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            Console.WriteLine("Authentication Success!!!!");
            return AuthenticateResult.Success(ticket);
        }
    }
}
