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
using Wurklist.General;
using Wurklist.login;

namespace Wurklist
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        private login.Login _login;
        private Kanban.KanbanBoard kanban = new Kanban.KanbanBoard();
        private int UserId;

        public MainPage()
        {
            this.InitializeComponent();

            _login = new login.Login();
        }

        // Handles the Click event on the Button inside the Popup control and 
        // closes the Popup. 
        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }

        // Handles the Click event on the Button on the page and opens the Popup. 
        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "Password")
            {
                statusText.Text = "'Password' is not allowed as a password.";
            }
            else
            {
                statusText.Text = string.Empty;
            }
        }

        private async void button_LoginButtonClicked(object sender, RoutedEventArgs e)
        {
            User user = new User(Username.Text, Password.Password);

            UserId = await _login.TryLoginAsync(user);
            kanban.SetUserId(UserId);
            kanban.GetAllProjectTasksFromUser();

            Textblock.Text = "Logged in as : \n ID : " + UserId.ToString() + " \n Username : " + user.Name;
        }

        private void button_RegisterButtonClicked(object sender, RoutedEventArgs e)
        {
            User user = new User(RegisterUsername.Text, RegisterPassword.Password, RegisterEmail.Text, RegisterDateOfBirth.Text);

            _login.Register(user);
        }

        private async void GotoKanbanBoard(object sender, RoutedEventArgs e)
        {
            if(User.GetUserId() != 0)
            {
                Frame.Navigate(typeof(Kanban.KanbanBoard), kanban);
            }
            else
            {
                ContentDialog warningMessage = new ContentDialog
                {
                    Title = "You must be logged in to access the kanban board.",
                    CloseButtonText = "Ok"
                };

                ContentDialogResult warningMessageResult = await warningMessage.ShowAsync();
            }
        }
    }
}
