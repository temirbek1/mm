using Microsoft.Extensions.Options;
using API.Test.Config;

namespace API.Test.Services
{
    public class MyService
    {
        private readonly AppSettings _settings;

        public MyService(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public void PrintSettings()
        {
            Console.WriteLine(_settings.Setting1);
        }
    }
}
