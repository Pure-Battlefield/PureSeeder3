using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Timers;
using PureSeeder.Core.Annotations;
using System.Linq;
using PureSeeder.Core.Configuration;
using Server = PureSeeder.Core.Settings.Server;

//using Server = PureSeeder.Core.Configuration.Server;

namespace PureSeeder.Core.Context
{
    public interface IDataContext
    {
        /// <summary>
        /// Data related to the current session
        /// </summary>
        SessionData Session { get; }
        /// <summary>
        /// Any user-changeable settings
        /// </summary>
        BindableSettings Settings { get; }
        
        /// <summary>
        /// Check if the currently logged in user matches the expected user
        /// </summary>
        bool IsCorrectUser { get; }
        /// <summary>
        /// Check if seeding should occur
        /// </summary>
        bool ShouldSeed { get; }
        /// <summary>
        /// Check if self-kicking should occur
        /// </summary>
        bool ShouldKick { get; }
        
        /// <summary>
        /// Update current status with the given page data
        /// </summary>
        /// <param name="pageData">Raw page data</param>
        void UpdateStatus(string pageData);
        
        /// <summary>
        /// Event fired when UpdateStatus is complete
        /// </summary>
        event ContextUpdatedHandler OnContextUpdate;
        /// <summary>
        /// Event fired when Hang Protection is invoked
        /// </summary>
        event HangProtectionInvokedHandler OnHangProtectionInvoke;

        /// <summary>
        /// Update the context in any necessary ways when a server is joined
        /// </summary>
        void JoinServer();
    }

    public delegate void ContextUpdatedHandler(object sender, EventArgs e);
    public delegate void HangProtectionInvokedHandler(object sender, EventArgs e);
    
    public class SeederContext : IDataContext
    {
        private readonly SessionData _sessionData;
        private readonly BindableSettings _bindableSettings;
        private readonly IDataContextUpdater[] _updaters;
        private readonly Timer _hangProtectionTimer;

        public SeederContext(SessionData sessionData, BindableSettings bindableSettings, IDataContextUpdater[] updaters)
        {
            if (sessionData == null) throw new ArgumentNullException("sessionData");
            if (bindableSettings == null) throw new ArgumentNullException("bindableSettings");
            if (updaters == null) throw new ArgumentNullException("updaters");

            _sessionData = sessionData;
            _bindableSettings = bindableSettings;
            _updaters = updaters;

            _hangProtectionTimer = new Timer(Constants.GameHangProtectionTimerInterval * 60 * 1000);
            _hangProtectionTimer.Elapsed += InvokeHangProtection;

            _sessionData.PropertyChanged += HangProtectionStatusChanged;

        }

        public SessionData Session { get { return _sessionData; } }
        public BindableSettings Settings { get { return _bindableSettings; }}

        public bool IsCorrectUser
        {
            get
            {
                return String.Equals(this._bindableSettings.Username, this._sessionData.CurrentLoggedInUser,
                                     StringComparison.InvariantCultureIgnoreCase);
            }
        }
        
        public void UpdateStatus(string pageData)
        {
            foreach (var updater in _updaters)
            {
                updater.UpdateContextData(this, pageData);
            }

            OnContextUpdated();
        }

        public bool ShouldSeed
        {
            get 
            { 
                var shouldSeed = true;
                // There are less than or equal to MinPlayers in the server
                shouldSeed &= _sessionData.CurrentPlayers <=
                              _bindableSettings.Servers[_bindableSettings.CurrentServer].MinPlayers;

                shouldSeed &= !BfIsRunning();

                return shouldSeed;
            }
        }

        // Todo: This should be abstracted and injected
        private bool BfIsRunning()
        {
            // Todo: The process name should be injected so it can work with BF3
            var bfProcess = Process.GetProcessesByName("bf4");
            return bfProcess.Length != 0;
        }

        public bool ShouldKick
        {
            get 
            { 
                var shouldKick = true;

                shouldKick &= _sessionData.CurrentPlayers >
                              _bindableSettings.Servers[_bindableSettings.CurrentServer].MaxPlayers;

                shouldKick &= BfIsRunning();

                return shouldKick;
            }
        }

        public event ContextUpdatedHandler OnContextUpdate;
        public event HangProtectionInvokedHandler OnHangProtectionInvoke;
        
        public void JoinServer()
        {
            SetHangProtectionTimer();
        }

        private void OnContextUpdated()
        {
            var handler = OnContextUpdate;
            if (handler != null)
                handler(this, new EventArgs());
        }

        private void OnHangProtectionInvoked()
        {
            var handler = OnHangProtectionInvoke;
            if (handler != null)
                handler(this, new EventArgs());
        }

        private void HangProtectionStatusChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "EnableGameHangProtection")
                return;

            SetHangProtectionTimer();
        }

        private void SetHangProtectionTimer()
        {
            if (!_bindableSettings.EnableGameHangProtection)
            {
                _hangProtectionTimer.Stop();
                return;
            }

            if (!BfIsRunning())
                return;

            _hangProtectionTimer.Start();
        }

        private void InvokeHangProtection(object sender, ElapsedEventArgs e)
        {
            OnHangProtectionInvoked();
        }
    }


    public interface IDataContextUpdater
    {
        void UpdateContextData(IDataContext context, string pageData);
    }

    class Bf4PlayerCountsUpdater : IDataContextUpdater
    {
        public void UpdateContextData(IDataContext context, string pageData)
        {
            //""slots"".*?""2"":{""current"":(.*?),""max"":(.*?)}
            // Todo: Make regex pattern a global setting so it can more easily be changed
            var curPlayersRegEx = new Regex(@"""slots"".*?""2"":{""current"":(.*?),""max"":(.*?)}");

            var curPlayers = curPlayersRegEx.Match(pageData);

            if (!curPlayers.Success)
            {
                context.Session.CurrentPlayers = null;
                context.Session.ServerMaxPlayers = null;
                return;
            }

            int currentPlayers, maxPlayers;

            int.TryParse(curPlayers.Groups[1].Value, out currentPlayers);
            int.TryParse(curPlayers.Groups[2].Value, out maxPlayers);

            context.Session.CurrentPlayers = currentPlayers;
            context.Session.ServerMaxPlayers = maxPlayers;
        }
    }

    class CurrentBf4UserUpdater : IDataContextUpdater
    {
        public void UpdateContextData(IDataContext context, string pageData)
        {
            // Todo: Make regex pattern a global setting so it can more easily be changed
            var curUserRegEx = new Regex(@"class=""username""\W*href=""/bf4/user/(.*?)/");

            var curUser = curUserRegEx.Match(pageData);

            if (!curUser.Success)
            {
                context.Session.CurrentLoggedInUser = "None";
                return;
            }

            context.Session.CurrentLoggedInUser = curUser.Groups[1].Value;
        }
    }

    

    public class CurrentPlayerCountException : ContextUpdateException
    {
        
    }

    public class ContextUpdateException : Exception
    {
        
    }
}
