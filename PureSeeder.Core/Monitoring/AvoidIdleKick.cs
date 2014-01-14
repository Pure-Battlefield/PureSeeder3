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
        public async void AvoidIdleKick(CancellationToken token)
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
                            //AutoItX.ControlClick(winHandle, "", "", "LEFT", 1, 0, 0);
                            AutoItX.ControlClick(winHandle, IntPtr.Zero, "left", 1, 0, 0);
                            Thread.Sleep(1000);
                            AutoItX.WinActivate("Program Manager");
                        }

                        Thread.Sleep(1 * 60 * 1000);  // Sleep for a minute
                    }
                });
        }
    }
}
