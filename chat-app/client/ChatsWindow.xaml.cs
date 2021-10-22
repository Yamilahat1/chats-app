using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Collections;

namespace client
{
    /// <summary>
    /// Interaction logic for ChatsWindow.xaml
    /// </summary>
    public partial class ChatsWindow : Window
    {
        private Dictionary<string, int> m_chats = new Dictionary<string, int> { }; // name-id

        private List<Chat> chats = new List<Chat>();
        private List<string> messages = new List<string>();

        private static int m_lastMsg = -1;
        private int m_offset = 0;
        private string m_username;
        private int m_id;
        public ChatsWindow(Window caller, string username, int id)
        {
            caller.Close();
            m_username = username;
            m_id = id;
            InitializeComponent();
            SetupChatsDict();
            lstChats.ItemsSource = chats;
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
        private void SetupChatsDict()
        {
            Communicator.Send("GetAllChats", new Dictionary<string, string> { { "UserID", m_id.ToString() } }, Code.GetAllChats);
            try
            {
                string[] res = Communicator.Recv()["Chats"].Split(',');
                res = res.Take(res.Length - 1).ToArray();
                foreach (var chat in res) m_chats.Add(chat.Split('-')[0], int.Parse(chat.Split('-')[1]));
                foreach (var chat in m_chats) chats.Add(new Chat() { Name = chat.Key });
            }
            catch (Exception) { }
        }
        public class Chat
        {
            public string Name { get; set; }
        }
        private void lstChats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int chatID = m_chats[chats[lstChats.SelectedIndex].Name];
            LoadChat(chatID);
        }
        private Message LoadMessage(int chatID)
        {
            Communicator.Send("LoadMessage", new Dictionary<string, string> { { "ChatID", chatID.ToString() }, { "Offset", m_offset.ToString() } }, Code.LoadChat);
            var res = Communicator.Recv();
            try
            {
                m_lastMsg = Convert.ToInt32(res["MessageID"]);
            }
            catch
            {
                return new Message();
            }
            m_offset++;
            return new Message() { Msg = string.Format("{0}: {1}", res["Sender"], res["Content"]) };
        }
        private void LoadChat(int chatID)
        {
            for (int i = 0; i < 20; i++)
            {
                int prevMsg = m_lastMsg;
                Message curr = LoadMessage(chatID);
                if (prevMsg == m_lastMsg) break;
                messages.Add(curr.Msg);
                lstMessages.Items.Add(curr.Msg);
            }
        }
        public class Message
        {
            public string Msg { get; set; }
        }
    }
}