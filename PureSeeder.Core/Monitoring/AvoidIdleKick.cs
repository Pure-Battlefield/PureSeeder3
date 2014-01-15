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
        public async void AvoidIdleKick(CancellationToken token, int numSeconds)
        {
            await Task.Run(() =>
                {
                    var winHandle = AutoItX.WinGetHandle("Crash");

                    var winExists = AutoItX.WinExists(winHandle) != 0;

                    while (!token.IsCancellationRequested)
                    {
                        winHandle = AutoItX.WinGetHandle(Constants.Games.First().WindowTitle);
                        winExists = AutoItX.WinExists(winHandle) != 0;

                        if (winExists)
                        {
                            //AutoItX.ControlClick(winHandle, IntPtr.Zero, "left", 1, 0, 0);
                            AutoItX.ControlClick(Constants.Games.First().WindowTitle, "", "", "left", 1, 1, 1);
                            //Thread.Sleep(500);
                            AutoItX.WinActivate("Program Manager");
                        }

                        Thread.Sleep(1 * numSeconds * 1000);  // Sleep for a minute
                    }
                });
        }
    }
}
