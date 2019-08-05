using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBook
{
    public class MyData : ViewModelBase
    {
        private int id;
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        private string action;
        public string Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
                OnPropertyChanged("Action");
            }
        }
        private string server;
        public string Server
        {
            get
            {
                return server;
            }

            set
            {
                server = value;
                OnPropertyChanged("Server");
            }
        }
        

        private string source;
        public string Source
        {
            get
            {
                return source;
            }

            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        private string destination;
        public string Destination
        {
            get
            {
                return destination;
            }

            set
            {
                destination = value;
                OnPropertyChanged("Destination");
            }
        }
        private string description;
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        private string sourceFolderPath;

        public string SourceFolderPath
        {
            get
            {
                return sourceFolderPath;
            }

            set
            {
                sourceFolderPath = value;
                OnPropertyChanged("SourceFolderPath");
            }
        }
        public string Site { get; set; }
    }
}
