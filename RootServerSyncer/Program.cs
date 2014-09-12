using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RootServerSyncer
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            /*
             * //Installs and starts the service
ServiceInstaller.InstallAndStart("MyServiceName", "MyServiceDisplayName", "C:\PathToServiceFile.exe");

//Removes the service
ServiceInstaller.Uninstall("MyServiceName");

//Checks the status of the service
ServiceInstaller.GetServiceStatus("MyServiceName");

//Starts the service
ServiceInstaller.StartService("MyServiceName");

//Stops the service
ServiceInstaller.StopService("MyServiceName");

//Check if service is installed
ServiceInstaller.ServiceIsInstalled("MyServiceName");*/

            System.Console.WriteLine("Failed xxxaaa");
            if (args[0] == "install")
            {

                try
                {
                    ServiceInstaller.InstallAndStart("RootSyncer", "RootSyncer", System.Reflection.Assembly.GetExecutingAssembly().Location);
                    //ServiceInstaller.InstallAndStart("MyServiceName", "MyServiceDisplayName", System.Reflection.Assembly.GetExecutingAssembly().Location + System.AppDomain.CurrentDomain.FriendlyName);
                    System.Console.WriteLine("Successfully installed");
                }
                catch
                {
                    System.Console.WriteLine("Failed to install");
                }
            }
            else if (args[0] == "uninstall")
            {
                try
                {
                    ServiceInstaller.Uninstall("RootSyncer");
                    Console.WriteLine("Successfully uninstalled");
                }
                catch
                {
                    Console.WriteLine("Failed to uninstall");
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new Main() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
