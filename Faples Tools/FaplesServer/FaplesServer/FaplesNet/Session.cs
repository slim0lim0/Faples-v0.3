using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FaplesNet
{
    public enum eSessionState
    {
        eINIT = 0,
        eLOGIN = 1,
        eWORLD = 4,
        eCHANNEL = 5,
        eCHARSELECT = 6,
        eCHARPLAY = 7,
        eCHARCREATE = 8
    }
    public class Session
    {
        public TcpClient Client { get; set; }
        public Guid ID { get; set; }
        public int State { get; set; } = 0;
        public LoginState LoginInfo { get; set; }
        public WorldState WorldInfo { get; set; }
        public CharacterState CharacterInfo { get; set; }
    }

    //public class WorldState
    //{
    //    public string World { get; set; }
    //    public int Channel { get; set; }
    //}

    //public class LoginState
    //{
    //    public string Username { get; set; }
    //    public string Password { get; set; }
    //}
}
