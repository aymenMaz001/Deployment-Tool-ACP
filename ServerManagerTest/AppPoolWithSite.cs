using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagerTest
{
    public class AppPoolWithSite
    {
        public void AddPoolSite(ServerManager serverManager,string name) {
            Configuration config = serverManager.GetApplicationHostConfiguration();
            ConfigurationSection applicationPoolsSection = config.GetSection("system.applicationHost/applicationPools");
            ConfigurationElementCollection applicationPoolsCollection = applicationPoolsSection.GetCollection();
            ConfigurationElement addElement = applicationPoolsCollection.CreateElement("add");
            addElement["name"] = name;
            ConfigurationElement recyclingElement = addElement.GetChildElement("recycling");
            ConfigurationElement periodicRestartElement = recyclingElement.GetChildElement("periodicRestart");
            ConfigurationElementCollection scheduleCollection = periodicRestartElement.GetCollection("schedule");
            ConfigurationElement addElement1 = scheduleCollection.CreateElement("add");
            addElement1["value"] = TimeSpan.Parse("03:00:00");
            scheduleCollection.Add(addElement1);
            applicationPoolsCollection.Add(addElement);
            serverManager.CommitChanges();
            Console.WriteLine("Pool added successfully");
            Console.ReadLine();
        }
}
}
