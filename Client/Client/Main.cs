using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Main : Form
    {
        private TcpClient client = null;
        private NetworkStream stream = null;
        private string username = null;

        public Main(string username, TcpClient client)
        {
            InitializeComponent();
            this.username = username;
            this.client = client;
            this.Text = username;
            stream = client.GetStream();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listViewUsers.View = View.List;
            listViewUsers.Columns.Add("users", listViewUsers.Width, HorizontalAlignment.Left);
            try
            {
                PrintMsg("Client Started");
                Thread t = new Thread(new ParameterizedThreadStart(ReceiveMessageListener));
                t.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ReceiveMessageListener(Object obj)
        {
            int i;
            byte[] inputBytes = new byte[1024];

            try
            {
                while ((i = stream.Read(inputBytes, 0, inputBytes.Length)) != 0)
                {
                    Chatroom.Message msg = Chatroom.SocketHelper.Deserialize(inputBytes) as Chatroom.Message;

                    switch (msg.Header)
                    {
                        case "MSG":
                            PrintMsg("From" + msg.Sender + ": " + msg.Data as string);
                            break;
                        case "UPDATEUSER":
                            InitializeListView(msg.Data as string[]);
                            break;
                        default:
                            PrintMsg(msg.Data as string);
                            break;
                    }
                    stream.Flush();
                    inputBytes = new byte[1024];
                }
            }
            catch (IOException ex)
            {
                PrintMsg("Server is shutted down!");
            }

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string recipient = null;
            try
            {
                recipient = listViewUsers.SelectedItems[0].Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select one recipient");
                return;
            }

            Chatroom.Message msg = new Chatroom.Message("MSG", textboxInput.Text);
            msg.Sender = username;
            msg.Recipient = recipient;

            byte[] inputStream = Chatroom.SocketHelper.Serialize(msg);
            stream.Write(inputStream, 0, inputStream.Length);
            stream.Flush();

            PrintMsg(username + ":" + textboxInput.Text);
            textboxInput.Text = string.Empty;
        }

        private void PrintMsg(string msg)
        {
            if (textboxOutput.InvokeRequired)
            {
                Action<string> action = (txt) => { textboxOutput.Text = textboxOutput.Text + Environment.NewLine + " >> " + txt; };
                textboxOutput.Invoke(action, msg);
            }
            else
            {
                textboxOutput.Text = textboxOutput.Text + Environment.NewLine + " >> " + msg;
            }
        }

        private bool InitializeListView(string[] userList)
        {
            if (listViewUsers.InvokeRequired)
            {
                Action<string[]> action = (list) =>
                {
                    listViewUsers.BeginUpdate();
                    listViewUsers.Clear();
                    ListViewItem item;
                    foreach (string user in list)
                    {
                        item = new ListViewItem();
                        item.Text = user;
                        listViewUsers.Items.Add(item);
                    }
                    listViewUsers.EndUpdate();

                };
                listViewUsers.Invoke(action, new object[] { userList });
            }
            else
            {
                listViewUsers.BeginUpdate();
                listViewUsers.Clear();
                foreach (string user in userList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = user;
                    listViewUsers.Items.Add(item);
                }
                listViewUsers.EndUpdate();
            }

            return true;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            //client.Close();
            //stream.Close();
            Environment.Exit(0);
        }
    }
}
