using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace ServerManagerTest
{
    class Program
    {

        static void Main(string[] args)
        {
            ServerManager serverManager = new ServerManager();
            DeafultDocument df = new DeafultDocument();
            StartStopServer server = new StartStopServer();
            AddWebSite site = new AddWebSite();
            AppPoolWithSite pool = new AppPoolWithSite();
            Console.WriteLine("Domain Name is : ");
            Console.WriteLine(System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName);
            Console.WriteLine(System.Environment.UserDomainName);
            Console.WriteLine(System.Environment.UserName);
            Console.ReadLine();

            // Start the server
            //server.StartServerTest(serverManager, "Default Web Site"); 

            //Add a pool
            //pool.AddPoolSite(serverManager,"poolTest");

            //Add a WebSite
            //site.Add(serverManager,"TestAdd1","*","8545","*","DefaultAppPool");

            //Stop the server
            //server.StopServerTest(serverManager, "Default Web Site");

            //Get The default Document
            //df.GetDefaultDocument(serverManager);

        }
    }
}
