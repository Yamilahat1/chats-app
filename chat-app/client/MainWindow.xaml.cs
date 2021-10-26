using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Communicator.Connect(true); // Change to false later
            InitializeComponent();
        }

        private void Top_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            Communicator.Send("LoginRequest", new Dictionary<string, string> { { "Username", username }, { "Password", password } }, Code.Login);
            var res = Communicator.Recv();
            if (!res.ContainsKey("Status"))
            {
                status.Content = "Login failed.";
                status.Visibility = Visibility.Visible;
            }
            else
            {
                ChatsWindow win = new ChatsWindow(this, username, Convert.ToInt32(res["id"]));
                win.ShowDialog();
            }
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            Communicator.Send("SignupRequest", new Dictionary<string, string> { { "Username", username }, { "Password", password } }, Code.Signup);
            var res = Communicator.Recv();

            if (!res.ContainsKey("Status"))
            {
                status.Content = "Signup failed.";
                status.Visibility = Visibility.Visible;
            }
            else btnLogin_Click(sender, e);
        }

        private void box_GotFocus(object sender, RoutedEventArgs e)
        {
            status.Visibility = Visibility.Hidden;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}