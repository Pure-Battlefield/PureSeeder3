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
        private readonly ICrashDetector[] _crashDetectors;

        public ProcessMonitor([NotNull] ICrashDetector[] crashDetectors)
        {
            if (crashDetectors == null) throw new ArgumentNullException("crashDetectors");
            _crashDetectors = crashDetectors;
        }

        // Need a couple of custom events to attach to
       public event ProcessStateChangeHandler OnProcessStateChanged; 

       public /*async*/ Task CheckOnProcess(CancellationToken ct, Func<GameInfo> getGameInfo, SynchronizationContext context)
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
                                context.Post(_ => OnProcessStateChanged(this, new ProcessStateChangeEventArgs() {IsRunning = isRunning}), null);
                                //OnProcessStateChanged.Invoke(this,new ProcessStateChangeEventArgs() {IsRunning = isRunning}); // Deprecated

                            previousState = isRunning;

                            foreach (var crashDetector in _crashDetectors)
                            {
                                crashDetector.DetectCrash(currentGame.ProcessName,
                                                           currentGame.FaultWindowTitle);    
                            }
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

    /// <summary>
    /// If the app is not responding, then it has crashed/locked up
    /// </summary>
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

    /// <summary>
    /// If there is a fault window, then it has crashed/locked up
    /// </summary>
    class DetectCrashByFaultWindow : ICrashDetector
    {
        public void DetectCrash(string processName, string faultWindowTitle)
        {
            var faultProcess = Process.GetProcessesByName("WerFault")
                                      .FirstOrDefault(x => x.MainWindowTitle == faultWindowTitle);

            if (faultProcess == null)
                return;

            // Assume the game has crashed
            faultProcess.Kill();

            var gameProcess = Process.GetProcessesByName(processName).FirstOrDefault();
            if (gameProcess != null)
                gameProcess.Kill();
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