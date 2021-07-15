using Ordering.Client.OrderService;
using Ordering.Entities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Ordering.Client
{
    public partial class Form1 : Form
    {
        OrderServiceClient proxyClient = new OrderServiceClient();
        private static TcpClient o_TcpClient;
        private static NetworkStream o_NetworkStream;
        private Thread o_Thread;

        public bool IsWorking = true;
        private DateTime dtLastRecivedDMMessageTime;

        public Form1()
        {
            InitializeComponent();

            //o_TcpClient = new TcpClient("10.4.4.241", 8097);
            ////o_TcpClient = new TcpClient("10.255.105.85", 8097);
            //o_NetworkStream = o_TcpClient.GetStream();
            //o_Thread = new Thread(new ThreadStart(Listen));
            //o_Thread.IsBackground = true;
            //o_Thread.Start();

        }
        private static void SendMsgToServer(oEnvelop o_oEnvelop)
        {
            try
            {
                if (o_TcpClient.Connected)
                {
                    o_oEnvelop.Serialize(o_NetworkStream);
                }
                else
                {
                    throw new Exception("Connection with DM is closed");
                }
            }
            catch (Exception gXp)
            {
                throw gXp;
            }
        }
        internal static void Authantication(string strUserName, string strPassword)
        {
            try
            {
                var o_oEnvelopMsgToServer = new oEnvelop();
                o_oEnvelopMsgToServer.MessageType = _MessageType.ClientLogIn;
                oClientLogIn o_oClientLogIn = new oClientLogIn();
                o_oClientLogIn.UserName =  strUserName;
                o_oClientLogIn.Password =  strPassword;
                o_oClientLogIn.ClientIP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(); ;
                o_oEnvelopMsgToServer.oMessages.Add(o_oClientLogIn);
                SendMsgToServer(o_oEnvelopMsgToServer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void Listen()

        {
            try
            {
                oEnvelop o_oEnvelop;
                while (IsWorking)
                {
                    while (!o_NetworkStream.DataAvailable && IsWorking)
                        Thread.Sleep(100);
                    if (!IsWorking)
                        break;
                    try
                    {
                        o_oEnvelop = new oEnvelop().Deserialize(o_NetworkStream) as oEnvelop;
                        dtLastRecivedDMMessageTime = DateTime.Now;
                        
                    }
                    catch (Exception gXp)
                    {

                    }
                }
            }
            catch (ThreadAbortException thr_Xp)
            { 
            }
            catch (Exception gXp)
            {

            }
        }

        private async void btnOrder_Click(object sender, EventArgs e)
        {
            string account = txtAccount.Text ?? "Na";
            if (!decimal.TryParse(txtPrice.Text, out decimal price))
                price = 0m;
            if (!int.TryParse(txtQuantity.Text, out int quantity))
                quantity = 0;

            var order = new Order { Account = account, Price = price, Quantity = quantity };
            await proxyClient.AddOrderAsync(order);
            proxyClient.Close();
            //this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Authantication("omsadmin", "1");
        }
    }
}
