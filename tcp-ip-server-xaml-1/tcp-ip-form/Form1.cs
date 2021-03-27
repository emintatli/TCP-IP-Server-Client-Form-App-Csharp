using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NetCoreServer;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace tcp_ip_form
{
   
    
    public partial class Form1 : Form
    {
        public static bool conStat = false;
        int threadK;
        int threadK2;
        public static string dataSend = "";
        public static bool dataSendBool = false;
        public static bool sync = false;
        public static bool dataSendBool2 = false;
        int sync_int = 0;
        public static bool lblStatus = false;
        public static bool lblStatus2 = false;


        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            picTrue.Visible = false;
        }
     

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
        }
        public void threadControl(int i)
        {
            threadK = i; //1 run // 0 stop 

        }
      
        private void btnConnect_Click(object sender, EventArgs e)
        {

      
            
                picFalse.Visible = false;
                picTrue.Visible = true;
            
           
            Thread thr1 = new Thread(threadServer);
            thr1.Name = "Server";
            thr1.Start();
        }

        public void threadServer()
        {
            threadControl(1);

            int port = Int32.Parse(txtPort.Text);
            IPAddress sIP = IPAddress.Any;
            var server = new ChatServer(sIP, port);

            // Start the server
            listStatus.Items.Add($"Server starting > {sIP.ToString()}:{port}");
            server.Start();
            listStatus.Items.Add("Done!");


            for (; ; ) // inf
            {
                Application.DoEvents();
                Thread.Sleep(200);
                progressBar1.Value = 0;




                string line = Console.ReadLine();
                if (threadK == 0) //stop server
                break;

                // Restart the server
                if (threadK == 3)
                {
                    progressBar1.Value = 0;
                    listStatus.Items.Add("Server restarting...");
                    server.Restart();
                    threadControl(1);
                    listStatus.Items.Add("Done!");
                    continue;
                }

                // Multicast admin message to all sessions
               // line = "000.000.000.002";
             //   server.Multicast(line);
                if (dataSendBool) {
                    server.Multicast(dataSend);
                    listStatus.Items.Add($"Message Sent:{txtSend.Text}");
                    listStatus.SelectedIndex = listStatus.Items.Count - 1;
                    Application.DoEvents();
                    progressBar1.Value = 100;
                    dataSendBool = false;
                }
                if (dataSendBool2)
                {
                    server.Multicast(labelSt2.Text);
                    Application.DoEvents();
                    progressBar1.Value = 100;
                    dataSendBool2 = false;
                }
            }
            listStatus.Items.Add("Server stopping...");
            server.Stop();
            listStatus.Items.Add("Done!");



        }



        public void threadServer2()
        {
            

            int port2 = Int32.Parse(txtPort2.Text);
            IPAddress sIP2 = IPAddress.Any;
            var server2 = new ChatServer(sIP2, port2);

            // Start the server
            listStatus.Items.Add($"Server starting > {sIP2.ToString()}:{port2}");
            server2.Start();
            listStatus.Items.Add("Done!");


            for (; ; ) // inf
            {
                string line = Console.ReadLine();
                if (threadK2 == 0) //stop server
                    break;

                // Restart the server
                //if (threadK2 == 3)
                //{
                //    progressBar1.Value = 0;
                //    listStatus.Items.Add("Server restarting...");
                //    server2.Restart();
                //    threadK2 = 1;
                //    listStatus.Items.Add("Done!");
                //    continue;
                //}

                // Multicast admin message to all sessions
               // line = "000.000.000.002";
                //   server.Multicast(line);
                if (lblStatus2)
                {
                    server2.Multicast(labelSt2.Text);
                    dataSendBool2 = true;
                    listStatus.Items.Add($"Message Sent:{labelSt2.Text}");
                    listStatus.SelectedIndex = listStatus.Items.Count - 1;
                    lblStatus2 = false;
                }
            }
            listStatus.Items.Add("Server stopping...");
            server2.Stop();
            listStatus.Items.Add("Done!");



        }



        private void btnStop_Click(object sender, EventArgs e)
        {
            picFalse.Visible = true;
            picTrue.Visible = false;
            threadControl(0);
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
        
            threadControl(3);
          
            
        }

      

       

        private void StTextChanged(object sender, EventArgs e)
        {
            listStatus.Items.Add($"Incoming Message: {labelSt.Text}");
            listStatus.SelectedIndex = listStatus.Items.Count - 1;
            lblStatus = true;



        }

        private void btnSend_Click(object sender, EventArgs e)
        {
           
            dataSend = txtSend.Text;
            dataSendBool = true;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            dataSend = txtSend.Text;
            dataSendBool = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            sync_int++;
            if (sync_int == 1)
            {
                btnSync.Text = "ON";
                sync = true;

            }
            else if (sync_int == 2)
            {
                btnSync.Text = "OFF";
                sync = false;
                sync_int = 0;
            }
            if (sync == true)
            {
                timer1.Interval = Int32.Parse(txtInvertal.Text);
                timer1.Enabled = true;
            }
            else if (sync == false)
            {
                timer1.Interval = Int32.Parse(txtInvertal.Text);
                timer1.Enabled = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
          
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            threadK2 = 1;
            Thread thr2 = new Thread(threadServer2);
            thr2.Name = "Server2";
            thr2.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            threadK2 = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            threadK2 = 3;
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblSt2TextChanged(object sender, EventArgs e)
        {
            lblStatus2 = true;
        }

        private void listboxVal(object sender, EventArgs e)
        {
          
        }
    }

    class ChatSession : TcpSession
    {
        public ChatSession(TcpServer server) : base(server) { }

        protected override void OnConnected()
        {

            Console.WriteLine($"Chat TCP session with Id {Id} connected!");

            // Send invite message
            string message = "000.000.000.002";
          //  SendAsync(message);  //AAAAA
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Chat TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            bool clientornot = false;
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Debug.WriteLine("Incoming: " + message);
            foreach (char ch in message) { if (ch == '#') { clientornot = true; } }
            if (clientornot)
            {
                Application.OpenForms[0].Controls["labelSt2"].Text = message;
                clientornot = false;
            }
            else
            {
                Application.OpenForms[0].Controls["labelSt"].Text = message;
            }
            
            if (message == "mc")
            {
                Form1.conStat = true;
            }
            else
            {
                Form1.conStat = false;
            }







            // Multicast message to all connected sessions
            //   Server.Multicast(message);

            // If the buffer starts with '!' the disconnect the current session
            if (message == "!")
                Disconnect();
        }
       

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP session caught an error with code {error}");
        }
    }


    class ChatServer : TcpServer
    {
        public ChatServer(IPAddress address, int port) : base(address, port) { }

        protected override TcpSession CreateSession() { return new ChatSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP server caught an error with code {error}");
        }
    }
}
