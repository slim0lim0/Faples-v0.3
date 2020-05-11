using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaplesNet
{
    public enum ePacketType
    {
        eDISCONNECT = -1,
        eINIT = 0,
        eLOGIN = 1,
        eLOGIN_SUCCEED = 2,
        eLOGIN_FAIL = 3,
        eWORLD_SUCCEED = 4,
        eWORLD_FAIL = 5,
        eCHANNEL_SUCCEED = 10,
        eCHANNEL_FAIL = 11,
        eCHARARACTER_SELECT = 6,
        eCHARACTER_PLAY = 7,
        eCHARACTER_CREATE = 8,
        eCHARACTER_CREATE_SUCCEED = 9,
        eCHARACTER_CREATE_FAIL = 10,
        eCHAT_MESSAGE = 99     
    }

    [Serializable]
    public class Packet
    {
        public Guid ID { get; set; } = new Guid();
        public int Type { get; set; }
        public byte[] Data { get; set; }
    }

    [Serializable]
    public class LoginState
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Serializable]
    public class WorldState
    {
        public string World { get; set; }
        public int Channel { get; set; }
    }
    [Serializable]
    public class CharacterState
    {
        public string Name { get; set; }
    }
    [Serializable]
    public class ChatMessage
    {
        public int Type { get; set; } = 0;
        public string Message { get; set; }
    }
}
