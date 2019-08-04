using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeViewOpt
{
    public class DirectoryItem : Item
    {
        public ObservableCollection<Item> Items { get; set; }

        public DirectoryItem()
        {
            Items = new ObservableCollection<Item>();
        }
    }
}
