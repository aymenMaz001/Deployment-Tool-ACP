using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBook
{
    public class DirectoryItem : FileItem
    {
        public ObservableCollection<FileItem> DirectoryItems { get; set; }

        public DirectoryItem()
        {
            DirectoryItems = new ObservableCollection<FileItem>();

        }
    }
}
