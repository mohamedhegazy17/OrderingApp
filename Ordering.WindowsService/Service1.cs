using Ordering.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.WindowsService
{
    public partial class Service1 : ServiceBase
    {
         
        public Service1()
        {
            InitializeComponent();
        }
        ServiceHost host;
        protected override void OnStart(string[] args)
        {
            // ...
            try
            {
                
                host = new ServiceHost(typeof(OrderService));
                host.Open();
                //Console.WriteLine("Hit any key to shut down");
                //Console.ReadKey();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //Console.WriteLine("Hit any key to exit");
                //Console.ReadKey();
            }
        }

        protected override void OnStop()
        {
            // ...
            host.Close();
        }
    }
}
