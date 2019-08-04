using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagerTest
{
    public class DeafultDocument
    {

        public void GetDefaultDocument(ServerManager serverManager)
        {

            Configuration appHostConfig = serverManager.GetApplicationHostConfiguration();
            ConfigurationSection defaultDocumentSection = appHostConfig.GetSection("system.webServer/defaultDocument");
            foreach (ConfigurationElement element in defaultDocumentSection.GetCollection("files"))
            {
                Console.WriteLine(element.GetAttribute("value").Value);
            }
            Console.ReadLine();
        }
    }

}
