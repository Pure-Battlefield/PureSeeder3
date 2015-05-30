using System;
using System.Collections.Generic;
using System.Linq;

namespace PureSeeder.Core.Settings
{
    public class TimesCollection
    {
        private Times _currentTimes;
        private TimesCollection _start;
        private TimesCollection _rest;

        public Times CurrentTimes { get { return this._currentTimes; } }

        public TimesCollection()
        {

        }

        private TimesCollection(Times times, TimesCollection start)
        {
            this._currentTimes = times;
            this._start = start;
            this._rest = null;
        }

        public void Add(Times times)
        {
            if (this._start == null)
            {
                this._currentTimes = times;
                this._start = this;
            }
            else
            {
                this._rest = new TimesCollection(times, this._start);
            }
        }

        public TimesCollection Next()
        {
            if (this._rest == null)
            {
                return this._start;
            } 
            else
            {
                return this._rest;
            }
        }

        public bool HasStarted(TimeSpan now)
        {
            return TimeSpan.Compare(now, this._currentTimes.Start) <= 0;
        }

        public bool HasEnded(TimeSpan now)
        {
            return TimeSpan.Compare(now, this._currentTimes.End) > 0;
        }
    }
}
