using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Server
{
    public partial class Service1 : ServiceBase
    {
        clsTcpServer o_clsServer;

        private readonly Logger m_Logger = LogManager.GetCurrentClassLogger();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                clsTcpServer clsTcpServer = new clsTcpServer();
                clsTcpServer.ListenToQueue();
                m_Logger.Info($"Starting {this.ServiceName}....");

                m_Logger.Debug($"Initializing service {this.ServiceName}...");
                Initialize();

                m_Logger.Info($"{this.ServiceName} service started successfully!");
            }
            catch (Exception gXp)
            {
                m_Logger.Fatal(gXp,$"Fatal error occurred while starting service {this.ServiceName}. Service will be stopped!");

                try
                {
                    this.Stop();
                }
                catch (Exception ex)
                {
                    m_Logger.Fatal(ex,$"Failed to stop service {this.ServiceName}.");
                }
            }
        }

        private void Initialize()
        {
            m_Logger.Debug($"Constructing clsServer in service {this.ServiceName}");
            o_clsServer = new clsTcpServer();
        }

        protected override void OnStop()
        {
            try
            {
                m_Logger.Info("Stopping service {0}...", this.ServiceName);

               // o_clsServer.Close();

                m_Logger.Info("Service {0} stopped successfully", this.ServiceName);
            }
            catch (Exception gXp)
            {
                m_Logger.Fatal(gXp,$"An error occurred while stopping service {this.ServiceName}");
            }
        }
    }
}
