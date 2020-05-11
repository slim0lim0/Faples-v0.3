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

namespace FaplesServer
{
    public partial class FplServerThreaded : Form
    {
        DatabaseManager g_oDbManager = new DatabaseManager();

        Dictionary<Guid, Session> Clients = new Dictionary<Guid, Session>();
        TcpListener gListener;
        Thread gListenThread;

        const int PACKET_MARKER = 2; // size of : ;$

        bool Running { get; set; } = false;
        public FplServerThreaded()
        {
            InitializeComponent();
            Prepare();
        }
        public void Prepare()
        {
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    txtServerIP.Text = address.ToString();
                }
            }

            PrepareDatabase();


            ManageControls();
        }

        public void PrepareDatabase()
        {
            g_oDbManager.InitDatabase();
        }

        private void btnServerStart_Click(object sender, EventArgs e)
        {
            Running = true;
            this.gListener = new TcpListener(IPAddress.Any, 29278);
            this.gListenThread = new Thread(new ThreadStart(ListenForClients));
            this.gListenThread.Start();

            ManageControls();
        }


        private void ListenForClients()
        {
            this.gListener.Start();

            while (true)
            {
                this.txtServerLog.Invoke(new MethodInvoker(delegate ()
                {
                    txtServerLog.AppendText("Waiting for next connection...");
                    txtServerLog.AppendText(Environment.NewLine);
                }));
                //blocks until a client has connected to the server
                TcpClient client = this.gListener.AcceptTcpClient();
                var ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address;

                this.txtServerLog.Invoke(new MethodInvoker(delegate ()
                {
                    txtServerLog.AppendText("Client (" + ip + ") - Connecting...");
                    txtServerLog.AppendText(Environment.NewLine);
                }));


                //create a thread to handle communication
                //with connected client

                Session session = new Session()
                {
                    ID = Guid.NewGuid(),
                    Client = client
                };

                Clients.Add(session.ID, session);

                SendConfirmation(session);

                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));

                clientThread.Start(session);

                this.txtServerLog.Invoke(new MethodInvoker(delegate ()
                {
                    txtServerLog.AppendText("Client connected successfully!");
                    txtServerLog.AppendText(Environment.NewLine);
                    txtServerLog.AppendText(Environment.NewLine);
                }));
            }
        }

        public void SendConfirmation(Session session)
        {
            using (NetworkStream clientStream = new NetworkStream(session.Client.Client))
            {

                Packet packet = new Packet
                {
                    ID = session.ID,
                    Type = 0,
                    Data = null
                };

          
                byte[] packetInfo = BuildPacketStream(packet, "");

                clientStream.Write(packetInfo, 0, packetInfo.Length);
                clientStream.Flush();
            }
        }

        public byte[] BuildPacketStream(Packet packet, string sData)
        {
            byte[] yReturn = null;

            string sTotal = "";

            sTotal += packet.ID.ToString() + ";$";
            sTotal += packet.Type.ToString() + ";$";
            sTotal += sData;

            yReturn = Encoding.ASCII.GetBytes(sTotal);

            return yReturn;
        }

        private void HandleClientComm(object session)
        {
            Session tcpSession = (Session)session;
            NetworkStream clientStream = tcpSession.Client.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    Clients[tcpSession.ID].Client.Close();
                    break;
                }

                //Packet packet = Utility.FromByteArray<Packet>(message);



                //    Packet packet = new Packet();
                //packet.ID = Guid.Parse(sData);

                Packet packet  = CreatePacket(message);

                if (ManagePacket(packet))
                {
                    break;
                }
            }

            Clients[tcpSession.ID].Client.Close();
            Clients.Remove(tcpSession.ID);
            txtServerLog.Invoke(new MethodInvoker(delegate ()
            {
                txtServerLog.AppendText("Client disconnectioned on Guid: " + tcpSession.ID);
                txtServerLog.AppendText(Environment.NewLine);
            }));
        }

        private Packet CreatePacket(byte[] yData)
        {
            Packet inPacket = new Packet();

            string sData = Encoding.UTF8.GetString(yData);
            int track = 0;
            sData = sData.Substring(0, sData.LastIndexOf(";$") + PACKET_MARKER);

            while(sData.Length != 0)
            {
                int iFirst = sData.IndexOf(";$");
                string sStrip = sData.Substring(0, iFirst);
                sData = sData.Remove(0, sStrip.Length + PACKET_MARKER);

                switch (track)
                {
                    case 0:
                        inPacket.ID = Guid.Parse(sStrip);
                        break;
                    case 1:
                        inPacket.Type = int.Parse(sStrip);
                        break;
                    case 2:
                        inPacket.Data = TranslateData(inPacket.Type, sData);
                        sData = "";
                        break;
                    default:
                        break;
                }
                if (track < 3)
                    track++;
            }         

            return inPacket;
        }

        private byte[] TranslateData(int iType, string sData)
        {
            byte[] yReturnData = null;

            if(iType == (int) eSessionState.eLOGIN)
            {
                LoginState login = new LoginState();
                int track = 0;

                while(sData.Length != 0)
                {
                    int iFirst = sData.IndexOf(";$");
                    string sStrip = sData.Substring(0, iFirst);
                    sData = sData.Remove(0, sStrip.Length + PACKET_MARKER);

                    switch (track)
                    {
                        case 0:
                            login.Username = sStrip;
                            break;
                        case 1:
                            login.Password = sStrip;
                            break;
                        case 2:
                            break;
                        default:
                            break;
                    }
                    if (track < 3)
                        track++;
                }

                yReturnData = Utility.ToByteArray(login);
            }
            else if(iType == (int) eSessionState.eCHARCREATE)
            {
                CharacterState character = new CharacterState();
                int track = 0;

                while (sData.Length != 0)
                {
                    int iFirst = sData.IndexOf(";$");
                    string sStrip = sData.Substring(0, iFirst);
                    sData = sData.Remove(0, sStrip.Length + PACKET_MARKER);

                    switch (track)
                    {
                        case 0:
                            character.Name = sStrip;
                            break;
                        default:
                            break;
                    }
                    if (track < 3)
                        track++;
                }

                yReturnData = Utility.ToByteArray(character);
            }

            return yReturnData;
        }

        private bool ManagePacket(Packet inPacket)
        {
            bool Finished = false;
            LoginState oNewLogin = null;
            WorldState oWorld = null;
            CharacterState oCharacter = null;

            if (inPacket.Type == (int)ePacketType.eLOGIN) // 1
            {
                oNewLogin = Utility.FromByteArray<LoginState>(inPacket.Data);

                if (oNewLogin != null)
                {
                    Clients[inPacket.ID].LoginInfo = oNewLogin;

                    using (NetworkStream clientStream = new NetworkStream(Clients[inPacket.ID].Client.Client))
                    {
                        bool bLoginError = false;

                        UserLogin oLogin = g_oDbManager.GetRecordByKey<UserLogin>("Users", "Username", oNewLogin.Username);

                        if (oLogin != null)
                        {
                            if (oNewLogin.Password == oLogin.Password)
                            {
                                Packet outPacket = new Packet
                                {
                                    ID = inPacket.ID,
                                    Type = (int) ePacketType.eLOGIN_SUCCEED,   // Success
                                    Data = null
                                };

                                string sPacketResult = "Login successful!;$";

                                byte[] packetInfo = BuildPacketStream(outPacket, sPacketResult); //  Utility.ToByteArray(outPacket);

                                clientStream.Write(packetInfo, 0, packetInfo.Length);
                                clientStream.Flush();

                                txtServerLog.Invoke(new MethodInvoker(delegate ()
                                {
                                    txtServerLog.AppendText("Logged in: " + oNewLogin.Username);
                                    txtServerLog.AppendText(Environment.NewLine);
                                }));
                            }
                            else
                            {
                                bLoginError = true;
                            }
                        }
                        else
                        {
                            bLoginError = true;
                        }



                        if(bLoginError)
                        {
                            Packet outPacket = new Packet
                            {
                                ID = inPacket.ID,
                                Type = (int)ePacketType.eLOGIN_FAIL,   // Failure
                                Data = Encoding.ASCII.GetBytes("Error: Login failed")
                            };


                            string sPacketResult = "Error: Login failed.;$";

                            byte[] packetInfo = BuildPacketStream(outPacket, sPacketResult); //  Utility.ToByteArray(outPacket);

                            clientStream.Write(packetInfo, 0, packetInfo.Length);
                            clientStream.Flush();
                        };
                    }
 
                }
            }
            else if (inPacket.Type == (int)ePacketType.eWORLD_SUCCEED)
            {
                oWorld = Utility.FromByteArray<WorldState>(inPacket.Data);

                if (oWorld != null)
                {
                    Clients[inPacket.ID].WorldInfo = oWorld;

                    txtServerLog.Invoke(new MethodInvoker(delegate ()
                    {
                        txtServerLog.AppendText("User - " + Clients[inPacket.ID].LoginInfo.Username + ": Selected " + oWorld.World);
                        txtServerLog.AppendText(Environment.NewLine);
                    }));
                }

                using (NetworkStream clientStream = new NetworkStream(Clients[inPacket.ID].Client.Client))
                {
                    Packet outPacket = new Packet
                    {
                        ID = inPacket.ID,
                        Type = (int) ePacketType.eWORLD_SUCCEED,
                        Data = null
                    };

                    byte[] packetInfo = Utility.ToByteArray(outPacket);

                    clientStream.Write(packetInfo, 0, packetInfo.Length);
                    clientStream.Flush();
                }
            }
            else if(inPacket.Type == (int)ePacketType.eCHARACTER_CREATE)
            {
                oCharacter = Utility.FromByteArray<CharacterState>(inPacket.Data);

                //Utility.FromByteArray<LoginState>(inPacket.Data 
                //UserLogin oLogin = g_oDbManager.GetRecordByKey<UserLogin>("Users", "Username", oNewLogin.Username);

                List<CharacterMap> oCharacters = g_oDbManager.GetRecords<CharacterMap>("CharacterNameMap");
                var oCharMap = oCharacters.Find(x => x.Name == oCharacter.Name);
                UserLogin oLogin = g_oDbManager.GetRecordByKey<UserLogin>("Users", "Username", Clients[inPacket.ID].LoginInfo.Username);

                if (oCharMap == null)
                {

                    if (oCharacter != null)
                    {
                        oCharMap = new CharacterMap();
                        oCharMap.Name = oCharacter.Name;

                        g_oDbManager.InsertRecord("CharacterNameMap", oCharMap);

                        oLogin.Characters.Clear();
                        oLogin.Characters.Add(new Character { Name = oCharMap.Name, Class = (int)eClass.Beginner });

                        g_oDbManager.UpdateRecord("Users", oLogin);

                        Clients[inPacket.ID].CharacterInfo = oCharacter;

                        txtServerLog.Invoke(new MethodInvoker(delegate ()
                        {
                            txtServerLog.AppendText("User - " + Clients[inPacket.ID].LoginInfo.Username + ": Created Character - " + oCharacter.Name);
                            txtServerLog.AppendText(Environment.NewLine);
                        }));


                        using (NetworkStream clientStream = new NetworkStream(Clients[inPacket.ID].Client.Client))
                        {
                            Packet outPacket = new Packet
                            {
                                ID = inPacket.ID,
                                Type = (int)ePacketType.eCHARACTER_CREATE_SUCCEED,
                                Data = null
                            };

                            string sPacketResult = "Character created successfully!;$";

                            byte[] packetInfo = BuildPacketStream(outPacket, sPacketResult);

                            clientStream.Write(packetInfo, 0, packetInfo.Length);
                            clientStream.Flush();
                        }
                    }                
                }
                else
                {
                    if (oCharacter != null)
                    {
                        Clients[inPacket.ID].CharacterInfo = oCharacter;

                        txtServerLog.Invoke(new MethodInvoker(delegate ()
                        {
                            txtServerLog.AppendText("User - " + Clients[inPacket.ID].LoginInfo.Username + ": Attempted create. Character Name EXISTS.");
                            txtServerLog.AppendText(Environment.NewLine);
                        }));
                    }

                    using (NetworkStream clientStream = new NetworkStream(Clients[inPacket.ID].Client.Client))
                    {
                        Packet outPacket = new Packet
                        {
                            ID = inPacket.ID,
                            Type = (int)ePacketType.eCHARACTER_CREATE_FAIL,
                            Data = null
                        };

                        string sPacketResult = "Character creation failed!;$";

                        byte[] packetInfo = BuildPacketStream(outPacket, sPacketResult);

                        clientStream.Write(packetInfo, 0, packetInfo.Length);
                        clientStream.Flush();
                    }
                }
              

           
            }
            else if (inPacket.Type == (int)ePacketType.eCHAT_MESSAGE)
            {
                ChatMessage msg = Utility.FromByteArray<ChatMessage>(inPacket.Data);

                txtServerChat.Invoke(new MethodInvoker(delegate ()
                {
                    txtServerChat.AppendText("User:" + msg.Message + "\n");
                }));



                byte[] messageInfo = Utility.ToByteArray(msg);

                foreach (Session session in Clients.Values)
                {
                    using (NetworkStream clientStream = new NetworkStream(session.Client.Client))
                    {
                        Packet outPacket = new Packet
                        {
                            ID = inPacket.ID,
                            Type = 99,
                            Data = messageInfo
                        };

                        byte[] packetInfo = Utility.ToByteArray(outPacket);

                        clientStream.Write(packetInfo, 0, packetInfo.Length);
                        clientStream.Flush();
                    }
                }
            }

            return Finished;
        }

        private void ManageControls()
        {
            bool bRunning = false;


            bRunning = Running;

            txtServerIP.Enabled = !bRunning;
            txtServerPort.Enabled = !bRunning;
            btnServerStart.Enabled = !bRunning;
            btnServerStop.Enabled = bRunning;
            txtMessage.Enabled = bRunning;
            btnSendMsg.Enabled = bRunning;
        }
    }
}
