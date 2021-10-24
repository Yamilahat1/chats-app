using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace client
{
    /// <summary>
    /// Interaction logic for ChatsWindow.xaml
    /// </summary>
    public partial class ChatsWindow : Window
    {
        private BackgroundWorker m_bw = new BackgroundWorker();
        private Queue<Byte> m_requests = new Queue<Byte>();

        private Dictionary<string, int> m_chats = new Dictionary<string, int> { }; // name-id

        private List<Chat> chats = new List<Chat>();
        private List<string> messages = new List<string>();

        private static int m_lastMsg = -1;
        private int m_offset = 0;
        private string m_username;
        private int m_id;
        private int m_currentChat;

        private bool kill = false;
        private bool isLoaded = false;
        public ChatsWindow(Window caller, string username, int id)
        {
            caller.Close();
            m_username = username;
            m_id = id;
            InitializeComponent();
            SetupChatsDict();
            txtInput.Text = "Type here...";
            lstChats.ItemsSource = chats;

            m_bw.WorkerSupportsCancellation = true;
            m_bw.WorkerReportsProgress = true;
            m_bw.DoWork += m_bw_DoWork;
            m_bw.ProgressChanged += m_bw_ProgressChanged;
            m_bw.RunWorkerCompleted += m_bw_RunWorkerCompleted;
            m_bw.RunWorkerAsync();
        }
        private void m_bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            m_bw.CancelAsync();
        }
        private void m_bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LoadChat();
        }
        private void m_bw_DoWork(object sender, DoWorkEventArgs e)
        {
            while(!kill)
            {
                if (!isLoaded || m_requests.Count > 0) continue;
                m_bw.ReportProgress(0);
                Thread.Sleep(1000);
            }
            e.Cancel = true;
        }
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\shutdown.wav";
            // player.PlaySync();

            kill = true;
            m_bw.CancelAsync();

            Communicator.Send("SignoutRequest", new Dictionary<string, string> { { "Username", m_username } }, Code.Signout);
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
            m_currentChat = m_chats[chats[lstChats.SelectedIndex].Name];
            lstMessages.Content = "";
            m_offset = 0;
            isLoaded = false;
            LoadChat();
        }
        private Message LoadMessage(int chatID)
        {
            m_requests.Enqueue(1);
            Communicator.Send("LoadMessage", new Dictionary<string, string> { { "ChatID", chatID.ToString() }, { "Offset", m_offset.ToString() } }, Code.LoadChat);
            var res = Communicator.Recv();
            m_requests.Dequeue();
            try
            {
                m_lastMsg = Convert.ToInt32(res["MessageID"]);
            }
            catch
            {
                return new Message();
            }
            m_offset++;
            return new Message() { Msg = string.Format("{0}: {1}", res["Sender"] == m_username ? "You" : res["Sender"], res["Content"]) };
        }
        private void LoadChat()
        {
            string content = lstMessages.Content.ToString();
            while (true)
            {
                int prevMsg = m_lastMsg;
                Message curr = LoadMessage(m_currentChat);
                if (prevMsg == m_lastMsg) break;
                messages.Add(curr.Msg);
                content += $"{curr.Msg}\n";
            }
            isLoaded = true;
            lstMessages.Content = content;
            if (!lstMessages.HasContent) lstMessages.Content = "System: Send a message to start chatting!";
        }
        public class Message
        {
            public string Msg { get; set; }
        }
        private void Top_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        // Textbox stuff
        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            Communicator.Send("SendMessage", new Dictionary<string, string> { { "UserID", m_id.ToString() }, { "ChatID", m_currentChat.ToString() }, { "Content", txtInput.Text } }, Code.SendMessage);
            var res = Communicator.Recv();
            if (Convert.ToInt32(res["Status"]) == 1) txtInput.Text = "Type here...";
        }
        private void txtInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text != "Type here...") return;
            txtInput.Text = "";
        }
        private void txtInput_LostFocus(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "Type here...";
        }
        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtInput.Text == "Type here...") txtInput.Text = "";
        }
    }
}