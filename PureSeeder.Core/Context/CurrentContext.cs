using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using PureSeeder.Core.Annotations;
using PureSeeder.Core.Configuration;
using System.Linq;
using PureSeeder.Core.Settings;
using Server = PureSeeder.Core.Settings.Server;

//using Server = PureSeeder.Core.Configuration.Server;

namespace PureSeeder.Core.Context
{
    public interface IDataContext
    {
        Servers Servers { get; set; }
        int? CurrentPlayers { get; set; }
        int? ServerMaxPlayers { get; set; }
        bool HangProtectionStatus { get; set; }
        bool SeedingEnabled { get; set; }
        string Username { get; set; }
        bool LoggingEnabled { get; set; }
        Server CurrentServer { get; set; }
        string CurrentLoggedInUser { get; set; }
        Constants.Game CurrentGame { get; set; }

        bool IsCorrectUser { get; }

        void UpdateStatus(string pageData);

        event PropertyChangedEventHandler PropertyChanged;
    }

    public class SeederContext : BindableBase, IDataContext
    {
        private readonly SeederUserSettings _settings;
        private readonly IDataContextUpdater[] _updaters;

        public SeederContext([NotNull] SeederUserSettings settings, [NotNull] IDataContextUpdater[] updaters)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            if (updaters == null) throw new ArgumentNullException("updaters");
            _settings = settings;
            _updaters = updaters;

            SetLocalDefaults();
        }

        #region Session Data

        private int? _currentPlayers;
        private int? _serverMaxPlayers;
        private bool _seedingEnabled;
        private string _currentLoggedInUser;
        private Constants.Game _currentGame;
       
        public int? CurrentPlayers
        {
            get { return this._currentPlayers; }
            set { SetProperty(ref _currentPlayers, value); }
        }

        public int? ServerMaxPlayers
        {
            get { return this._serverMaxPlayers; }
            set { SetProperty(ref _serverMaxPlayers, value); }
        }

        public bool SeedingEnabled
        {
            get { return this._seedingEnabled; }
            set { SetProperty(ref this._seedingEnabled, value); }
        }

        public string CurrentLoggedInUser
        {
            get { return this._currentLoggedInUser; }
            set { SetProperty(ref _currentLoggedInUser, value); }
        }

        public Constants.Game CurrentGame
        {
            get { return this._currentGame; }
            set { SetProperty(ref _currentGame, value); }
        }

        public bool IsCorrectUser
        {
            get
            {
                return String.Equals(this.Username, this.CurrentLoggedInUser,
                                     StringComparison.InvariantCultureIgnoreCase);
            }
        }

        #endregion
        
        #region Settings Data

        public Servers Servers
        {
            get
            {
                if (_settings.Servers == null)
                {
                    _settings.SetDefaultServers();
                    if (_settings.CurrentServer == null)
                        _settings.CurrentServer = _settings.Servers[_settings.Servers.CurrentServerIndex];
                    _settings.Save();
                }

                if (_settings.CurrentServer == null)
                    _settings.CurrentServer = _settings.Servers[_settings.Servers.CurrentServerIndex];
                return _settings.Servers;
            }
            set
            {
                SetProperty(_settings, value, x => x.Servers);
                _settings.Save();
            }
        }
        
        public string Username
        {
            get { return _settings.Username; }
            set
            {
                //SetProperty(ref _settings.Username, value);
                SetProperty(_settings, value, x => x.Username);
                _settings.Save();
            }
        }

        public bool HangProtectionStatus
        {
            get { return _settings.EnableGameHangProtection; }
            set
            {
                //SetProperty(ref _hangProtectionStatus, value);
                SetProperty(_settings, value, x => x.EnableGameHangProtection);
                _settings.Save();
            }
        }

        public bool LoggingEnabled
        {
            get { return _settings.EnableLogging; }
            set 
            { 
                SetProperty(_settings, value, x => x.EnableLogging);
                _settings.Save();
            }
        }

        public Server CurrentServer
        {
            get { return _settings.CurrentServer; }
            set
            {
                SetProperty(_settings, value, x => x.CurrentServer);
                _settings.Save();
            }
        }

        #endregion
        
        
        public void UpdateStatus(string pageData)
        {
            foreach (var updater in _updaters)
            {
                updater.UpdateContextData(this, pageData);
            }
        }

        private void SetLocalDefaults()
        {
            CurrentPlayers = null;
            ServerMaxPlayers = null;
            SeedingEnabled = true;
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
                context.CurrentPlayers = null;
                context.ServerMaxPlayers = null;
                return;
            }

            int currentPlayers, maxPlayers;

            int.TryParse(curPlayers.Groups[1].Value, out currentPlayers);
            int.TryParse(curPlayers.Groups[2].Value, out maxPlayers);

            context.CurrentPlayers = currentPlayers;
            context.ServerMaxPlayers = maxPlayers;
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
                context.CurrentLoggedInUser = "None";
                return;
            }

            context.CurrentLoggedInUser = curUser.Groups[1].Value;
        }
    }

    

    public class CurrentPlayerCountException : ContextUpdateException
    {
        
    }

    public class ContextUpdateException : Exception
    {
        
    }

    
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected bool SetProperty<T1, T2>(T1 storage, T2 value, Expression<Func<T1, T2>> outExpr)
        {
            if (object.Equals(storage, value)) return false;

            var expr = (MemberExpression) outExpr.Body;
            var prop = (PropertyInfo) expr.Member;
            prop.SetValue(storage, value);
            this.OnPropertyChanged(prop.Name);
            return true;
        }
    }
}
