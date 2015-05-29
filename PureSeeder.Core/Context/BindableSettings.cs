﻿using System;
using System.ComponentModel;
using Newtonsoft.Json;
using PureSeeder.Core.Settings;

namespace PureSeeder.Core.Context
{
    public class BindableSettings : BindableBase
    {
        private readonly SeederUserSettings _settings;

        public BindableSettings(SeederUserSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            //settings.Reset();
            //settings.Reload();
            //settings.Upgrade();
            //settings.Save();
            _settings = settings;
            DirtySettings = false;
            this.PropertyChanged += SettingChanged;
            TimesCollection.TimesCollectionChanged += SettingChanged;
        }

        // Note: This is to allow Deserializing to this class
        protected BindableSettings() : this(new SeederUserSettings())
        {
        }

        private void SettingChanged(object sender, EventArgs e)
        {
            var propertyChangedEventArgs = e as PropertyChangedEventArgs;
            if (propertyChangedEventArgs != null)
            {
                if (propertyChangedEventArgs.PropertyName == "DirtySettings")
                    return;
            }

            //_settings.Save();
            DirtySettings = true;
        }

        public bool SaveSettings()
        {
            _settings.Save();
            DirtySettings = false;
            return true;
        }

        private bool _dirtySettings;
        [JsonIgnore]
        public bool DirtySettings
        {
            get { return _dirtySettings; }
            set { SetField(ref _dirtySettings, value); }
        }

        [JsonIgnore]
        public string Username
        {
            get { return _settings.Username; }
            set { SetProperty(_settings, value, x => x.Username); }
        }

        public bool EnableLogging
        {
            get { return _settings.EnableLogging; }
            set { SetProperty(_settings, value, x => x.EnableLogging); }
        }

        public TimesCollection TimesCollection
        {
            get { return _settings.TimesCollection; }
            set { SetProperty(_settings, value, x => x.TimesCollection); }
        }

        // Deprecated
//        [JsonIgnore]
//        public int CurrentServer
//        {
//            get { return _settings.CurrentServer; }
//            set { SetProperty(_settings, value, x => x.CurrentServer); }
//        }

        public int RefreshInterval
        {
            get { return _settings.RefreshInterval; }
            set { SetProperty(_settings, value, x => x.RefreshInterval); }
        }

        public int StatusRefreshInterval
        {
            get { return _settings.StatusRefreshInterval; }
            set { SetProperty(_settings, value, x => x.StatusRefreshInterval); }
        }

        [JsonIgnore]
        public bool MinimizeToTray
        {
            get { return _settings.MinimizeToTray; }
            set { SetProperty(_settings, value, x => x.MinimizeToTray); }
        }

        public bool AutoLogin
        {
            get { return _settings.AutoLogin; }
            set { SetProperty(_settings, value, x => x.AutoLogin); }
        }

        public int IdleKickAvoidanceTimer
        {
            get { return _settings.IdleKickAvoidanceTimer; }
            set { SetProperty(_settings, value, x => x.IdleKickAvoidanceTimer); }
        }

        public int ReadyUpperTimer
        {
            get { return _settings.ReadyUpperTimer; }
            set { SetProperty(_settings, value, x => x.ReadyUpperTimer); }
        }

        [JsonIgnore]
        public string Password
        {
            get { return _settings.Password; }
            set { SetProperty(_settings, value, x => x.Password); }
        }

        [JsonIgnore]
        public string Email
        {
            get { return _settings.Email; }
            set { SetProperty(_settings, value, x => x.Email); }
        }

        [JsonIgnore]
        public bool AutoMinimizeSeeder
        {
            get { return _settings.AutoMinimizeSeeder; }
            set { SetProperty(_settings, value, x => x.AutoMinimizeSeeder); }
        }

        [JsonIgnore]
        public bool AutoMinimizeGame
        {
            get { return _settings.AutoMinimizeGame; }
            set { SetProperty(_settings, value, x => x.AutoMinimizeGame); }
        }
    }
}