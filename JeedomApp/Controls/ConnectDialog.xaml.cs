﻿using Jeedom;
using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Controls;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Media.SpeechRecognition;

//TODO: Gérer l'adresse sur le dns jeedom

namespace JeedomApp.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConnectDialog : UserControl
    {
        SpeechRecognizer recognizer;
        public ConnectDialog()
        {
            this.InitializeComponent();
        }
        private async void PivotConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PivotConnection.SelectedIndex == 1)
            {

            }
        }
        
        private void Demo_Click(object sender, RoutedEventArgs e)
        {
            // Charger des données de demo pour l'application

            // Masque le dialogue de connection
            ConnectDialog.HideConnectDialog();
        }
        private async void ShowError(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
            return;
        }

       private async void bConnect_Click(object sender, RoutedEventArgs e)
        {
            // Lance le rapatriement des données de Jeedom
            var taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());

            await taskFactory.StartNew(async () =>
            {
                // Connection à Jeedom
                var error = await RequestViewModel.Instance.ConnectJeedomByLogin();
                if (error != null)
                { ShowError(error.message); return; }

                // Création du mobile dans le plugin
                error = await RequestViewModel.Instance.CreateEqLogicMobile();
                if (error != null)
                { ShowError(error.message); return; }

                error = await RequestViewModel.Instance.SearchConfigByKey("jeedom::url", "core");
                if (error != null)
                { ShowError(error.message); return; }
                //RequestViewModel.config.HostExt = RequestViewModel.Instance.configByKey;

                await taskFactory.StartNew(async () =>
                {
                    await RequestViewModel.Instance.FirstLaunch();
                });

                // Masque le dialogue de connection
                ConnectDialog.HideConnectDialog();
            });
        }
        public static void ShowConnectDialog()
        {
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                var modal = Window.Current.Content as ModalDialog;
                var view = modal.ModalContent as ConnectDialog;
                if (view == null)
                    modal.ModalContent = view = new ConnectDialog();
                modal.IsModal = true;
                //view.Logo.Begin();
            });
        }
        public static void HideConnectDialog()
        {
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                var modal = Window.Current.Content as ModalDialog;
                if (modal != null)
                    modal.IsModal = false;
            });
        }

       
    }
}
