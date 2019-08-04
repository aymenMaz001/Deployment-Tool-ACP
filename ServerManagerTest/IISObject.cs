using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagerTest
{
    public class IISObject

    {
        public long siteId { get; set; }

        public string hostName { get; set; }

        public string webSiteName { get; set; }

        public string portNumber { get; set; }

        public string applicationPool { get; set; }

    }
}
