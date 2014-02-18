using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PureSeeder.Core.Configuration;

namespace PureSeeder.Core.Monitoring
{
    class GameMinimizer
    {
        public Task MinimizeGameOnce(CancellationToken token, Func<GameInfo> getCurrentGame)
        {
            return Task.Factory.StartNew(() =>
                {
                    var currentGame = getCurrentGame.Invoke();
                    while (!token.IsCancellationRequested)
                    {
                        var process = Process.GetProcessesByName(currentGame.ProcessName).FirstOrDefault();

                        if (process != null)
                        {
                            if (currentGame != null)
                            {
                                PInvoke.MinimizeWindow(currentGame.WindowTitle);
                                break;
                            }
                        }

                        Thread.Sleep(1000); // Sleep for a second
                    }
                });
        }
    }
}