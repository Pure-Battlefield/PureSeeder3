using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PureSeeder.Core.Configuration;
using AutoIt;

namespace PureSeeder.Core.Monitoring
{
    class IdleKickAvoider
    {
        public Task AvoidIdleKick(CancellationToken token, int numSeconds, Func<GameInfo> getCurrentGame)
        {
            return Task.Factory.StartNew(() => {
                    while (!token.IsCancellationRequested)
                    {
                        var currentGame = getCurrentGame.Invoke();

                        if (currentGame != null)
                        {
                            PInvoke.ClickInWindow(currentGame.WindowTitle, 20, 20);
                        }

                        Thread.Sleep(1 * numSeconds * 1000);  // Sleep
                    }
                });
        }
    }
}
