using Common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public sealed class ConfigurationService 
    {
        public async Task InstallAsync(string username, string password, string minerToken)
        {

        }

        public async Task<Dictionary<string, string>> GetVolumesAsync()
        {
            if (!IsInstalled())
            {
                throw new InvalidOperationException("App not installed!");
            }

            string[] composeFileContents = await ReadComposeFileAsync();
            var volumeDefinitions = composeFileContents.SkipWhile(x => x.Trim() != "volumes:")
                                                       .TakeWhile(x => x.Trim().StartsWith('-'))
                                                       .Where(x => !x.Trim().StartsWith('#'))
                                                       .Select(x => x.Trim());

            var volumes = new Dictionary<string, string>();

            foreach(string volume in volumeDefinitions)
            {
                int splitIndex = volume.LastIndexOf(':');

                string externalPath = volume.Substring(0, splitIndex);
                string internalPath = volume.Substring(splitIndex + 1);
                volumes.Add(externalPath, internalPath);
            }

            return volumes;
        }



        private Task<string[]> ReadComposeFileAsync()
            => File.ReadAllLinesAsync(GetInstallationPath() + "\\docker-compose.yml");
        private Task<string[]> ReadEnvFileAsync()
            => File.ReadAllLinesAsync(GetInstallationPath() + "\\harvester.env");

        private Task WriteComposeFileAsync(string[] content)
            => File.WriteAllLinesAsync(GetInstallationPath() + "\\docker-compose.yml", content);
        private Task WriteEnvFileAsync(string[] content)
            => File.WriteAllLinesAsync(GetInstallationPath() + "\\harvester.env", content);

        public static bool IsInstalled()
            => Directory.Exists(GetInstallationPath());
        public static string GetInstallationPath()
            => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\ChiaPool\\";
    }
}
