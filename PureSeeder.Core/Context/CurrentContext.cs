using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using PureSeeder.Core.Annotations;
using System.Linq;
using Server = PureSeeder.Core.Settings.Server;

//using Server = PureSeeder.Core.Configuration.Server;

namespace PureSeeder.Core.Context
{
    public interface IDataContext
    {
        SessionData Session { get; }
        BindableSettings Settings { get; }
        
        bool IsCorrectUser { get; }
        void UpdateStatus(string pageData);
        bool ShouldSeed { get; }
        bool ShouldKick { get; }
    }

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
        
        public void UpdateStatus(string pageData)
        {
            foreach (var updater in _updaters)
            {
                updater.UpdateContextData(this, pageData);
            }
        }

        public bool ShouldSeed
        {
            get 
            { 
                var shouldSeed = true;
                // There are less than or equal to MinPlayers in the server
                shouldSeed &= _sessionData.CurrentPlayers <=
                              _bindableSettings.Servers[_bindableSettings.CurrentServer].MinPlayers;

                throw new NotImplementedException("Need to make sure BF is not already running.");

                return shouldSeed;
            }
        }

        public bool ShouldKick
        {
            get 
            { 
                var shouldKick = true;

                shouldKick &= _sessionData.CurrentPlayers >
                              _bindableSettings.Servers[_bindableSettings.CurrentServer].MaxPlayers;

                throw new NotImplementedException("Need to make sure BF is running");

                return shouldKick;
            }
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
