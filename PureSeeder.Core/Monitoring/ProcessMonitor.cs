using System;
using System.Threading.Tasks;

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
                });
        }


    }

    public class ProcessStateChangeEventArgs : EventArgs
    {
        public bool IsRunning { get; set; }
    }
}