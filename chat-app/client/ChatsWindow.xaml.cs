using System;
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
using System.Windows.Shapes;
using System.Media;
using System.Threading;

namespace client
{
    /// <summary>
    /// Interaction logic for ChatsWindow.xaml
    /// </summary>
    public partial class ChatsWindow : Window
    {
        private string m_username;
        public ChatsWindow(Window caller, string username)
        {
            caller.Close();
            m_username = username;
            InitializeComponent();
        }
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\shutdown.wav";
            player.PlaySync();
            Communicator.Send("SignoutRequest", new Dictionary<string, string> { { "Username", m_username } }, Code.Signout);
            Communicator.Recv();
            this.Close();
        }
    }
}
