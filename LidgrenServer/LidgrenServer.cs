using GameData;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace LidgrenServer
{
    public class Server
    {
        public static List<PlayerData> Players = new List<PlayerData>();
        public static List<PlayerData> RegisteredPlayers = new List<PlayerData>();

        public static NetPeerConfiguration config = new NetPeerConfiguration("ppMyGame")
        {
            Port = 5001
        };
        public static NetServer server;

        public Server()
        {
            // Create a list of players
            RegisteredPlayers = Utility.CreatePlayerContext();
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.StatusChanged);
            server = new NetServer(config);
            config.AutoFlushSendQueue = true;
            server.Start();
        }

        public static void process(NetIncomingMessage msgIn, string inMess)
        {
            Console.WriteLine("Data " + inMess);
            process(DataHandler.ExtractMessage<TestMess>(inMess));
            process(DataHandler.ExtractMessage<ErrorMess>(inMess));
            process(DataHandler.ExtractMessage<PlayerData>(inMess));
            process(DataHandler.ExtractMessage<LeavingData>(inMess));
            process(DataHandler.ExtractMessage<JoinRequestMessage>(inMess));

        }
        public static void process(LeavingData leaving)
        {
            if (leaving == null) return;
            var found = Players.FirstOrDefault(p => p.playerID == leaving.playerID);
            if (found != null)
                Players.Remove(found);
            Console.WriteLine("{0} has Left ", leaving.playerID);

        }
        private static void process(JoinRequestMessage joinRequest)
        {
            if (joinRequest == null) return;
            PlayerData found = RegisteredPlayers
                                .FirstOrDefault(p => p.GamerTag.ToUpper() == joinRequest.TagName.ToUpper()
                                                    && p.Password.ToUpper() == joinRequest.Password.ToUpper());
            if (found == null)
                DataHandler.sendNetMess<ErrorMess>(server, new ErrorMess { message = " Illegal login attempt by " + joinRequest.TagName }, SENT.TOALL);
            else
            {
                found.header = "Joining";
                found.X = Utility.NextRandom(0, 600);
                found.Y = Utility.NextRandom(0, 800);
                DataHandler.sendNetMess<PlayerData>(server, found, SENT.TOALL);
            }
        }
        private static void process(ErrorMess errorMess)
        {
            if (errorMess == null) return;

        }
        private static void process(PlayerData playerData)
        {
            if (playerData == null) return;
        }
        private static void process(TestMess testMess)
        {
            if (testMess == null) return;
            DataHandler.sendNetMess<TestMess>(Server.server,
                        new TestMess { message = "Test Reply" }, SENT.TOALL);
        }
    }
}
