using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        private TcpListener server = null;
        private Hashtable clientsTable = null;

        public Server(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            clientsTable = new Hashtable();
            server.Start();
            StartListener();

            AppDomain.CurrentDomain.ProcessExit += delegate
            {
                server.Stop();
            };
        }

        private void StartListener()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine(">>Waiting for a connection...");
                    TcpClient client = server.AcceptTcpClient();
                    new HandleClient(clientsTable, client);
                    Console.WriteLine(">>New client connected!");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(">>SocketException: {0}", e);
                server.Stop();
            }
        }
    }

    class HandleClient
    {
        private Hashtable clientsTable;
        private TcpClient client;

        public HandleClient(Hashtable clientsTable, TcpClient client)
        {
            this.clientsTable = clientsTable;
            this.client = client;
            Thread t = new Thread(new ParameterizedThreadStart(HandleDevice));
            t.Start(client);
        }

        private void HandleDevice(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();

            Byte[] bytes = new Byte[1024];
            Byte[] replyBytes = new Byte[1024];
            int i;
            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    Chatroom.Message msg = Chatroom.SocketHelper.Deserialize(bytes) as Chatroom.Message;

                    switch (msg.Header)
                    {
                        case "CREATEUSER":
                            string newUser = msg.Data as string;
                            Console.WriteLine(">>Register new user: {0}", newUser);
                            string replyCode;
                            if (AddUser(msg.Data as string, client))
                            {
                                replyCode = "1";
                            }
                            else
                            {
                                replyCode = "0";
                            }

                            replyBytes = Encoding.ASCII.GetBytes(replyCode);
                            stream.Write(replyBytes, 0, replyBytes.Length);
                            stream.Flush();
                            Console.WriteLine(">>Reply code: {0}", replyCode);

                            UpdateUserListInAllClients();
                            Broadcast("NOTICE", newUser + " is online");

                            break;
                        case "MSG":
                            TcpClient recipientClient = clientsTable[msg.Recipient] as TcpClient;
                            SendMessage(recipientClient, "MSG", msg.Data as string, msg.Sender, msg.Recipient);
                            Console.WriteLine(msg.Sender + " => " + msg.Recipient + ": " + msg.Data as string);
                            break;
                        default:
                            Console.WriteLine(">>" + msg.Data as string);
                            break;
                    }

                    stream.Flush();
                    bytes = new Byte[1024];
                }
            }
            catch (IOException e)
            {
                if (!client.Connected)
                {
                    string user = GetUserName(client);
                    Console.WriteLine(">>Client " + user + " disconnect");
                    if (user != null)
                    {
                        DeleteUser(user);
                        UpdateUserListInAllClients();
                        Broadcast("NOTICE", user + " is offline");
                    }
                    Console.WriteLine(">>" + user + " is removed");
                }
                else
                {
                    Console.WriteLine("Exception: {0}", e.ToString());
                }
            }
        }

        private void UpdateUserListInAllClients()
        {
            Broadcast("UPDATEUSER", GetUsersList());
            Console.WriteLine(">>Update userlist");
        }

        private void Broadcast(string header, object data)
        {
            foreach(TcpClient socket in clientsTable.Values)
            {
                SendMessage(socket, header, data);
            }
        }

        private void SendMessage(TcpClient socket, string header, object data, string sender = null, string recipient = null)
        {
            try
            {
                NetworkStream nwStream = socket.GetStream();
                Chatroom.Message msg = new Chatroom.Message(header, data);
                msg.Sender = sender;
                msg.Recipient = recipient;
                Byte[] replyBytes = Chatroom.SocketHelper.Serialize(msg);
                nwStream.Write(replyBytes, 0, replyBytes.Length);
                nwStream.Flush();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
            }
        }

        private bool AddUser(string username, TcpClient clientSocket)
        {
            if (clientsTable.ContainsKey(username))
            {
                return false;
            }
            clientsTable.Add(username, clientSocket);

            PrintClientsTable();

            return true;
        }

        private string GetUserName(TcpClient client)
        {
            return clientsTable.Keys.OfType<string>().FirstOrDefault(key => clientsTable[key].Equals(client));
        }

        private string[] GetUsersList()
        {
            string[] users = new string[clientsTable.Keys.Count];
            clientsTable.Keys.CopyTo(users, 0);
            return users;
        }

        private void DeleteUser(string username)
        {
            clientsTable.Remove(username);
        }

        private void PrintClientsTable()
        {
            Console.WriteLine();
            Console.WriteLine("-----Client table-----");
            foreach (string key in clientsTable.Keys)
            {
                Console.WriteLine(key + ": " + clientsTable[key]);
            }
            Console.WriteLine();
        }
    }
}
