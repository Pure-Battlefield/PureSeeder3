using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Timers;
using Newtonsoft.Json;
using PureSeeder.Core.Annotations;
using System.Linq;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Settings;
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
        /// Exports settings to a json file
        /// </summary>
        /// <param name="filename"></param>
        void ExportSettings(string filename);

        /// <summary>
        /// Imports settings from a json file
        /// </summary>
        /// <param name="filename"></param>
        void ImportSettings(string filename);
       
        
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

        public SeederContext(SessionData sessionData, BindableSettings bindableSettings, IDataContextUpdater[] updaters)
        {
            if (sessionData == null) throw new ArgumentNullException("sessionData");
            if (bindableSettings == null) throw new ArgumentNullException("bindableSettings");
            if (updaters == null) throw new ArgumentNullException("updaters");

            _sessionData = sessionData;
            _bindableSettings = bindableSettings;
            _updaters = updaters;

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

        public void ExportSettings(string filename)
        {
            var json = JsonConvert.SerializeObject(_bindableSettings, Formatting.Indented);
            File.WriteAllText(filename, json);
        }

        public void ImportSettings(string filename)
        {
            var jsonText = File.ReadAllText(filename);
            var newSettings = PartialObject<BindableSettings>.Create(jsonText);

            // Todo: This probably isn't the best way of doing this
            //  - should probably have some method of denoting if a setting is importable, then it should run
            //    newSettings.MergeValue() on each setting that is importable
            newSettings.MergeValue((BindableSettings x) => x.Servers, _bindableSettings);
            newSettings.MergeValue((BindableSettings x) => x.RefreshInterval, _bindableSettings);
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
            var bfProcess = Process.GetProcessesByName("bf4"); // Process name is bf4.exe (in Details tab of Task Manager)
            
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
        
        public void JoinServer()
        {
            
        }

        private void OnContextUpdated()
        {
            var handler = OnContextUpdate;
            if (handler != null)
                handler(this, new EventArgs());
        }
    }
}
