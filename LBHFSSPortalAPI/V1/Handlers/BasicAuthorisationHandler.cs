using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
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
            // skip authentication if request method is OPTIONS
            if (Context.Request.Method == "OPTIONS")
            {
                return AuthenticateResult.NoResult();
            }
            // skip authentication if endpoint has [AllowAnonymous] attribute
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();

            var accessToken = Request.Cookies[Cookies.AccessTokenName];

            if (accessToken == null)
                return AuthenticateResult.Fail("Missing 'access_token' cookie");

            var session = _sessionsGateway.GetSessionByToken(accessToken);

            if (session?.User == null)
                return AuthenticateResult.Fail("Invalid session key");

            if (session.User.UserRoles == null || !session.User.UserRoles.Any())
                return AuthenticateResult.Fail("No roles have been assigned to the user");

            var userRoleNames = session.User.UserRoles
                .Where(ur => ur.Role != null)
                .Select(ur => ur.Role.Name)
                .ToList();

            var claims = userRoleNames.SelectMany(r => new[] {
                new Claim(ClaimTypes.NameIdentifier, $"{session.User.Id}"),
                new Claim(ClaimTypes.Role, r)
            })
            .ToArray();

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            LoggingHandler.LogInfo("Authentication Success!!!!");
            return AuthenticateResult.Success(ticket);
        }
    }
}
