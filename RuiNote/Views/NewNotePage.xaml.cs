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

using RuiNote.Helpers;
using RuiNote.Services;
using RuiNote.ViewModel;
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
    public sealed partial class NewNotePage : Page
    {
        private string pageName = null;

        public NewNotePage()
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
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            pageName = Frame.CurrentSourcePageType.FullName;
            Messenger.Default.Send(new NotificationMessage("GetNoteListCount"));
            System.Diagnostics.Debug.WriteLine("pageName = " + pageName);

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Left newnotepage now...");
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {

            //Rui: 
            //Handle the Virtual Hardware Button: Back,
            //When user taps it, check the title text block and content text block, if one of them is not null,
            //it shall save it as vaild note. 
            e.Handled = true;
            if (pageName == "RuiNote.Views.NewNotePage")
            {
                System.Diagnostics.Debug.WriteLine("now we can save the page!");
                Messenger.Default.Send(new NotificationMessage("NewNotePageBackButtonPressed"));
            }


        }

    }
}
