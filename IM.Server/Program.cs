using IM.Server.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IM.Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string _listeningHost = ConfigurationManager.AppSettings["ListeningHost"];
            int _listeningPort = int.TryParse(ConfigurationManager.AppSettings["ListeningPort"], out _listeningPort) ? _listeningPort : 12304
                , _socketTimeout = int.TryParse(ConfigurationManager.AppSettings["SocketTimeout"], out _socketTimeout) ? _socketTimeout : 3000;

            Application.ApplicationExit += Application_ApplicationExit;
            SocketServer.Default.Start(_listeningHost, _listeningPort, _socketTimeout);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            SocketServer.Default.Stop();
        }
    }
}
