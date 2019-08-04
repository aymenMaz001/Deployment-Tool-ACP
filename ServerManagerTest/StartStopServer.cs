using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagerTest
{
    public class StartStopServer
    {
       
        public void StartServerTest(ServerManager serverManager, String name)
        {
            var site = serverManager.Sites.FirstOrDefault(s => s.Name == name);
            if (site != null)
            {
                //stop the site
                //site.Stop();
                //start the site
                site.Start();
                Console.WriteLine("The Server Has been started");
            }
            Console.ReadLine();
        }

        public void StopServerTest(ServerManager serverManager, String name)
        {
            var site = serverManager.Sites.FirstOrDefault(s => s.Name == name);
            if (site != null)
            {
                //stop the site
                site.Stop();
                //start the site
                //site.Start();
                Console.WriteLine("The Server Has been stopped");
            }
            Console.ReadLine();
        }
    }
}
