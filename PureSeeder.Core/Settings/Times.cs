using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureSeeder.Core.Context;
using Newtonsoft.Json;

namespace PureSeeder.Core.Settings
{
    class Times : BindableBase, ITimes
    {
        private string _startString;
        private string _endString;
        private TimeSpan _start;
        private TimeSpan _end;
        private ServerStatusCollection _serversStatuses;

        public Times()
        {
            _serversStatuses = new ServerStatusCollection();
        }

        [Description("Start time of server group")]
        public string StartString
        {
            get { return this._startString; }
            set {
                if (value.Length != 6)
                {
                    throw new ArgumentException("StartString must be 6 digits");
                }
                Convert.ToInt32(value);
                SetField(ref _startString, value);
                var time = value.ToArray();
                var hours = time[0] + time[1];
                var minutes = time[2] + time[3];
                var seconds = time[4] + time[5];
                SetField(ref _start, 
                    new TimeSpan(Convert.ToInt32(hours), 
                                 Convert.ToInt32(minutes), 
                                 Convert.ToInt32(seconds)));
            }
        }

        [Description("End time of server group")]
        public string EndString
        {
            get { return this._endString; }
            set {
                if (value.Length != 6)
                {
                    throw new ArgumentException("StartString must be 6 digits");
                }
                Convert.ToInt32(value);
                SetField(ref _endString, value);
                var time = value.ToArray();
                var hours = time[0] + time[1];
                var minutes = time[2] + time[3];
                var seconds = time[4] + time[5];
                SetField(ref _end,
                    new TimeSpan(Convert.ToInt32(hours),
                                 Convert.ToInt32(minutes),
                                 Convert.ToInt32(seconds)));
            }
        }

        [Description("Servers in the group")]
        public Servers Servers
        {
            set { this._serversStatuses.SetInnerServerCollection(value); }
            get { return this._serversStatuses.Servers;  }
        }

        [JsonIgnore]
        public TimeSpan Start
        {
            get { return this._start; }
            set { SetField(ref _start, value); }
        }

        [Description("End time of server group")]
        public TimeSpan End
        {
            get { return this._end; }
            set { SetField(ref _end, value); }
        }


        public ServerStatusCollection ServerStatuses { get { return this._serversStatuses; } }

        public bool IsInTime()
        {
            DateTime now = DateTime.Now;
            return (TimeSpan.Compare(now.TimeOfDay, this._start) >= 0) && (TimeSpan.Compare(now.TimeOfDay, this._end) < 0);
        }
    }
}
