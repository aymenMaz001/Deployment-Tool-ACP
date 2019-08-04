using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            ////Unzip Folder
            //string zipPath = @"c:/test.zip";
            //string extractPath = @"c:/testExtracted";
            //ZipFile.ExtractToDirectory(zipPath, extractPath);



            //var path = @"c:/Release5.0/";
            var path = @"C:\Users\Aymen\Desktop";
            string[] allfiles = Directory.GetFileSystemEntries(path, "*.*", SearchOption.AllDirectories);
            foreach (var file in allfiles)
            {
                Console.WriteLine(file);
            }
            Console.ReadLine();
        }
    }
}
