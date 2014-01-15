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
            //await Task.Run(() =>
             //   {

            return Task.Factory.StartNew(() => {
                    while (!token.IsCancellationRequested)
                    {
                        var currentGame = getCurrentGame.Invoke();

                        if (currentGame != null)
                        {
                            var winHandle = AutoItX.WinGetHandle(currentGame.WindowTitle);
                            var winExists = AutoItX.WinExists(winHandle) != 0;

                            if (winExists)
                            {
                                //AutoItX.ControlClick(winHandle, IntPtr.Zero, "left", 1, 0, 0);
                                AutoItX.ControlClick(currentGame.WindowTitle, "", "", "left", 1, 1, 1);
                                
                                //Thread.Sleep(500);
                                AutoItX.WinActivate("Program Manager"); // Remove focus from the game
                            }
                        }

                        Thread.Sleep(1 * /*numSeconds*/ 10 * 1000);  // Sleep
                    }
                });
        }
    }
}
