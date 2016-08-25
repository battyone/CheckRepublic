using System;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Checks;
using Newtonsoft.Json;

namespace Knapcode.CheckRepublic.Sandbox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args, CancellationToken.None).Wait();
        }

        public static async Task MainAsync(string[] args, CancellationToken token)
        {
            var checkRunner = new CheckRunner();
            var check = new IsNuGetToolsUp();
            var checkResult = await checkRunner.ExecuteCheckAsync(check, token);

            var output = JsonConvert.SerializeObject(checkResult);
            Console.WriteLine(output);
        }
    }
}
