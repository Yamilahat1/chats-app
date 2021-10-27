using System.Collections.Generic;
using System.Windows;

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
            var a = Communicator.Recv();
            string chatID = a["ChatID"];

            foreach (string user in txtParticipants.Text.Split(','))
            {
                Communicator.Send("AddUser", new Dictionary<string, string> { { "Tag", user.Substring(user.IndexOf('#') + 1) }, { "ChatID", chatID } }, Code.AddUserToChat);
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