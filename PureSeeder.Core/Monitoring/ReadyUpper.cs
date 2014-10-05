using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PureSeeder.Core.Configuration;

namespace PureSeeder.Core.Monitoring
{
    public class ReadyUpper
    {
        public Task ReadyUp(CancellationToken token, int numSeconds, Func<GameInfo> getCurrentGame)
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
                            PInvoke.SendEnterPress(currentGame.WindowTitle);
                        }
                    }

                    Thread.Sleep(1*numSeconds*1000);
                }
            });
        }
    }
}