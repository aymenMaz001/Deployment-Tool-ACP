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

namespace RunBook
{
    /// <summary>
    /// The ViewModel to link the View and the Model
    /// </summary>
    public class TreeViewFileSystemViewModel : ViewModelBase
    {
        /// <summary>
        /// Show a treeview from a default path when opening the application
        /// </summary>
        public TreeViewFileSystemViewModel()
        {
            //var path = @"C:\Release5.0";
            ////var path = @"C:\Users\Aymen\Desktop\Esprit\3B";
            //DirectoryItems = GetItems(path);
            //Text = "Path: " + path;
        }

        private ObservableCollection<FileItem> directoryItems;

        public ObservableCollection<FileItem> DirectoryItems
        {
            get
            {
                return directoryItems;
            }

            set
            {
                directoryItems = value;
                OnPropertyChanged("DirectoryItems");
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

        /// <summary>
        /// Return all the directories, subdirectories and files recursively
        /// </summary>
        /// <param name="path">path of the folder</param>
        /// <returns></returns>
        public ObservableCollection<FileItem> GetItems(string path)
        {
            var FileSystemItem = new ObservableCollection<FileItem>();
            try
            {
                var dirInfo = new DirectoryInfo(path);

                foreach (var directory in dirInfo.EnumerateDirectories())
                {
                    var item = new DirectoryItem
                    {
                        Name = directory.Name,
                        Path = directory.FullName,
                        DirectoryItems = GetItems(directory.FullName)
                    };

                    FileSystemItem.Add(item);
                }

                foreach (var file in dirInfo.EnumerateFiles())
                {
                    var item = new FileItem
                    {
                        Name = file.Name,
                        Path = file.FullName
                    };

                    FileSystemItem.Add(item);
                }
            }
            catch (UnauthorizedAccessException unAuthDirectory)
            {
                MessageBox.Show(unAuthDirectory.Message);
            }
            catch (PathTooLongException longPath)
            {
                MessageBox.Show(longPath.Message);
            }
            return FileSystemItem;
        }


        /// <summary>
        /// Check the .zip folder loaded from an openFileDialog
        /// and extract it in the same path by adding _unzipped suffix
        ///  to its name
        /// </summary>
        /// <param name="fileDialog"></param>
        public void ExtractFolder(ref OpenFileDialog fileDialog)
        {
            if (Path.GetExtension(fileDialog.FileName).Equals(".ZIP", StringComparison.OrdinalIgnoreCase))
            {
                var fileNameFullPath = fileDialog.FileName;
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileDialog.FileName);
                var DirectoryFullPath = Path.GetDirectoryName(fileNameFullPath);
                var extractedFolderPath = $"{DirectoryFullPath}\\{FileNameWithoutExtension}_unzipped";

                if (!Directory.Exists(extractedFolderPath))
                {
                    ZipFile.ExtractToDirectory(fileNameFullPath, extractedFolderPath);
                    DirectoryItems = GetItems(extractedFolderPath);
                }
                else
                {
                    MessageBox.Show("The File already exist in the same path");
                }
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
