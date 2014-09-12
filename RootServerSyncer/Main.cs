using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RootServerSyncer
{
    public partial class Main : ServiceBase
    {

        private EventLog eventLog;
        public Aera.FileDB datadb;
        public Aera.FileDB hosts;
        public Aera.FileDB newhosts;
        public int mins = 60;
        public string _mins = "60";
        public int totalSync = 0;
        public System.Threading.Thread client;
        public bool running = false;
        public string inifile = "aera.ini";
        public string syncurl = "http://pub.haztech.com.my/data.txt";

        public string ires = "";
        public string ores = "";

        public Main()
        {
            InitializeComponent();

            this.ServiceName = "RootSyncer";

            string source = "Main";
            eventLog = new EventLog();
            eventLog.Source = source;

        }


        public void cliThread()
        {
            int num = 1;
            int success = 0;
            int fail = 0;
            while (running)
            {
                ores = "";
                ires = "";
                success = 0;
                fail = 0;
                try
                {
                    syncurl = INI.GetIniValue("sync", "url", inifile);
                    _mins = INI.GetIniValue("sync", "minute_sync", inifile);
                    try
                    {
                        mins = Convert.ToInt32(_mins);
                    }
                    catch
                    {
                    }
                    datadb = new Aera.FileDB(new Uri(@"" + syncurl));
                    ores = datadb.ReadLine(0);
                    datadb.Dispose();
                    INI.WriteIniValue("sync", "ores", ores, inifile);

                    hosts = new Aera.FileDB(@"c:\\Windows\\System32\\drivers\\etc\\hosts");
                    newhosts = new Aera.FileDB();
                    newhosts.DB = new List<string>();
                    foreach (string _ires in hosts.DB)
                    {
                        newhosts.DB.Add(_ires);
                    }
                    newhosts.Save("test.txt");

                    success++;
                }
                catch
                {
                    fail++;
                }
                num++;
                System.Threading.Thread.Sleep(mins * 50000);
            }
        }
        public void startProgress()
        {
            try
            {
                running = true;
                client = new System.Threading.Thread(new System.Threading.ThreadStart(cliThread));
            }
            catch
            {
                client.Abort();
            }
        }


        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("starting up!");
            try
            {

                hosts = new Aera.FileDB(@"c:\\Windows\\System32\\drivers\\etc\\hosts");
                newhosts = new Aera.FileDB();
                newhosts.DB = new List<string>();
                foreach (string _ires in hosts.DB)
                {
                    newhosts.DB.Add(_ires);
                }
                newhosts.Save("test.txt");

                //startProgress();
            }
            catch { }
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("shutting down!");
            running = false;
            try
            {
                client.Abort();
            }
            catch { }
        }
    }
}
