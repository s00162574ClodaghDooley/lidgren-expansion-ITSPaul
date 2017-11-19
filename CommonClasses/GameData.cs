using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GameData
{

    public class PlayerData
    {
        public string header = string.Empty;
        public string playerID;
        public string imageName = string.Empty;
        public string GamerTag = string.Empty;
        public string PlayerName = string.Empty;
        public int XP;
        public float X;
        public float Y;
        public string Password;

        public PlayerData() { }
        public PlayerData(string messageHeader, string ImgName, string id, string tag, float x, float y)
        {
            header = messageHeader;
            playerID = id;
            imageName = ImgName;
            X = x;
            Y = y;
            GamerTag = tag;
        }

        public string PlayerMessage(string header)
        {
            return header + ":" + playerID + ":" + X.ToString() + ":" + Y.ToString();
        }


    }

    public class MovedData
    {
        public string playerID;
        public float toX;
        public float toY;
    }

    public class LeavingData
    {
        public string playerID;
        public string Tag;
    }

    public class GetWorldSize
    {
        public int X;
        public int y;

    }

    public class Initialise { public string message;  };

    public class JoinRequestMessage
    {
        public string TagName;
        public string Password;

    }

    public class TestMess
    {
        public string message = "Hello there";
    }

    public class ErrorMess
    {
        public string message = "Error --> ";
    }
    public class Joined
    {
        public string playerId;
        public string gameTag;
        public int atX;
        public int atY;
    }

    public class GamePacket<T>{
        public GamePacket(T obj)
        {
            val = obj;
        }
        public T val;
        public Type type;
    }



}
