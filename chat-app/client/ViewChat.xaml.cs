using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.VisualBasic;
using System.Threading;

namespace client
{
    /// <summary>
    /// Interaction logic for ViewChat.xaml
    /// </summary>
    public partial class ViewChat : Window
    {
        private BackgroundWorker m_bw = new BackgroundWorker();
        private string m_chatName;
        private int m_chatID;
        private int m_userID;
        private bool kill = false;
        public ViewChat(string chatName, int chatID, int userID)
        {
            m_chatID = chatID;
            m_userID = userID;
            m_chatName = chatName;
            InitializeComponent();
            txtChatName.Content = chatName;

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
            UpdateDetails();
        }

        private void UpdateDetails()
        {

        }

        private void m_bw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!kill)
            {
                m_bw.ReportProgress(0);
                Thread.Sleep(1000);
            }
            e.Cancel = true;
        }

        private void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            string users = Interaction.InputBox("Enter User(s) tag(s)", "Add User Dialog", "");
            foreach (string user in users.Split(','))
            {
                Communicator.Send("AddUser", new Dictionary<string, string> { { "Tag", user.Substring(user.IndexOf('#') + 1) }, { "ChatID", m_chatID.ToString() } }, Code.AddUserToChat);
                try
                {
                    if (Communicator.Recv()["Status"] != "1") MessageBox.Show($"Couldn't add {user}", "Error");
                }
                catch
                {
                    MessageBox.Show($"Couldn't add {user}", "Error");
                }
            }
        }

        private void btnChangeName_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRemoveUser_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
