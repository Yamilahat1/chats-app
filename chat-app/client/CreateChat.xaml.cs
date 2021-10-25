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

namespace client
{
    /// <summary>
    /// Interaction logic for CreateChat.xaml
    /// </summary>
    public partial class CreateChat : Window
    {
        private int m_userID;
        private ChatsWindow m_prevWindow;
        public CreateChat(ChatsWindow caller, int userID)
        {
            InitializeComponent();
            m_userID = userID;
            m_prevWindow = caller;
        }
        private void btnCreateChat_Click(object sender, RoutedEventArgs e)
        {
            bool isOK = true;
            Communicator.Send("CreateChat", new Dictionary<string, string> { { "ChatName", txtChatName.Text }, { "AdminID", m_userID.ToString() } }, Code.CreateChat);
            string chatID = Communicator.Recv()["ChatID"];

            foreach (string user in txtParticipants.Text.Split(','))
            {
                Communicator.Send("AddUser", new Dictionary<string, string> { { "Nickname", user }, { "ChatID", chatID } }, Code.AddUserToChat);
                try
                {
                    if (Communicator.Recv()["Status"] != "1") DisplayError($"Couldn't add {user}", false);
                }
                catch
                {
                    DisplayError($"Couldn't add {user}", false);
                }
            }
            if (isOK) this.Close();
        }
        private void DisplayError(string error, bool toClear)
        {
            if (toClear) txtError.Content = "";
            txtError.Content += $"\n{error}";
            txtError.Visibility = Visibility.Visible;
        }
    }
}
