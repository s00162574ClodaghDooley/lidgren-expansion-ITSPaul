using GameComponentNS;
using GameData;
using Lidgren.Network;
using LidgrenClient.Game_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LidgrenClient
{
    public class LidgrenGameClient : GameComponent
    {
        public static PlayerData playerData;
        public static List<PlayerData> otherPlayers = new List<PlayerData>();
        public static  NetPeerConfiguration ClientConfig;
        public static NetClient client;

        public static string IncomingServerMessage;

        public LidgrenGameClient(Game game) : base(game)
        {
            game.Components.Add(this);
            ClientConfig = new NetPeerConfiguration("ppMyGame");
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
                        DataHandler.sendNetMess<Initialise>(client,
                       new Initialise { message = "Client connected" }, SENT.FROMCLIENT);
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
            process(DataHandler.ExtractMessage<Joined>(inMess));

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
        private void process(Joined jMessage)
        {
            // The player Data maintained by the client shoudl eb kept in sync with 
            // The player Data of the game player object
            if (playerData == null)
            {
                // Create a new player component 
                Player p = new Player(Game,
                    Game.Content.Load<Texture2D>("Player"),
                    Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2());
                // Fill in the new player Component Data
                p.playerData = new PlayerData("Created", "Player",
                                jMessage.playerId, jMessage.gameTag,
                                p.Position.X, p.Position.Y);
                // set this players player Data
                playerData = p.playerData;
            }
            else
            { // We have another player
                if (playerData.playerID != jMessage.playerId)
                {
                    if (otherPlayers.FirstOrDefault(p => p.playerID == jMessage.playerId) == null)
                    {
                        // Create a position for the player
                        Vector2 otherPos = new Vector2(Game.GraphicsDevice.Viewport.Bounds.Center.X,
                                                        Game.GraphicsDevice.Viewport.Bounds.Center.Y);
                        // Other players created at the centre also
                        // Create the player Data
                        PlayerData data = new PlayerData("Other", "Player",
                                        jMessage.playerId, jMessage.gameTag,
                                        otherPos.X, otherPos.Y);
                        // Create the game player for the other player
                        OtherPlayer p = new OtherPlayer(Game,
                        Game.Content.Load<Texture2D>("Player"),
                        Game.GraphicsDevice.Viewport.Bounds.Center.ToVector2());
                        p.playerData = data;
                        // Remember the other players
                        otherPlayers.Add(data);
                        new FadeText(Game, Vector2.Zero, jMessage.gameTag + " has Joined the game");
                    }
                }
            }
        }

    }
}
