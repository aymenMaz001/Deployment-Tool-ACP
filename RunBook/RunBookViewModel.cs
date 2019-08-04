using Microsoft.Win32;
using RunBook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace RunBook
{
    public class RunBookViewModel : ViewModelBase
    {
        public RunBookViewModel()
        {
            ListEntries.Add(new MyData() { Id = 1, Action = "StopIIS", Description = "", Destination = "", Server = "192.168.2.152", Site = "All", Source = "" });
            ListEntries.Add(new MyData() { Id = 2, Action = "Copy", Description = "", Destination = @"\Reports", Server = "192.168.2.152", Site = "AxeCreditPortal", Source = @"axeDispatchField.xml" });
            ListEntries.Add(new MyData() { Id = 3, Action = "Copy", Description = "Copying formats to app server", Destination = @"\Axe_Credit", Server = "192.168.2.152", Site = "AcpWebServices", Source = "DocGen.docx" });
            ListEntries.Add(new MyData() { Id = 4, Action = "Execute", Description = "Executing DB scripts", Destination = @"\Reports", Server = "192.168.2.152", Site = "", Source = "DocGen.docx" });
            ListEntries.Add(new MyData() { Id = 5, Action = "Execute", Description = "", Destination = "Axe_Credit", Server = "192.168.2.152", Site = "", Source = "DocGen.docx" });
            //LoadXmlConfig(@"C:\output.xml");
            cbList.Add("StopIIS");
            cbList.Add("Copy");
            cbList.Add("Execute");
            cbList.Add("Execute");

        }
        public static string EmptyField = "Empty Field"; // Write value if a field value is null or empty
        private int id;
        private string action;
        private string server;
        private string source;
        private string destination;
        private string description;
        private string site;
        private MyData selectedAction;
        private bool isBtnVisible;
        private bool isBtnAddVisible;
        private bool isDetailsVisible;
        private bool isFormVisible;

        private ObservableCollection<string> cbList = new ObservableCollection<string>();
        public ObservableCollection<string> CbList
        {
            get
            {
                return cbList;
            }

            set
            {
                cbList = value;
                OnPropertyChanged("CbList");
            }
        }


        private ObservableCollection<MyData> listEntries = new ObservableCollection<MyData>();

        public ObservableCollection<MyData> ListEntries
        {
            get
            {
                return listEntries;
            }

            set
            {
                listEntries = value;
                
                OnPropertyChanged("ListEntries");
            }
        }


        /// <summary>
        /// Method to update automatically the id column
        /// </summary>
        private void UpdateIdList()
        {
            var i = 0;
            foreach (var item in ListEntries)
            {
                item.Id = ++i;
            }
        }

        /// <summary>
        /// Method to load the configuration file from
        /// an XML file and bind it to the datagrid
        /// </summary>
        /// <param name="path"></param>
        private void LoadXmlConfig(string path)
        {
            XmlSerializer serialiser = new XmlSerializer(ListEntries.GetType());
            // Create a new file stream for reading the XML file
            FileStream ReadFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Load the object saved above by using the Deserialize function
            var LoadedConfig = (ObservableCollection<MyData>)serialiser.Deserialize(ReadFileStream);
            ListEntries = LoadedConfig;
            // Cleanup
            ReadFileStream.Close();
        }

        private ICommand addEntry;
        public ICommand AddEntry
        {
            get
            {
                if(addEntry == null)
                {
                    addEntry = new RelayCommand<object>((obj) =>
                    {
                        ListEntries.Add(new MyData() { Id = Id, Action = Action, Description = Description, Destination = Destination, Server = Server, Site = Site, Source = Source });
                        IsOpenDialog = false;


                    });
                }
                return addEntry;
            }
        }

        private ICommand viewDetails;

        public ICommand ViewDetails
        {
            get
            {
                if(viewDetails == null)
                {
                    viewDetails = new RelayCommand<object>((obj) =>
                    {
                        IsOpenDialog = true;
                        IsDetailsVisible = true;
                        IsFormVisible = false;

                        Action = string.IsNullOrEmpty(SelectedAction.Action) ? ("Empty Field") : SelectedAction.Action;
                        Server = string.IsNullOrEmpty(SelectedAction.Server) ? ("Empty Field") : SelectedAction.Server;
                        Source = string.IsNullOrEmpty(SelectedAction.Source) ? ("Empty Field") : SelectedAction.Source;
                        Destination = string.IsNullOrEmpty(SelectedAction.Destination) ? ("Empty Field") : SelectedAction.Destination;
                        Description = string.IsNullOrEmpty(SelectedAction.Description) ? ("Empty Field") : SelectedAction.Description;
                        Site = string.IsNullOrEmpty(SelectedAction.Site) ? ("Empty Field") : SelectedAction.Site;
                        
                    });
                }
                return viewDetails;
            }
        }

        private ICommand openAddDialog;
        public ICommand OpenAddDialog
        {
            get
            {
                if (openAddDialog == null)
                {
                    openAddDialog = new RelayCommand<object>((obj) =>
                    {
                        IsOpenDialog = true;
                        IsBtnAddVisible = true;
                        IsBtnVisible = false;
                        IsDetailsVisible = false;
                        IsFormVisible = true;


                        Action = "";
                        Server = "";
                        Source = "";
                        Destination = "";
                        Description = "";
                    });
                }
                return openAddDialog;
            }

        }

        private ICommand openEditDialog;
        public ICommand OpenEditDialog
        {
            get
            {
                if (openEditDialog == null)
                {
                    openEditDialog = new RelayCommand<object>((obj) =>
                    {
                        IsOpenDialog = true;
                        IsBtnAddVisible = false;
                        IsBtnVisible = true;
                        IsFormVisible = true;
                        IsDetailsVisible = false;

                        Action =  SelectedAction.Action;
                        Server = SelectedAction.Server;
                        Description = SelectedAction.Description;
                        Source = SelectedAction.Source;
                        Destination = SelectedAction.Destination;

                    });
                }
                return openEditDialog;
            }
        }

        private ICommand deleteEntry;

        public ICommand DeleteEntry
        {
            get
            {
                if (deleteEntry == null)
                {
                    deleteEntry = new RelayCommand<object>((obj) =>
                    {
                        ListEntries.Remove(SelectedAction);
                        UpdateIdList();
                    });
                }
                return deleteEntry;
            }
        }

        private ICommand persistToXml;

        public ICommand PersistToXml
        {
            get
            {
                if (persistToXml == null)
                {
                    persistToXml = new RelayCommand<object>((obj) =>
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "XML File (*.xml) | *.xml";
                        saveFileDialog.DefaultExt = "xml";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            XmlSerializer serialiser = new XmlSerializer(ListEntries.GetType());
                            TextWriter Filestream = new StreamWriter(saveFileDialog.FileName);
                            //write to the file
                            serialiser.Serialize(Filestream, ListEntries);
                            // Close the file
                            Filestream.Close();
                        }


                    });
                    
                }
                return persistToXml;
            }
        }

        private ICommand readXmlFile;
        public ICommand ReadXmlFile
        {
            get
            {
                if (readXmlFile == null)
                {
                    readXmlFile = new RelayCommand<object>((obj) =>
                    {
                        var uploadConfigXml = new OpenFileDialog();
                        uploadConfigXml.Filter = "XML Files (*.xml)|*.xml";
                        uploadConfigXml.ShowDialog();
                    });
                }
                        return readXmlFile;
            }
        }

        public MyData SelectedAction
        {
            get
            {
                return selectedAction;
            }

            set
            {
                selectedAction = value;
                OnPropertyChanged("SelectedAction");
            }
        }

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

        public string Site
        {
            get
            {
                return site;
            }

            set
            {
                site = value;
                OnPropertyChanged("Site");
            }
        }

        public int Id
        {
            get
            {
                id = ListEntries.Count() + 1;
                //id = listEntries.IndexOf(SelectedAction);
                return id;
            }

            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public bool IsBtnVisible
        {
            get
            {
                return isBtnVisible;
            }

            set
            {
                isBtnVisible = value;
                OnPropertyChanged("IsBtnVisible");
            }
        }

        public bool IsBtnAddVisible
        {
            get
            {
                return isBtnAddVisible;
            }

            set
            {
                isBtnAddVisible = value;
                OnPropertyChanged("IsBtnAddVisible");
            }
        }

        private bool isOpenDialog;

        public bool IsOpenDialog
        {
            get
            {
                return isOpenDialog;
            }

            set
            {
                isOpenDialog = value;
                OnPropertyChanged("IsOpenDialog");
            }
        }

        public bool IsDetailsVisible
        {
            get
            {
                return isDetailsVisible;
            }

            set
            {
                isDetailsVisible = value;
                OnPropertyChanged("IsDetailsVisible");
            }
        }

        public bool IsFormVisible
        {
            get
            {
                return isFormVisible;
            }

            set
            {
                isFormVisible = value;
                OnPropertyChanged("IsFormVisible");
            }
        }
    }
}
