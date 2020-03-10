using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Login : Form
    {
        private TcpClient client;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtServerAddress.Text == string.Empty
                || txtPort.Text == string.Empty
                || txtEnterUsername.Text == string.Empty)
            {
                MessageBox.Show("Each textboxes cannot be empty!");
                return;
            }

            string ipAddress = txtServerAddress.Text;
            int port = Convert.ToInt32(txtPort.Text);
            try
            {
                client = new TcpClient(ipAddress, port);
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Cannot connect server " + ipAddress + ":" + port);
                return;
            }

            string username = txtEnterUsername.Text;
            if (LoginByNewUsername(username))
            {
                Main mainForm = new Main(username, client);
                this.Hide();
                mainForm.ShowDialog();
                Application.ExitThread();
            }
            else
            {
                MessageBox.Show("Username has already existed!");
            }
        }

        private bool LoginByNewUsername(string username)
        {
            NetworkStream stream = client.GetStream();
            Chatroom.Message msg = new Chatroom.Message("CREATEUSER", username);
            Byte[] data = Chatroom.SocketHelper.Serialize(msg);

            stream.Write(data, 0, data.Length);
            stream.Flush();
            Console.WriteLine("CREATEUSER:{0}", username);

            data = new Byte[1];
            String responseCode = String.Empty;
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseCode = Encoding.ASCII.GetString(data);

            if (responseCode == "1")
            {
                return true;
            }
            return false;
        }
    }
}
