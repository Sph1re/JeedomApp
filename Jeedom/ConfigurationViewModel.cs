using Jeedom.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace Jeedom
{
    public class ConfigurationViewModel
    {
        public bool Populated = false;
        private const string settingAPIKey = "apikeySetting";
        private const string settingConnexionAuto = "ConnexionAutoSetting";
        private const string settingFavoriteList = "FavoriteListSetting";
        private const string settingHost = "addressSetting";
        private const string settingHostExt = "addressExtSetting";
        private const string settingIdMobile = "IdMobileSetting";
        private const string settingIdPush = "IdPushSetting";
        private const string settingLogin = "LoginSetting";
        private const string settingPassword = "PasswordSetting";
        private const string settingTwoFactor = "TwoFactorSetting";
        private Address _address = new Address();
        private string _apikey;
        private bool? _connexionAuto;
        private List<string> _favoriteList;
        private bool _GeoFenceActivation;
        private string _GeoFenceActivationDistance;
        private bool _GeolocActivation;
        private string _GeolocObjectId;
        private string _hostExt;
        private string _idMobile;
        private string _idPush;
        private string _login;
        private bool _NotificationActivation;
        private string _NotificationObjectId;
        private string _password;
        private bool? _twoFactor;
        private string _twoFactorCode;
        private bool _useExtHost = false;
        private ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        private ApplicationDataContainer RoamingSettings = ApplicationData.Current.RoamingSettings;

        public ConfigurationViewModel()
        {
            Address.Link = RoamingSettings.Values[settingHost] as string;
            _login = RoamingSettings.Values[settingLogin] as string;
            _password = RoamingSettings.Values[settingPassword] as string;

            if (RoamingSettings.Values[settingConnexionAuto] != null)
                _connexionAuto = Convert.ToBoolean(LocalSettings.Values[settingConnexionAuto]);
            _apikey = RoamingSettings.Values[settingAPIKey] as string;

            //Populated si API Key et host disponible
            TestPopulated();

            _idMobile = LocalSettings.Values[settingIdMobile] as string;
            _idPush = LocalSettings.Values[settingIdPush] as string;

            _GeolocActivation = (LocalSettings.Values["GeolocActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["GeolocActivation"]);
            _GeoFenceActivation = (LocalSettings.Values["GeoFenceActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["GeoFenceActivation"]);
            _NotificationActivation = (LocalSettings.Values["NotificationActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["NotificationActivation"]);

            _GeolocObjectId = (LocalSettings.Values["GeolocObjectId"] == null) ? "" : LocalSettings.Values["GeolocObjectId"].ToString();
            _NotificationObjectId = (LocalSettings.Values["NotificationObjectId"] == null) ? "" : LocalSettings.Values["NotificationObjectId"].ToString();
            _GeoFenceActivationDistance = (LocalSettings.Values["GeoFenceActivationDistance"] == null) ? "" : LocalSettings.Values["GeoFenceActivationDistance"].ToString();

            //TODO: convertir list<string> to json avant de stocker en roaming
            _favoriteList = RoamingSettings.Values[settingFavoriteList] as List<string>;
            if (_favoriteList == null)
                FavoriteList = new List<string>();
        }

        public string ApiKey
        {
            set
            {
                if (value != null)
                {
                    _apikey = value;
                    TestPopulated();
                    RoamingSettings.Values[settingAPIKey] = value;
                }
            }
            get
            {
                return _apikey;
            }
        }

        public bool? ConnexionAuto
        {
            get
            {
                return _connexionAuto;
            }

            set
            {
                _connexionAuto = value;
                LocalSettings.Values[settingConnexionAuto] = value;
            }
        }

        public List<string> FavoriteList
        {
            get { return _favoriteList; }
            set
            {
                _favoriteList = value;
                //TODO: décommenter si conversion en json
                //RoamingSettings.Values[settingFavoriteList] = value;
            }
        }

        public bool GeoFenceActivation
        {
            set
            {
                _GeoFenceActivation = value;
                LocalSettings.Values["GeoFenceActivation"] = value;
            }

            get
            {
                return _GeoFenceActivation;
            }
        }

        public string GeoFenceActivationDistance
        {
            set
            {
                _GeoFenceActivationDistance = value;
                LocalSettings.Values["GeoFenceActivationDistance"] = value;
            }

            get
            {
                return _GeoFenceActivationDistance;
            }
        }

        public bool GeolocActivation
        {
            set
            {
                _GeolocActivation = value;
                LocalSettings.Values["GeolocActivation"] = value;
            }

            get
            {
                return _GeolocActivation;
            }
        }

        public string GeolocObjectId
        {
            set
            {
                _GeolocObjectId = value;
                LocalSettings.Values["GeolocObjectId"] = value;
            }

            get
            {
                return _GeolocObjectId;
            }
        }

        public string Host
        {
            set
            {
                Address.Link = value;
                TestPopulated();
                RoamingSettings.Values[settingHost] = Address.Link;
            }

            get
            {
                return Address.Link;
            }
        }

        public string HostExt
        {
            set
            {
                _hostExt = value;
                //_hostExt = _hostExt.Replace("http://", "");
                //_hostExt = _hostExt.Replace("https://", "");
                RoamingSettings.Values[settingHostExt] = _hostExt;
            }

            get
            {
                return _hostExt;
            }
        }

        public string IdMobile
        {
            set
            {
                if (value != null)
                {
                    _idMobile = value;
                    LocalSettings.Values[settingIdMobile] = value;
                }
            }
            get
            {
                return _idMobile;
            }
        }

        public string IdPush
        {
            set
            {
                if (value != null)
                {
                    _idPush = value;
                    LocalSettings.Values[settingIdPush] = value;
                }
            }
            get
            {
                return _idPush;
            }
        }

        public string Login
        {
            get
            {
                return _login;
            }

            set
            {
                _login = value;
                RoamingSettings.Values[settingLogin] = value;
            }
        }

        public bool NotificationActivation
        {
            set
            {
                _NotificationActivation = value;
                LocalSettings.Values["NotificationActivation"] = value;
            }

            get
            {
                return _NotificationActivation;
            }
        }

        public string NotificationObjectId
        {
            set
            {
                _NotificationObjectId = value;
                LocalSettings.Values["NotificationObjectId"] = value;
            }

            get
            {
                return _NotificationObjectId;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                byte[] bytes = Encoding.GetEncoding(0).GetBytes(value);
                _password = Encoding.UTF8.GetString(bytes);
                RoamingSettings.Values[settingPassword] = _password;
            }
        }

        public bool? TwoFactor
        {
            get
            {
                return _twoFactor;
            }

            set
            {
                _twoFactor = value;
            }
        }

        public string TwoFactorCode
        {
            get
            {
                return _twoFactorCode;
            }

            set
            {
                _twoFactorCode = value;
            }
        }

        /// <summary>
        /// URI d'accès au serveur JEEDOM
        /// </summary>
        public Uri Uri
        {
            get
            {
                return Address.Uri;
            }
        }

        public bool UseExtHost
        {
            get
            {
                return _useExtHost;
            }

            set
            {
                _useExtHost = value;
            }
        }

        internal Address Address { get { return _address; } set { _address = value; } }

        /// <summary>
        /// Supprime tous les paramètres
        /// </summary>
        public void Reset()
        {
            this.ApiKey = "";
            this.ConnexionAuto = true;
            this.GeoFenceActivation = false;
            this.GeoFenceActivationDistance = "";
            this.GeolocActivation = false;
            this.GeolocObjectId = "";
            this.Host = "";
            this.Login = "";
            this.NotificationActivation = false;
            this.NotificationObjectId = "";
            this.Password = "";
            this.TwoFactor = false;
            this.TwoFactorCode = "";
            this.FavoriteList = new List<string>();
        }

        /// <summary>
        /// Vérifie que la configuration est entièrement peuplée (api key et host disponible)
        /// </summary>
        private void TestPopulated()
        {
            if (String.IsNullOrWhiteSpace(Address.Link) || String.IsNullOrWhiteSpace(_apikey))
                Populated = false;
            else
                Populated = true;
        }
    }
}