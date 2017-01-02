﻿using Jeedom.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace JeedomApp.Controls
{
    public sealed partial class OnOffPluginControl : UserControl
    {
        #region Public Fields

        public static readonly DependencyProperty OffBrushProperty =
            DependencyProperty.Register("OffBrush", typeof(Brush), typeof(OnOffPluginControl), new PropertyMetadata(XamlBindingHelper.ConvertValue(typeof(Brush), "#FF434A54")));

        public static readonly DependencyProperty OffIconProperty =
            DependencyProperty.Register("OffIcon", typeof(string), typeof(OnOffPluginControl), new PropertyMetadata("\uf00d"));

        public static readonly DependencyProperty OffProperty =
            DependencyProperty.Register("Off", typeof(Command), typeof(OnOffPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty OnBrushProperty =
            DependencyProperty.Register("OnBrush", typeof(Brush), typeof(OnOffPluginControl), new PropertyMetadata(XamlBindingHelper.ConvertValue(typeof(Brush), "#FF8CC152")));

        public static readonly DependencyProperty OnIconProperty =
                    DependencyProperty.Register("OnIcon", typeof(string), typeof(OnOffPluginControl), new PropertyMetadata("\uf00c"));

        public static readonly DependencyProperty OnProperty =
            DependencyProperty.Register("On", typeof(Command), typeof(OnOffPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty StateBoolProperty =
            DependencyProperty.Register("StateBool", typeof(bool), typeof(OnOffPluginControl), new PropertyMetadata(false));

        public static readonly DependencyProperty StateBrushProperty =
                    DependencyProperty.Register("StateBrush", typeof(Brush), typeof(OnOffPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty StateIconProperty =
            DependencyProperty.Register("StateIcon", typeof(string), typeof(OnOffPluginControl), new PropertyMetadata("\uf00d"));

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(Command), typeof(OnOffPluginControl), new PropertyMetadata(null));

        #endregion Public Fields

        #region Private Fields

        private bool _DataContextLoaded = false;

        #endregion Private Fields

        #region Public Constructors

        public OnOffPluginControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += OnOffPluginControl_DataContextChanged;
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public Command Off { get { return (Command)GetValue(OffProperty); } set { SetValue(OffProperty, value); } }
        public Brush OffBrush { get { return (Brush)GetValue(OffBrushProperty); } set { SetValue(OffBrushProperty, value); } }
        public string OffIcon { get { return (string)GetValue(OffIconProperty); } set { SetValue(OffIconProperty, value); } }
        public Command On { get { return (Command)GetValue(OnProperty); } set { SetValue(OnProperty, value); } }
        public Brush OnBrush { get { return (Brush)GetValue(OnBrushProperty); } set { SetValue(OnBrushProperty, value); } }
        public string OnIcon { get { return (string)GetValue(OnIconProperty); } set { SetValue(OnIconProperty, value); } }
        public Command State { get { return (Command)GetValue(StateProperty); } set { SetValue(StateProperty, value); } }
        public bool StateBool { get { return (bool)GetValue(StateBoolProperty); } set { SetValue(StateBoolProperty, value); } }
        public Brush StateBrush { get { return (Brush)GetValue(StateBrushProperty); } set { SetValue(StateBrushProperty, value); } }
        public string StateIcon { get { return (string)GetValue(StateIconProperty); } set { SetValue(StateIconProperty, value); } }

        #endregion Public Properties

        #region Public Methods

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Cmds_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateUI();
        }

        private void OnOffPluginControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            // Pas besoin de recharger
            if (_DataContextLoaded)
                return;

            if (this.DataContext == null)
                return;
            if (!(this.DataContext.GetType() == typeof(EqLogic)))
                return;

            var eq = (EqLogic)this.DataContext;

            // On s'abonne aux changement des commandes
            eq.Cmds.CollectionChanged += Cmds_CollectionChanged; ;

            foreach (var cmd in eq.Cmds)
            {
                switch (cmd.Display.generic_type)
                {
                    case "LIGHT_STATE":
                        State = cmd;
                        break;

                    case "LIGHT_ON":
                        On = cmd;
                        break;

                    case "LIGHT_OFF":
                        Off = cmd;
                        break;

                    default:
                        break;
                }
            }

            _DataContextLoaded = true;
            UpdateUI();
        }

        /// <summary>
        /// Mise à jour de l'interface
        /// </summary>
        private void UpdateUI()
        {
            if (State == null)
                return;

            if (State.Value == "1" || State.Value.ToLower() == "on")
            {
                StateBool = true;
                StateBrush = OnBrush;
                StateIcon = OnIcon;
            }
            else
            {
                StateBool = false;
                StateBrush = OffBrush;
                StateIcon = OffIcon;
            }
        }

        #endregion Private Methods
    }
}