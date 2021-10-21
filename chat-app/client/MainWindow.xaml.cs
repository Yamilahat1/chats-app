using System;
using System.Xaml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            
            Communicator.Send("LoginRequest", new Dictionary<string, string> { { "Username", username }, { "Password", password } }, Code.Login);
            var res = Communicator.Recv();
            foreach (KeyValuePair<string, string> kv in res)
                MessageBox.Show(kv.Value.ToString());
        }
    }
}
