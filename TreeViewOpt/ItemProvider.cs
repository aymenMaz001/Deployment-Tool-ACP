using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TreeViewOpt
{
    public class ItemProvider : ViewModelBase
    {
        public ItemProvider()
        {
            var path = @"C:\Release5.0";
            //var path = @"C:\Users\Aymen\Desktop\Esprit\3B";
            Items = GetItems(path);
            Text = "Path: "+path;
        }

        public ObservableCollection<Item> items;

        public ObservableCollection<Item> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        private string text;

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        private ICommand browseFile;
        public ICommand BrowseFile
        {
            get
            {
                if (browseFile == null)
                {
                    browseFile = new RelayCommand<object>((obj) =>
                    {
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.ShowDialog();
                        Text = fileDialog.FileName;
                        ExtractFolder(ref fileDialog);
                    });
                }
                return browseFile;
            }
        }

        public ObservableCollection<Item> GetItems(string path)
        {
            var items = new ObservableCollection<Item>();
         
            var dirInfo = new DirectoryInfo(path);

            foreach (var directory in dirInfo.EnumerateDirectories())
            {
                var item = new DirectoryItem
                {
                    Name = directory.Name,
                    Path = directory.FullName,
                    Items = GetItems(directory.FullName)
                };

                items.Add(item);
            }

            foreach (var file in dirInfo.EnumerateFiles())
            {
                var item = new Item
                {
                    Name = file.Name,
                    Path = file.FullName
                };

                items.Add(item);
            }

            return items;
        }

        public void ExtractFolder(ref OpenFileDialog fileDialog)
        {
            if (Path.GetExtension(fileDialog.FileName).Equals(".ZIP", StringComparison.OrdinalIgnoreCase))
            {
                var fileNameFullPath = fileDialog.FileName;
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileDialog.FileName);
                var DirectoryFullPath = Path.GetDirectoryName(fileNameFullPath);
                var extractedFolderPath = $"{DirectoryFullPath}\\{FileNameWithoutExtension}_unzipped";

                ZipFile.ExtractToDirectory(fileNameFullPath, extractedFolderPath);
                Items = GetItems(extractedFolderPath);
            }
            else if (string.IsNullOrEmpty(fileDialog.FileName))
            {
                // Closing the foleDialog without choosing any file.
            }
            else
            {
                // Choosing a file whose extension is not .zip
                MessageBox.Show("Could Not handle this file\n Only .zip are allowed");
            }
        }
    }
}
