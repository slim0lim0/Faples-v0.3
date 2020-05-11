using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FaplesNet;

namespace FaplesClientHarness
{
    public partial class FplClientThreaded : Form
    {
        Session gSession;
        private NetworkStream NST;
        private bool Running { get; set; } = false;
        public FplClientThreaded()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Running = true;

            gSession = new Session()
            {
                ID = new Guid(),
                Client = new TcpClient(AddressFamily.InterNetwork)
            };

            int port = int.Parse(txtServerPort.Text);

            IPEndPoint IpServer = new IPEndPoint(IPAddress.Parse(txtServerIP.Text), port);
            try
            {
                gSession.Client.Connect(IpServer);
                if (gSession.Client.Connected)
                {
                    txtServerLog.AppendText("Connected to Server!\n");
                    NST = new NetworkStream(gSession.Client.Client);

                    Thread clientThread = new Thread(new ThreadStart(HandleServerComm));
                    clientThread.Start();
                }
                else
                    txtServerLog.AppendText("Connection to Server failed!\n");
            }
            catch (Exception ex)
            {
                Running = false;
                MessageBox.Show(ex.Message);
            }

            ManageControls();
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            gSession.Client.Close();

            txtServerLog.Invoke(new MethodInvoker(delegate ()
            {
                txtServerLog.AppendText("User has disconnected.\n");
            }));
        }

        private void HandleServerComm()
        {
            //NetworkStream clientStream = new NetworkStream(gSession.Client.Client);

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = NST.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }


                //message has successfully been received

                Packet packet = Utility.FromByteArray<Packet>(message);

                if (ProcessPacket(packet))
                {
                    break;
                }
            }


            if(gSession.Client != null)
                gSession.Client.Close();


            gSession = null;
            Running = false;
            this.Invoke(new MethodInvoker(delegate ()
            {
                ManageControls();
            }));
        }

        private bool ProcessPacket(Packet packet)
        {
            bool Finished = false;

            if(packet.Type == -1)
            {
                Finished = true;
            }
            else if(packet.Type == 0)
            {
                gSession.ID = packet.ID;
                gSession.State = 0;

                txtServerLog.Invoke(new MethodInvoker(delegate ()
                {
                    txtServerLog.AppendText("Packet processed: Guid Set...    State: Login\n");
                }));
            }
            else if(packet.Type == 1)
            {
                gSession.State = 1;
                txtServerLog.Invoke(new MethodInvoker(delegate ()
                {
                    txtServerLog.AppendText("Packet processed: Login successful...  State: World Select\n");
                }));
            }
            else if(packet.Type == 2)
            {
                gSession.State = 0;
                gSession.LoginInfo = null;
                txtServerLog.Invoke(new MethodInvoker(delegate ()
                {
                    txtServerLog.AppendText("Packet processed: Login failed...  State: Login\n");
                }));
            }
            else if(packet.Type == 99)
            {
                ChatMessage msg = Utility.FromByteArray<ChatMessage>(packet.Data);

                txtServerChat.Invoke(new MethodInvoker(delegate ()
                {
                    txtServerChat.AppendText("User:" + msg.Message + "\n");
                }));
            }

            return Finished;
        }

        private void ManageControls()
        {
            bool bRunning = false;


            bRunning = Running;

            txtServerIP.Enabled = !bRunning;
            txtServerPort.Enabled = !bRunning;
            btnConnect.Enabled = !bRunning;
            btnDisconnect.Enabled = bRunning;
            txtMessage.Enabled = bRunning;
            btnSendMsg.Enabled = bRunning;
        }
        private void BtnSendMsg_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text != "")
            {
                ChatMessage message = new ChatMessage()
                {
                    Message = txtMessage.Text
                };

                byte[] messageInfo = Utility.ToByteArray(message);

                Packet packet = new Packet
                {
                    ID = gSession.ID,
                    Type = 99,
                    Data = messageInfo
                };

                byte[] packetInfo = Utility.ToByteArray(packet);

                NST.Write(packetInfo, 0, packetInfo.Length);
                NST.Flush();
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginState newLogin = new LoginState()
            {
                Username = "Fapler1",
                Password = "1234"
            };

            gSession.LoginInfo = newLogin;

            if(gSession.State == 0)
            {
                byte[] loginInfo = Utility.ToByteArray(newLogin);

                Packet packet = new Packet
                {
                    ID = gSession.ID,
                    Type = 0,
                    Data = loginInfo
                };


                byte[] packetInfo = Utility.ToByteArray(packet);
               // NetworkStream clientStream = new NetworkStream(gSession.Client.Client);

                NST.Write(packetInfo, 0, packetInfo.Length);
                NST.Flush();
            }
            else
            {
                txtServerLog.Invoke(new MethodInvoker(delegate ()
                {
                    txtServerLog.AppendText("User already logged in....  State: World Select\n");
                }));
            }
        }

        private void BtnWorld1_Click(object sender, EventArgs e)
        {
            WorldState world = new WorldState()
            {
                World = "World1",
                Channel = 0
            };

            gSession.WorldInfo = world;

            byte[] worldInfo = Utility.ToByteArray(world);

            Packet packet = new Packet
            {
                ID = gSession.ID,
                Type = (int) ePacketType.eWORLD_SUCCEED,
                Data = worldInfo
            };

            byte[] packetInfo = Utility.ToByteArray(packet);

            NST.Write(packetInfo, 0, packetInfo.Length);
            NST.Flush();
        }

        private void BtnWorld2_Click(object sender, EventArgs e)
        {
            WorldState world = new WorldState()
            {
                World = "World2",
                Channel = 0
            };

            gSession.WorldInfo = world;

            byte[] worldInfo = Utility.ToByteArray(world);

            Packet packet = new Packet
            {
                ID = gSession.ID,
                Type = (int)ePacketType.eWORLD_SUCCEED,
                Data = worldInfo
            };

            byte[] packetInfo = Utility.ToByteArray(packet);

            NST.Write(packetInfo, 0, packetInfo.Length);
            NST.Flush();
        }

        private void BtnWorld3_Click(object sender, EventArgs e)
        {
            WorldState world = new WorldState()
            {
                World = "World3",
                Channel = 0
            };

            gSession.WorldInfo = world;

            byte[] worldInfo = Utility.ToByteArray(world);

            Packet packet = new Packet
            {
                ID = gSession.ID,
                Type = (int)ePacketType.eWORLD_SUCCEED,
                Data = worldInfo
            };

            byte[] packetInfo = Utility.ToByteArray(packet);

            NST.Write(packetInfo, 0, packetInfo.Length);
            NST.Flush();
        }
    }
}
