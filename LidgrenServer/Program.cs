using GameData;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LidgrenServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server s = new Server();
            for (;;)
            {
                // Stop the fan from going around needlessly
                Server.server.MessageReceivedEvent.WaitOne();
                NetIncomingMessage msgIn;
                while ((msgIn = Server.server.ReadMessage()) != null)
                {
                    //create message type handling with a switch
                    switch (msgIn.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            //This type handles all data that has been sent by you.
                            // broadcast message to all clients
                            var inMess = msgIn.ReadString();
                            Server.process(msgIn, inMess);
                            //NetOutgoingMessage reply = server.CreateMessage();
                            //reply.Write(inMess);
                            //foreach (NetConnection client in server.Connections)
                            //    server.SendMessage(reply, client, NetDeliveryMethod.ReliableOrdered);
                            break;
                        //All other types are for library related events (some examples)
                        case NetIncomingMessageType.DiscoveryRequest:
                            Console.WriteLine("Discovery Request from Client");
                            NetOutgoingMessage msg = Server.server.CreateMessage();
                            //add a string as welcome text
                            msg.Write("Greetings from " + Server.config.AppIdentifier + " server ");
                            //send a response
                            Server.server.SendDiscoveryResponse(msg, msgIn.SenderEndPoint);
                            // Create Player later to ensure proper discovery 
                            break;
                        case NetIncomingMessageType.ConnectionApproval:
                            msgIn.SenderConnection.Approve();
                            break;


                        case NetIncomingMessageType.StatusChanged:

                            switch ((NetConnectionStatus)msgIn.ReadByte())
                            {
                                case NetConnectionStatus.Connected:
                                    Console.WriteLine("{0} Connected", msgIn.SenderConnection);
                                    break;
                                case NetConnectionStatus.Disconnected:
                                    Console.WriteLine("{0} Disconnected", msgIn.SenderConnection);

                                    break;
                                case NetConnectionStatus.RespondedAwaitingApproval:
                                    msgIn.SenderConnection.Approve();
                                    break;
                            }
                            break;
                        default:
                            Console.WriteLine("unhandled message with type: "
                                + msgIn.MessageType);
                            break;
                    }
                    //Recycle the message to create less garbage
                    Server.server.Recycle(msgIn);
                }
            }
        }

    }
}
