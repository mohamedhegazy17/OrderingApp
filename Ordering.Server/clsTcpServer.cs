using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using NLog;
using Ordering.Server.Contract;
using System.Messaging;

namespace Ordering.Server
{
    public class clsTcpServer
    {


        static TcpListener o_TcpListener;
        Thread o_ServerListenerThread;
        private static readonly Logger m_Logger = LogManager.GetCurrentClassLogger();

        private bool IsWorking = true;
        private static NetworkStream o_NetworkStream;
        private Thread o_ThreadClientListener;
        public static string queueSrc;
        public static MessageQueue msgQueue;
        public clsTcpServer()
        {
            o_TcpListener = new TcpListener(IPAddress.Parse("10.4.4.138"), 8097);

            o_ServerListenerThread = new Thread(new ThreadStart(this.Listen));
            o_ServerListenerThread.Name = "OMS Server main listen";
            o_ServerListenerThread.Start();
            o_ServerListenerThread.IsBackground = true;
        }

        private void Listen()
        {
            o_TcpListener.Start();
            while (IsWorking)
            {
                try
                {
                    Socket o_Socket = o_TcpListener.AcceptSocket();
                    if (o_Socket.Connected)
                    {
                        string SessionID = Guid.NewGuid().ToString();

                        o_NetworkStream = new NetworkStream(o_Socket);

                        o_ThreadClientListener = new Thread(this.ClientListener);
                        o_ThreadClientListener.IsBackground = true;
                        o_ThreadClientListener.Start();
                    }
                }
                catch (Exception gXp)
                {
                    m_Logger.ErrorException("An error occurred while accepting/starting DistributionManager session", gXp);
                }
            }
        }
        public static void SendMsgToClient(o_orderData orderData)
        {
            try
            {
                Socket o_Socket = o_TcpListener.AcceptSocket();
                if (o_Socket.Connected)
                {
                    o_NetworkStream = new NetworkStream(o_Socket);
                    m_Logger.Trace($"server is sending new message to client  orderData.Account: {orderData.Account}");
                    orderData.Serialize(o_NetworkStream);
                }
                else
                {
                    throw new Exception("Connection with client is closed");
                }
            }
            catch (Exception gXp)
            {
                throw gXp;
            }
        }

        public static void ListenToQueue()
        {
            queueSrc = ConfigurationManager.AppSettings["queueSrc"].ToString();
            msgQueue = new MessageQueue(queueSrc);
            //msgQueue = new MessageQueue(@".\private$\orders2");
            msgQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(o_orderData) });

            msgQueue.PeekCompleted += new PeekCompletedEventHandler(msgQueue_PeekCompleted);
            msgQueue.BeginPeek();
        }
        //public static void AddItemsToQueue(Message message)
        //{
        //    Queue<Message> evtLogQueue = new Queue<Message>();
        //    evtLogQueue.Enqueue(message);

        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("Log");
        //    foreach (var log in evtLogQueue)
        //    {
        //        DataRow dr = dataTable.NewRow();
        //        dr["Log"] = log;
        //        dataTable.Rows.Add(dr);
        //    }
        //    evtLogQueue.Clear();     // Most probably you will also need to clear the queue.


        //}
        private static void msgQueue_PeekCompleted(object source, PeekCompletedEventArgs e)
        {
            try
            {
                MessageQueue messageQueue = (MessageQueue)source;

                Message message = messageQueue.EndPeek(e.AsyncResult);

               Console.WriteLine("message recieved on Queue {0}, containing {1}", queueSrc, ((o_orderData)message.Body).Account);
         //       AddItemsToQueue(message);
                SendMsgToClient((o_orderData)message.Body);

                messageQueue.Receive();

            }
            catch (Exception)
            {


            }

            msgQueue.BeginPeek();

        }


        public void ClientListener()
        {
            try
            {
                while (IsWorking)
                {
                    while (!o_NetworkStream.DataAvailable && IsWorking)
                        Thread.Sleep(100);
                    if (!IsWorking)
                        break;

                    oEnvelop o_oEnvelop = null;

                    try
                    {
                        o_oEnvelop = (oEnvelop)new oEnvelop().Deserialize(o_NetworkStream);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }


                    m_Logger.Debug("Received message: {0}", o_oEnvelop.MessageType);
                }
            }
            catch (ThreadAbortException taXp)
            {
                m_Logger.WarnException("An error occurred while aborting DistributionManager listener thread", taXp);
            }
            catch (Exception gXp)
            {
                m_Logger.FatalException("An unknown error occurred while aborting DistributionManager listener thread", gXp);
            }
        }
    }
}