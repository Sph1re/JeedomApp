﻿using Jeedom.Model;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JeedomApp.Selectors
{
    internal class EqLogicTemplateSelector : DataTemplateSelector
    {
        #region Public Properties

        public DataTemplate DefaultEqLogicTemplate { get; set; }
        public DataTemplate OnOffEqLogicTemplate { get; set; }
        public DataTemplate OnOffSliderEqLogicTemplate { get; set; }
        public DataTemplate SonosEqLogicTemplate { get; set; }
        public DataTemplate ForecastIoEqLogicTemplate { get; set; }
        public DataTemplate DarkSkyEqLogicTemplate { get; set; }
        public DataTemplate ZWaveEqLogicTemplate { get; set; }
        public DataTemplate CameraEqLogicTemplate { get; set; }
        public DataTemplate TemperatureEqLogicTemplate { get; set; }
        public DataTemplate HumidityEqLogicTemplate { get; set; }
        public DataTemplate TempHumEqLogicTemplate { get; set; }

        #endregion Public Properties

        #region Protected Methods

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var eq = item as EqLogic;

            // Cherche par rapport au plugin
           // System.Diagnostics.Debug.WriteLine(eq.EqTypeName);
            switch (eq.EqTypeName)
            {
                case "sonos3":
                    container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 4);
                    container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 4);
                    return SonosEqLogicTemplate;

                case "darksky":
                case "forecastio":
                    container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 4);
                    container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 4);
                    return DarkSkyEqLogicTemplate;

                case "camera":
                    container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 2);
                    container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 4);
                    return CameraEqLogicTemplate;
            }

            // Lumière On Off Slider
            if (ContainCmd(eq, new[] { "LIGHT_STATE", "LIGHT_ON", "LIGHT_OFF", "LIGHT_SLIDER" }))
            {
                container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 2);
                container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 4);
                return OnOffSliderEqLogicTemplate;
            }

            // Lumière OnOff
            if (ContainCmd(eq, new[] { "LIGHT_STATE", "LIGHT_ON", "LIGHT_OFF" }))
            {
                container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 2);
                container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 2);
                return OnOffEqLogicTemplate;
            }

            // Lumière Toggle
            if (ContainCmd(eq, new[] { "LIGHT_STATE", "LIGHT_TOGGLE" }))
            {
                container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 2);
                container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 2);
                return OnOffEqLogicTemplate;
            }

            // Temperature & Humidité
            if (ContainCmd(eq, new[] { "TEMPERATURE", "HUMIDITY" }))
            {
                container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 2);
                container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 2);
                return TempHumEqLogicTemplate;
            }

            // Temperature
            if (ContainCmd(eq, new[] { "TEMPERATURE" }))
            {
                container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 2);
                container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 2);
                return TemperatureEqLogicTemplate;
            }

            // Humidity
            if (ContainCmd(eq, new[] { "HUMIDITY" }))
            {
                container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 2);
                container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 2);
                return HumidityEqLogicTemplate;
            }

            return DefaultEqLogicTemplate;
        }

        /// <summary>
        /// Renvoie vrai si l'équipement a les commandes recherchées
        /// </summary>
        /// <param name="eq">L'équipement Jeedom</param>
        /// <param name="types">Les generic_type recherchés</param>
        /// <returns></returns>
        private static bool ContainCmd(EqLogic eq, string[] types)
        {
            //Pour éviter de parcourir toutes les cmds
            if (eq.Cmds.Count() != types.Count())
                return false;

            int _find = 0;
            foreach (var type in types)
            {
                if (eq.Cmds != null)
                {
                    var search = eq.Cmds.Where(c => c.Display.generic_type == type);
                    if (search.Count() > 0)
                        _find += 1;
                }
            }
            return _find == types.Count();
        }

        #endregion Protected Methods
    }
}
