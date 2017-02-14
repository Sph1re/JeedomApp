using System;
using System.Text;
using Windows.Networking.Connectivity;
using Windows.Storage;

namespace Jeedom
{
    public class ConfigurationViewModel
    {
        private UriBuilder _uri;
        private string _host = "http://";
        private string _hostExt;
        private string _login;
        private string _password;
        private bool? _twoFactor;
        private bool? _connexionAuto;
        private bool _useExtHost = false;
        private string _twoFactorCode;
        private string _apikey;
        private string _idMobile;

        /* public static bool HasInternetConnection()
         {
             bool hasInternet = false;
             ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
             if (profile != null) hasInternet = profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
             return hasInternet;
         }*/

        public Uri Uri
        {
            get
            {
                if (_useExtHost == false)
                {
                    if (_host == "")
                        return null;
                    _uri = new UriBuilder(_host);
                }
                else
                {
                    if (_hostExt == "")
                        return null;
                    _uri = new UriBuilder(_hostExt);
                }
                // Termine l'URL par un "/" pour Docker par exemple dont le chemin est de la forme "http://xxx.xxx.xxx.xxx:xxxx/jeedom"
                if (!_uri.Path.EndsWith("/"))
                    _uri.Path += "/";

                return _uri.Uri;
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

        public string Host
        {
            set
            {
                _host = value;
                //_host = _host.Replace("http://","");
                // _host = _host.Replace("https://", "");
                RoamingSettings.Values[settingHost] = _host;
            }

            get
            {
                return _host;
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

        public string ApiKey
        {
            set
            {
                if (value != null)
                {
                    _apikey = value;
                    RoamingSettings.Values[settingAPIKey] = value;
                }
            }
            get
            {
                return _apikey;
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

        public bool Populated = false;
        private bool _GeolocActivation;

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

        private bool _GeoFenceActivation;

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

        private string _GeoFenceActivationDistance;

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

        private bool _NotificationActivation;

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

        private string _GeolocObjectId;

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

        private string _NotificationObjectId;

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

        private ApplicationDataContainer RoamingSettings = ApplicationData.Current.RoamingSettings;
        private ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        private const string settingHost = "addressSetting";
        private const string settingHostExt = "addressExtSetting";
        private const string settingLogin = "LoginSetting";
        private const string settingPassword = "PasswordSetting";
        private const string settingTwoFactor = "TwoFactorSetting";
        private const string settingConnexionAuto = "ConnexionAutoSetting";
        private const string settingAPIKey = "apikeySetting";
        private const string settingIdMobile = "IdMobileSetting";

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
            this.HostExt = "";
            this.Login = "";
            this.NotificationActivation = false;
            this.NotificationObjectId = "";
            this.Password = "";
            this.TwoFactor = false;
            this.TwoFactorCode = "";
            this.UseExtHost = false;
        }

        public ConfigurationViewModel()
        {
            Populated = true;

            _host = RoamingSettings.Values[settingHost] as string;
            if (_host == null)
                Populated = false;
            _hostExt = RoamingSettings.Values[settingHostExt] as string;
            if (_hostExt == null)
                Populated = false;

            _login = RoamingSettings.Values[settingLogin] as string;
            if (_login == null)
                Populated = false;
            _password = RoamingSettings.Values[settingPassword] as string;
            if (_password == null)
                Populated = false;

            if (RoamingSettings.Values[settingConnexionAuto] != null)
                _connexionAuto = Convert.ToBoolean(RoamingSettings.Values[settingConnexionAuto]);
            _apikey = RoamingSettings.Values[settingAPIKey] as string;
            if (_apikey == null)
                Populated = false;

            _idMobile = LocalSettings.Values[settingIdMobile] as string;
            if (_idMobile == null)
                Populated = false;

            _GeolocActivation = (LocalSettings.Values["GeolocActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["GeolocActivation"]);
            _GeoFenceActivation = (LocalSettings.Values["GeoFenceActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["GeoFenceActivation"]);
            _NotificationActivation = (LocalSettings.Values["NotificationActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["NotificationActivation"]);

            _GeolocObjectId = (LocalSettings.Values["GeolocObjectId"] == null) ? "" : LocalSettings.Values["GeolocObjectId"].ToString();
            _NotificationObjectId = (LocalSettings.Values["NotificationObjectId"] == null) ? "" : LocalSettings.Values["NotificationObjectId"].ToString();
            _GeoFenceActivationDistance = (LocalSettings.Values["GeoFenceActivationDistance"] == null) ? "" : LocalSettings.Values["GeoFenceActivationDistance"].ToString();
        }
    }
}