using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Monitoring;

namespace PureSeeder.Core.ProcessControl
{
    public interface IProcessController
    {
        Task StopGame();
        Task MinimizeAfterLaunch();
        Task<bool> BfIsRunning();

        ProcessMonitor GetProcessMonitor();
        IdleKickAvoider GetIdleKickAvoider();
    }

    class ProcessController : IProcessController
    {
        public async Task StopGame()
        {
            if (!await BfIsRunning())
                return;

            var process = Process.GetProcessesByName(Constants.Games.Bf4.ProcessName).FirstOrDefault();

            if (process != null)
                process.Close();
        }

        public async Task MinimizeAfterLaunch()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5 * 60 * 1000);  // Cancel after 5 mins

            await new GameMinimizer().MinimizeGameOnce(cts.Token, () => Constants.Games.Bf4);
        }

        public Task<bool> BfIsRunning()
        {
            throw new System.NotImplementedException();
        }

        public ProcessMonitor GetProcessMonitor()
        {
            return new ProcessMonitor(
                new ICrashDetector[]
                {
                    new CrashDetector(new CrashHandler()), new DetectCrashByFaultWindow(), 
                });
        }

        public IdleKickAvoider GetIdleKickAvoider()
        {
            return new IdleKickAvoider();
        }
    }
}