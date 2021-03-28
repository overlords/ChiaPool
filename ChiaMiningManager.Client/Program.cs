using System.Threading.Tasks;

namespace ChiaMiningManager
{
    public class Program
    {
        private static AppStartup Application;

        private static async Task Main(string[] args)
        {
            Application = new AppStartup();

            await Application.InitializeAsync();
            await Application.RunAsync();
        }
    }
}

