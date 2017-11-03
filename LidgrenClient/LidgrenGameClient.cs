using GameData;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LidgrenClient
{
    public class LidgrenGameClient
    {
        public static PlayerData player;
        public static List<PlayerData> otherPlayers = new List<PlayerData>();
        public static  NetPeerConfiguration ClientConfig;
        public static NetClient client;

        public static string ServerMessage;

        public LidgrenGameClient()
        {
            ClientConfig = new NetPeerConfiguration("myGame");
            //for the client
            ClientConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            client = new NetClient(ClientConfig);
            client.Start();
            client.DiscoverLocalPeers(5001);
        }
        public static void process(NetIncomingMessage msgIn, string inMess)
        {
            process(DataHandler.ExtractMessage<TestMess>(inMess));
            process(DataHandler.ExtractMessage<ErrorMess>(inMess));
            process(DataHandler.ExtractMessage<PlayerData>(inMess));
            process(DataHandler.ExtractMessage<LeavingData>(inMess));

        }
        public static void process(LeavingData leaving)
        {
            if (leaving == null) return;
            var found = otherPlayers.FirstOrDefault(p => p.playerID == leaving.playerID);
            if (found != null)
                otherPlayers.Remove(found);
            

        }



        private static void process(ErrorMess errorMess)
        {
            if (errorMess == null) return;

        }

        private static void process(PlayerData playerData)
        {
            if (playerData == null) return;
            switch (playerData.header)
            {

                default:
                    ServerMessage = "Unknown player Data header";
                    break;
                    
            }
        }

        private static void process(TestMess testMess)
        {
            if (testMess == null) return;
            else ServerMessage = testMess.message;
        }
    }
}
