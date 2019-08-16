using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBook
{
    public class ServerManagerACP
    {
        public void StartServer(ServerManager serverManager, String name)
        {
            var site = serverManager.Sites.FirstOrDefault(s => s.Name == name);
            if (site != null)
            {
                site.Start();
                Console.WriteLine("The Server Has been started");
            }
            Console.ReadLine();
        }

        public void StopServer(ServerManager serverManager, String name)
        {
            var site = serverManager.Sites.FirstOrDefault(s => s.Name == name);
            if (site != null)
            {
                site.Stop();
                Console.WriteLine("The Server Has been stopped");
            }
            Console.ReadLine();
        }

        public void Add(ServerManager server,string sitePath, String name, string ip, string port, string hostName, string poolName)
        {

            if (server.Sites != null && server.Sites.Count > 0)
            {
                //we will first check to make sure that the site isn't already there
                if (server.Sites.FirstOrDefault(s => s.Name == name) == null)
                {
                    //we will just pick an arbitrary location for the site
                    //string path = @"c:\TestServer\";
                    string path = sitePath;

                    string bindingInfo = string.Format(@"{0}:{1}:{2}", ip, port, hostName);

                    //add the new Site to the Sites collection
                    Site site = server.Sites.Add(name, "http", bindingInfo, path);

                    //set the ApplicationPool for the new Site
                    site.ApplicationDefaults.ApplicationPoolName = poolName; //myApplicationPool.Name;

                    //save the new Site!
                    server.CommitChanges();
                    Console.WriteLine("Site has been created");
                }
            }
            Console.ReadLine();
        }
    }
}
