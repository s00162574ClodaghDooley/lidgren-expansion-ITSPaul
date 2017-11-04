using GameComponentNS;
using GameData;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LidgrenClient
{
    public class LidgrenGameClient : GameComponent
    {
        public static PlayerData player;
        public static List<PlayerData> otherPlayers = new List<PlayerData>();
        public static  NetPeerConfiguration ClientConfig;
        public static NetClient client;

        public static string IncomingServerMessage;

        public LidgrenGameClient(Game game) : base(game)
        {
            game.Components.Add(this);
            ClientConfig = new NetPeerConfiguration("myGame");
            //for the client
            ClientConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            client = new NetClient(ClientConfig);
            client.Start();
            client.DiscoverLocalPeers(5001);
            
        }

        public override void Update(GameTime gameTime)
        {
            checkMessages();
            base.Update(gameTime);
        }

        private void checkMessages()
        {
            NetIncomingMessage ServerMessage;
            if ((ServerMessage = LidgrenGameClient.client.ReadMessage()) != null)
            {
                switch (ServerMessage.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        string message = ServerMessage.ReadString();
                        process(ServerMessage, message);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        
                        LidgrenGameClient.client.Connect(ServerMessage.SenderEndPoint);
                         IncomingServerMessage = "Connected to " + ServerMessage.SenderEndPoint.Address.ToString();
                        new FadeText(Game, Vector2.Zero, IncomingServerMessage);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        switch (ServerMessage.SenderConnection.Status)
                        {
                            /* .. */
                        }
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        //InGameMessage = ServerMessage.ReadString();
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        break;
                    default:
                        IncomingServerMessage = "unhandled message with type: "
                            + ServerMessage.MessageType.ToString();
                        new FadeText(Game, Vector2.Zero, IncomingServerMessage);
                        break;
                }
            }
        }



        public void process(NetIncomingMessage msgIn, string inMess)
        {
            process(DataHandler.ExtractMessage<TestMess>(inMess));
            process(DataHandler.ExtractMessage<ErrorMess>(inMess));
            process(DataHandler.ExtractMessage<PlayerData>(inMess));
            process(DataHandler.ExtractMessage<LeavingData>(inMess));

        }
        public void process(LeavingData leaving)
        {
            if (leaving == null) return;
            var found = otherPlayers.FirstOrDefault(p => p.playerID == leaving.playerID);
            if (found != null)
                otherPlayers.Remove(found);
            

        }
        private void process(ErrorMess errorMess)
        {
            if (errorMess == null) return;

        }
        private void process(PlayerData playerData)
        {
            if (playerData == null) return;
            switch (playerData.header)
            {

                default:
                    IncomingServerMessage = "Unknown player Data header";
                    break;
                    
            }
        }
        private void process(TestMess testMess)
        {
            if (testMess == null) return;
            else new FadeText(Game, Vector2.Zero, testMess.message); 
        }
    }
}
