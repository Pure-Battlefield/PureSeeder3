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

       public /*async*/ Task CheckOnProcess(CancellationToken ct, Func<GameInfo> getGameInfo)
        {
            /*await*/ return Task.Factory.StartNew(() =>
                {
                    // Check for the process every few seconds
                    // If it's running, trigger idle kick avoidance
                    // If state changes from running or to running, trigger an event
                    var previousState = false;

                    while (!ct.IsCancellationRequested)
                    {
                        var currentGame = getGameInfo.Invoke();

                        if (currentGame != null)
                        {
                            var process =
                                Process.GetProcessesByName(currentGame.ProcessName).FirstOrDefault();
                            var isRunning = process != null;
                            if (isRunning != previousState && OnProcessStateChanged != null)
                                OnProcessStateChanged.Invoke(this,
                                                             new ProcessStateChangeEventArgs() {IsRunning = isRunning});

                            previousState = isRunning;

                            _crashDetector.DetectCrash(currentGame.ProcessName,
                                                       currentGame.FaultWindowTitle);
                        }

                        Thread.Sleep(5000);
                    }
                });
        }
    }

    public interface ICrashDetector
    {
        void DetectCrash(string processName, string faultWindowTitle);
    }

    class CrashDetector : ICrashDetector
    {
        private readonly ICrashHandler _crashHandler;

        public CrashDetector([NotNull] ICrashHandler crashHandler)
        {
            if (crashHandler == null) throw new ArgumentNullException("crashHandler");
            _crashHandler = crashHandler;
        }

        public void DetectCrash(string processName, string faultWindowTitle)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            if (process == null)
                return;

            if(!process.Responding)
                _crashHandler.HandleCrash(process, processName, faultWindowTitle);
        }
    }

    public interface ICrashHandler
    {
        void HandleCrash(Process process, string processName, string faultWindowTitle);
    }

    class CrashHandler : ICrashHandler
    {
        public void HandleCrash(Process process, string processName, string faultWindowTitle)
        {
            if(process != null)
                process.Kill();

            // Find the fault window and kill it
            var faultProcess =
                    Process.GetProcessesByName("WerFault")
                           .FirstOrDefault(x => x.MainWindowTitle == faultWindowTitle);
            
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