﻿using Jeedom;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace JeedomApp.Controls
{
    [ContentProperty(Name = "InnerContent")]
    public sealed partial class TileControl : UserControl
    {
        #region Public Fields

        public static readonly DependencyProperty InnerContentProperty =
            DependencyProperty.Register("InnerContent", typeof(object), typeof(TileControl), new PropertyMetadata(null));

        public static readonly DependencyProperty EqLogicIdProperty =
            DependencyProperty.Register("EqLogicId", typeof(object), typeof(TileControl), new PropertyMetadata(null));

        public static readonly DependencyProperty SubTitleProperty =
            DependencyProperty.Register("SubTitle", typeof(string), typeof(TileControl), new PropertyMetadata(""));

        public static readonly DependencyProperty TileBrushProperty =
            DependencyProperty.Register("TileBrush", typeof(Brush), typeof(TileControl), new PropertyMetadata(null));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TileControl), new PropertyMetadata(""));

        public static readonly DependencyProperty UpdatingProperty =
            DependencyProperty.Register("Updating", typeof(bool), typeof(TileControl), new PropertyMetadata(false));

        #endregion Public Fields

        #region Public Constructors

        public TileControl()
        {
            this.InitializeComponent();
            //(this.Content as FrameworkElement).DataContext = this;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Le contenu de la tuile
        /// </summary>
        public object InnerContent
        {
            get { return (object)GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }

        /// <summary>
        /// Le sous-titre de la tuile
        /// </summary>
        public string SubTitle
        {
            get { return (string)GetValue(SubTitleProperty); }
            set { SetValue(SubTitleProperty, value); }
        }

        public string EqLogicId
        {
            get { return (string)GetValue(EqLogicIdProperty); }
            set { SetValue(EqLogicIdProperty, value); }
        }

        /// <summary>
        /// La couleur de fond de la tuile
        /// </summary>
        public Brush TileBrush
        {
            get { return (Brush)GetValue(TileBrushProperty); }
            set { SetValue(TileBrushProperty, value); }
        }

        /// <summary>
        /// Le titre de la tuile
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Si la tuile est en train d'être mise à jour
        /// </summary>
        public bool Updating
        {
            get { return (bool)GetValue(UpdatingProperty); }
            set { SetValue(UpdatingProperty, value); }
        }

        #endregion Public Properties

        private void RelativePanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var panel = sender as RelativePanel;
            TileFlyout.ShowAt(panel, e.GetPosition(panel));
        }

        private void AddToFavorite_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuFlyoutItem;
            string id = item.Tag as string;
            var lst = from eq in RequestViewModel.Instance.EqLogicList where eq.Id == id select eq;
            if (lst.Count() != 0)
            {
                var eq = lst.First();
                RequestViewModel.Instance.AddToFavorite(eq);
            }
        }
    }
}