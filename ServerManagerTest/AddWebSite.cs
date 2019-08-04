using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagerTest
{
    public class AddWebSite
    {
        public void Add(ServerManager server,String name)
        {

            if (server.Sites != null && server.Sites.Count > 0)
            {
                //we will first check to make sure that the site isn't already there
                if (server.Sites.FirstOrDefault(s => s.Name == name) == null)
                {
                    //we will just pick an arbitrary location for the site
                    string path = @"c:\TestServer\";

                    //we must specify the Binding information
                    string ip = "*";
                    string port = "8000";
                    string hostName = "*";

                    string bindingInfo = string.Format(@"{0}:{1}:{2}", ip, port, hostName);

                    //add the new Site to the Sites collection
                    Site site = server.Sites.Add(name, "http", bindingInfo, path);

                    //set the ApplicationPool for the new Site
                    site.ApplicationDefaults.ApplicationPoolName = "DefaultAppPool"; //myApplicationPool.Name;

                    //save the new Site!
                    server.CommitChanges();
                    Console.WriteLine("Site has been created");
                }
            }
            Console.ReadLine();
        }
    }
}
