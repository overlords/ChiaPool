﻿using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ChiaPool.Authentication
{
    public class BasicAuthHandler : AuthenticationHandler<BasicAuthOptions>
    {
        private readonly MinerContext DbContext;
        private readonly HashingService HashingService;

        public BasicAuthHandler(IOptionsMonitor<BasicAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            MinerContext dbContext, HashingService hashingService)
            : base(options, logger, encoder, clock)
        {
            DbContext = dbContext;
            HashingService = hashingService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var header) ||
                !AuthenticationHeaderValue.TryParse(header, out var authHeader) ||
                authHeader.Scheme != Scheme.Name)
            {
                return AuthenticateResult.NoResult();
            }

            byte[] authBytes = Convert.FromBase64String(authHeader.Parameter);
            string userAndPass = Encoding.UTF8.GetString(authBytes);
            string[] parts = userAndPass.Split(':');
            if (parts.Length != 2)
            {
                return AuthenticateResult.Fail($"Invalid {Scheme.Name} authentication header");
            }
            string username = parts[0];
            string password = parts[1];
            string passwordHash = HashingService.HashString(password);

            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Name == username && x.PasswordHash == passwordHash);

            if (user == null)
            {
                return AuthenticateResult.Fail("Invalid username or password");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
