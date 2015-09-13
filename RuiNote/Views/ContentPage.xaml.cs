using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using RuiNote.Common;

using RuiNote.Helpers;
using RuiNote.Services;
using RuiNote.Services.Interfaces;
using Cimbalino.Toolkit.Services;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace RuiNote.Views
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentPage : Page
    {

        public ContentPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            Messenger.Default.Send(new NotificationMessage("UpdateNoteList"));

            var navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
            navigationService.RemoveAllBackStack();
            base.OnNavigatedTo(e);
        }

        private void ContentPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (contentPivot.SelectedIndex == 2)
            {
                Messenger.Default.Send(new NotificationMessage("AppBarButtonNewNotebookVisiable"));
            }

            else
            {
                Messenger.Default.Send(new NotificationMessage("AppBarButtonNewNoteVisiable"));
            }
        }

        private void Border_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
    }
}
