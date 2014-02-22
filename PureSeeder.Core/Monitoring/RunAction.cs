using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PureSeeder.Core.Configuration;

namespace PureSeeder.Core.Monitoring
{
    class RunAction
    {
        public Task RunActionOnGameLoad(CancellationToken token, Func<GameInfo> getCurrentGame, Action action)
        {
            return Task.Factory.StartNew(() =>
            {
                var currentGame = getCurrentGame.Invoke();
                while (!token.IsCancellationRequested)
                {
                    var process = Process.GetProcessesByName(currentGame.ProcessName).FirstOrDefault();

                    if (process == null)
                    {
                        Thread.Sleep(1000); // Sleep for a second
                        continue;
                    }
                    
                    action.Invoke();
                    break;
                }
            });
        }
    }
}