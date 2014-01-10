using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PureSeeder.Core.Configuration;

namespace PureSeeder.Core.Monitoring
{
    public delegate void ProcessStateChangeHandler(object sender, ProcessStateChangeEventArgs e);

    public class ProcessMonitor
    {
        // Need a couple of custom events to attach to
        public event ProcessStateChangeHandler OnProcessStateChanged; 

        public async void CheckOnProcess(object sender, EventArgs e)
        {
            await Task.Run(() =>
                {
                    // Check for the process every few seconds
                    // If it's running, trigger idle kick avoidance
                    // If state changes from running or to running, trigger an event

                    Thread.Sleep(10000);
                });
        }
        
        public async void CheckOnProcess(CancellationToken ct)
        {
            await Task.Run(() =>
                {
                    // Check for the process every few seconds
                    // If it's running, trigger idle kick avoidance
                    // If state changes from running or to running, trigger an event

                    var processList = Process.GetProcessesByName(Constants.ProcessNames.Bf4);
                    var process = processList.FirstOrDefault();

                    var isRunning = process != null;
                    var previousState = isRunning;
                    
                    while (!ct.IsCancellationRequested)
                    {
                        process = Process.GetProcessesByName(Constants.ProcessNames.Bf4).FirstOrDefault();
                        isRunning = process != null;
                        if (isRunning != previousState)
                        {
                            if(OnProcessStateChanged != null)
                                OnProcessStateChanged.Invoke(this, new ProcessStateChangeEventArgs() {IsRunning = isRunning});

                            previousState = isRunning;

                            await CrashDetection();
                        }

                        Thread.Sleep(5000);
                    }

                });
        }

        private async Task CrashDetection()
        {
            await Task.Run(() =>
                {
                    var process = Process.GetProcessesByName(Constants.ProcessNames.Bf4).FirstOrDefault();
                    if (process == null)
                        return;

                    if (!process.Responding) // Check if the process is responding
                    {
                        process.Kill(); // Kill the process
                        var faultProcess =
                            Process.GetProcessesByName("WerFault")
                                   .FirstOrDefault(x => x.MainWindowTitle == Constants.WindowTitles.Bf4FaultWindow); // Find the fault window

                        if (faultProcess == null)
                            return;

                        faultProcess.Kill(); // Kill the fault window
                    }
                });
        }

       
    }

    public class ProcessStateChangeEventArgs : EventArgs
    {
        public bool IsRunning { get; set; }
    }
}