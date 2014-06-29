using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PureSeeder.Core.Annotations;
using System.Linq;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Monitoring;
using PureSeeder.Core.ServerManagement;
using PureSeeder.Core.Settings;
using Server = PureSeeder.Core.Settings.Server;

//using Server = PureSeeder.Core.Configuration.Server;

namespace PureSeeder.Core.Context
{
    public delegate void ContextUpdatedHandler(object sender, EventArgs e);
    public delegate void HangProtectionInvokedHandler(object sender, EventArgs e);
    
    public class SeederContext : IDataContext
    {
        private readonly SessionData _sessionData;
        private readonly BindableSettings _settings;
        private readonly IDataContextUpdater[] _updaters;
        private readonly IServerStatusUpdater _serverStatusUpdater;
        private readonly IPlayerStatusGetter _playerStatusGetter;

        public SeederContext(SessionData sessionData, BindableSettings bindableSettings, IDataContextUpdater[] updaters,
            [NotNull] IServerStatusUpdater serverStatusUpdater,
            [NotNull] IPlayerStatusGetter playerStatusGetter)
        {
            if (sessionData == null) throw new ArgumentNullException("sessionData");
            if (bindableSettings == null) throw new ArgumentNullException("bindableSettings");
            if (updaters == null) throw new ArgumentNullException("updaters");
            if (serverStatusUpdater == null) throw new ArgumentNullException("serverStatusUpdater");
            if (playerStatusGetter == null) throw new ArgumentNullException("playerStatusGetter");

            _sessionData = sessionData;
            _settings = bindableSettings;
            _updaters = updaters;
            _serverStatusUpdater = serverStatusUpdater;
            _playerStatusGetter = playerStatusGetter;

            _sessionData.ServerStatuses.SetInnerServerCollection(_settings.Servers);
        }

        public SessionData Session { get { return _sessionData; } }
        public BindableSettings Settings { get { return _settings; }}

        public void ExportSettings(string filename)
        {
            var json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
            File.WriteAllText(filename, json);
        }

        public void ImportSettings(string filename)
        {
            var jsonText = File.ReadAllText(filename);
            var newSettings = PartialObject<BindableSettings>.Create(jsonText);

            // Todo: This probably isn't the best way of doing this
            //  - should probably have some method of denoting if a setting is importable, then it should run automatically

            newSettings.MergeItem((BindableSettings x) => x.RefreshInterval, _settings);

            // Note: This is a little hacky but I'm not sure how to trigger a refresh on the binding when replacing the entire list
            var servers = new Servers();
            newSettings.MergeItem(x => x.Servers, ref servers);

            if (servers.Any())
            {
                _settings.Servers.Clear();
                //_settings.CurrentServer = 0; Deprecated
                foreach (var server in servers)
                {
                    _settings.Servers.Add(server);
                }
            }
        }

        public Task UpdateServerStatuses()
        {
            return _serverStatusUpdater.UpdateServerStatuses(this);
        }

        /// <summary>
        /// Runs all updaters that use the page source
        /// </summary>
        /// <param name="pageData">Page source from page currently loaded in browser</param>
        public void UpdateContextWithBrowserPage(string pageData)
        {
            foreach (var updater in _updaters)
            {
                updater.Update(this, pageData);
            }

            // Updating complete. Fire the event.
            OnContextUpdated();
        }

        // Todo: This should be abstracted and injected
        public bool IsSeeding()
        {
            return this.Session.BfIsRunning;
        }

        
        // Deprecated
//        public void StopGame()
//        {
//            if (!Session.BfIsRunning)
//                return;
//
//            var process = Process.GetProcessesByName(_sessionData.CurrentGame.ProcessName).FirstOrDefault();
//
//            if(process != null)
//                process.Close();
//        }

        public PlayerStatus GetPlayerStatus()
        {
            return _playerStatusGetter.GetPlayerStatus(this);
        }

        public event ContextUpdatedHandler OnContextUpdate;
        
        public void JoinServer()
        {
            //SpinUpMinimizer(); Deprecated
        }

        // Deprecated
//        private async void SpinUpMinimizer()
//        {
//            if (!this._settings.AutoMinimizeGame)
//                return;
//
//            var cts = new CancellationTokenSource();
//            cts.CancelAfter(300 * 1000);  // Cancel the background task after 5 minutes.
//
//            var minimizerCt = cts.Token;
//            await new GameMinimizer().MinimizeGameOnce(minimizerCt, () => Session.CurrentGame);
//        }

        /// <summary>
        /// Fires the OnContextUpdate event.
        /// </summary>
        private void OnContextUpdated()
        {
            var handler = OnContextUpdate;
            if (handler != null)
                handler(this, new EventArgs());
        }


        public UserStatus GetUserStatus()
        {
            if(String.Equals(_sessionData.CurrentLoggedInUser, _settings.Username, StringComparison.InvariantCultureIgnoreCase))
                return UserStatus.Correct;
            if(String.Equals(_sessionData.CurrentLoggedInUser, Constants.NotLoggedInUsername, StringComparison.InvariantCultureIgnoreCase))
                return UserStatus.None;

            return UserStatus.Incorrect;
        }
    }

 

    class PlayerStatusGetter : IPlayerStatusGetter
    {
        public PlayerStatus GetPlayerStatus(IDataContext context)
        {
            var httpClient = new HttpClient();

            /////

            //var serverResponse = httpClient.GetAsync("http://bf4-server1.purebattlefield.org").Result;

            /////

            var response = httpClient.GetStringAsync("http://battlelog.battlefield.com/bf4/").Result;

            var jsonRegex = new Regex(@"Surface\.globalContext = (.*)", RegexOptions.Singleline);

            var jsonMatch = jsonRegex.Match(response);

            if(!jsonMatch.Success)
                return new PlayerStatus(null, null);

            var json = jsonMatch.Groups[1].Value;

            var jObject = JObject.Parse(json);

            if(jObject == null)
                return new PlayerStatus(null, null);

            var user = jObject["session"]["user"];

            if(user == null)
                return new PlayerStatus(null, null);

            var username = user["username"].Value<string>();
            var currentServer = user["presence"]["playingMP"]["serverGuid"].Value<string>();

            return new PlayerStatus(username, currentServer);
        }
    }
}
