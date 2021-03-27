using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TcpClient = NetCoreServer.TcpClient;


namespace tcp_client_1
{

    public partial class Form1 : Form
    {
        static public int threadK = 1;
        static public bool sendMes = false;
        public static bool bbreak = false;

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void serverThread()
        {
            string address = "127.0.0.1";
            int port = Int32.Parse(txtPort.Text);

            ChatClient client = new ChatClient(address, port);
            client.ConnectAsync();
            for (; ; )
            {
                if (bbreak == true) { bbreak = false; break; }
                string line = Console.ReadLine();
                

                // Disconnect the client
                if (threadK == 0)
                {
                    listBox1.Items.Add("Client disconnecting...");
                    client.DisconnectAsync();
                    listBox1.Items.Add("Done");
                    threadK = 1;
                    continue;
                }

                // Send the entered text to the chat server
                if (sendMes)
                {
                    client.SendAsync(txtSend.Text);
                    sendMes = false;
                }
              
            }
            listBox1.Items.Add("Client disconnecting...");
 
            client.DisconnectAndStop();
            listBox1.Items.Add("Done");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            threadK = 1;
            Thread thr1 = new Thread(serverThread);
            thr1.Name = "Server";
            thr1.Start();
           
            

          

        }

        private void button3_Click(object sender, EventArgs e)
        {
            threadK = 0;
        }

        private void btnSend(object sender, EventArgs e)
        {
            listBox1.TopIndex = listBox1.Items.Count - 1;
            sendMes = true;
        }

        private void lblTextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Add(label1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bbreak = true;
        }
    }


    class ChatClient : TcpClient
    {
        public ChatClient(string address, int port) : base(address, port) { }

        public void DisconnectAndStop()
        {
            _stop = true;
            DisconnectAsync();
            while (IsConnected)
                Thread.Yield();
        }

        protected override void OnConnected()
        {
            Application.OpenForms[0].Controls["label1"].Text = $"TCP connected : {Id}";
        }

        protected override void OnDisconnected()
        {
            Application.OpenForms[0].Controls["label1"].Text = $"TCP disconnected : {Id}";
            // Wait for a while...
            Thread.Sleep(1000);

            // Try to connect again
            if (!_stop)
                ConnectAsync();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Application.OpenForms[0].Controls["label1"].Text = $"Incoming message : {message}";
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP client caught an error with code {error}");
        }

        private bool _stop;
    }

}
