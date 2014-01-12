using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PureSeeder.Core.Annotations;
using PureSeeder.Core.Configuration;

namespace PureSeeder.Core.Monitoring
{
    public delegate void ProcessStateChangeHandler(object sender, ProcessStateChangeEventArgs e);

    public class ProcessMonitor
    {
        private readonly ICrashDetector _crashDetector;

        public ProcessMonitor([NotNull] ICrashDetector crashDetector)
        {
            if (crashDetector == null) throw new ArgumentNullException("crashDetector");
            _crashDetector = crashDetector;
        }

        // Need a couple of custom events to attach to
       public event ProcessStateChangeHandler OnProcessStateChanged; 

       public async void CheckOnProcess(CancellationToken ct)
        {
            await Task.Run(() =>
                {
                    // Check for the process every few seconds
                    // If it's running, trigger idle kick avoidance
                    // If state changes from running or to running, trigger an event

                    var processList = Process.GetProcessesByName(Constants.Games.First().ProcessName);
                    var process = processList.FirstOrDefault();

                    var isRunning = process != null;
                    var previousState = isRunning;

                    while (!ct.IsCancellationRequested)
                    {
                        process = Process.GetProcessesByName(Constants.Games.First().ProcessName).FirstOrDefault();
                        isRunning = process != null;
                        if (OnProcessStateChanged != null)
                            OnProcessStateChanged.Invoke(this, new ProcessStateChangeEventArgs() {IsRunning = isRunning});

                        previousState = isRunning;

                        _crashDetector.DetectCrash(Constants.Games.First().ProcessName, Constants.Games.First().WindowTitle);

                        Thread.Sleep(5000);
                    }
                });
        }

        
    }

    public interface ICrashDetector
    {
        void DetectCrash(string processName, string windowTitle);
    }

    class CrashDetector : ICrashDetector
    {
        private readonly ICrashHandler _crashHandler;

        public CrashDetector([NotNull] ICrashHandler crashHandler)
        {
            if (crashHandler == null) throw new ArgumentNullException("crashHandler");
            _crashHandler = crashHandler;
        }

        public void DetectCrash(string processName, string windowTitle)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            if (process == null)
                return;

            if(!process.Responding)
                _crashHandler.HandleCrash(process, processName, windowTitle);
        }
    }

    public interface ICrashHandler
    {
        void HandleCrash(Process process, string processName, string windowTitle);
    }

    class CrashHandler : ICrashHandler
    {
        public void HandleCrash(Process process, string processName, string windowTitle)
        {
            if(process != null)
                process.Kill();

            // Find the fault window and kill it
            var faultProcess =
                    Process.GetProcessesByName("WerFault")
                           .FirstOrDefault(x => x.MainWindowTitle == windowTitle);
            
            if (faultProcess == null)
                return;

            faultProcess.Kill(); // Kill the fault window
        }
    }

    public class ProcessStateChangeEventArgs : EventArgs
    {
        public bool IsRunning { get; set; }
    }
}