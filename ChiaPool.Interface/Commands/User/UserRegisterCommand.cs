using ChiaPool.Api;
using ChiaPool.Extensions;
using ChiaPool.Models;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("User Register")]
    public sealed class UserRegisterCommand : ChiaCommand
    {
        private readonly ServerApiAccessor ServerAccessor;

        public UserRegisterCommand(ServerApiAccessor serverAccessor)
        {
            ServerAccessor = serverAccessor;
        }

        [CommandParameter(0, Name = "Name", Description = "The name of the user to register")]
        public string Username { get; set; }

        [CommandParameter(1, Name = "Password", Description = "The password of the user to register")]
        public string Password { get; set; }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var registerTask = ServerAccessor.RegisterUserAsync(Username);

            var userInfo = await registerTask.Try();

            if (userInfo == null)
            {
                var response = (registerTask.Exception.InnerException as HttpRequestException)?.StatusCode;
                if (response == null || !response.HasValue)
                {
                    throw registerTask.Exception.InnerException;
                }
                var responseCode = response.Value;

                if (responseCode == HttpStatusCode.NotFound)
                {
                    await ErrorLineAsync("This pool does not support user registration!");   
                }
                if (responseCode == HttpStatusCode.Conflict)
                {
                    await WarnLineAsync("This username has already been taken!");
                }
                else
                {
                    throw registerTask.Exception.InnerException;
                }

                return;
            }

            await SuccessLineAsync("Successfully created user!");
            await TableAsync(new Dictionary<string, Func<UserInfo, object>>()
            {
                ["Id"] = x => x.Id,
                ["Username"] = x => x.Name,
                ["Password"] = x => Password,
            }, new[] { userInfo });
        }
    }
}
