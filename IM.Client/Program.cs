using IM.Client.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IM.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string _serverHost = ConfigurationManager.AppSettings["ServerHost"];
            int _serverPort = int.TryParse(ConfigurationManager.AppSettings["ServerPort"], out _serverPort) ? _serverPort : 12304
                , _socketTimeout = int.TryParse(ConfigurationManager.AppSettings["SocketTimeout"], out _socketTimeout) ? _socketTimeout : 3000;

            Application.ApplicationExit += Application_ApplicationExit;
            //启动SocketClient
            SocketClient.Default.Start(_serverHost, _serverPort, _socketTimeout);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            SocketClient.Default.Stop();
        }
    }
}
