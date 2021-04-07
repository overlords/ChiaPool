using ChiaPool.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ChiaPool.Authentication
{
    public class PlotterAuthenticationHandler : AuthenticationHandler<PlotterAuthenticationOptions>
    {
        private readonly MinerContext DbContext;

        public PlotterAuthenticationHandler(IOptionsMonitor<PlotterAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
                                          MinerContext dbContext)
            : base(options, logger, encoder, clock)
        {
            DbContext = dbContext;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var header) ||
                !AuthenticationHeaderValue.TryParse(header, out var authHeader) ||
                authHeader.Scheme != Scheme.Name)
            {
                return AuthenticateResult.NoResult();
            }

            string token = authHeader.Parameter;
            var plotter = await DbContext.Plotters.FirstOrDefaultAsync(x => x.Token == token);

            if (plotter == null)
            {
                return AuthenticateResult.Fail("Invalid token");
            }

            var claims = new[]
            {
                new Claim("Token", token),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
