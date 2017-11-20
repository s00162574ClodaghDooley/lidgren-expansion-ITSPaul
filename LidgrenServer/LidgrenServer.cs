﻿using GameData;
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
        public static string[] collectableNames = new string[]
        {
            "Badges_0","Badges_1","Badges_2","Badges_3","Badges_4","Badges_5","Badges_6","Badges_7","Badges_8","Badges_9","Badges_10","Badges_11","Badges_12",
        };
        public static List<CollectableData> collectables = new List<CollectableData>()
        {
            new CollectableData
            {
                AssetName = collectableNames[new Random().Next(collectableNames.Count()-1)],
                Value = new Random().Next(10,20),
                X = new Random().Next(0,800),
                Y = new Random().Next(0,600),
            }
        };
        public static List<PlayerData> Players = new List<PlayerData>();
        public static List<PlayerData> RegisteredPlayers = new List<PlayerData>();
        public static GetWorldSize world = new GetWorldSize { X = 800, y = 600 };
        public static NetPeerConfiguration config = new NetPeerConfiguration("magam")
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
            process(DataHandler.ExtractMessage<Initialise>(inMess));
            process(DataHandler.ExtractMessage<MovedData>(inMess));


        }

        private static void process(MovedData movedData)
        {
            if (movedData == null)
                return;
            // Update the player position on the server
            PlayerData player = Players.First(p => p.playerID == movedData.playerID);
            player.X = movedData.toX;
            player.Y = movedData.toY;
            // Tell all the clients the new position
            DataHandler.sendNetMess<MovedData>(server,
           new MovedData
           {
               playerID = movedData.playerID,
               toX = movedData.toX,
               toY = movedData.toY
           },
           SENT.TOALL);


        }

        private static void process(Initialise initialise)
        {
            // Forgot this in the previous commit
            if (initialise == null) return;
            CreatePlayer();
        }

        public static void CreatePlayer()
        {
            // Create a new player with Id of the players
            string playerID = Players.Count().ToString();
            Players.Add(new PlayerData("", "", playerID, "Player " + playerID, 0f, 0f));
            DataHandler.sendNetMess<Joined>(Server.server,
                       new Joined { playerId = playerID, gameTag = "Player " + playerID }, SENT.TOALL);
            // Tell all the clients that the there is a new player and all the players so far

            //Send Collectables
            foreach (CollectableData collectable in collectables)
            {
                DataHandler.sendNetMess<CollectableData>(Server.server, collectable, SENT.TOALL);
            }
            foreach (PlayerData player in Players)
            {
                DataHandler.sendNetMess<Joined>(Server.server,
           new Joined { playerId = player.playerID, gameTag = "Player " + playerID }, SENT.TOALL);

            }


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
