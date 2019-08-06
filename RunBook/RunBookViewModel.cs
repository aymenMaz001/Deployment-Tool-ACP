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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RunBook
{
    public class RunBookViewModel : ViewModelBase
    {
        public RunBookViewModel()
        {
            
            //modifiy original file
            //xmlFilePth = @"..\..\XmlConfigurations\Configurations.xml";
            //Modify file in debug
            xmlFilePth = @"XmlConfigurations\Configurations.xml";
            //ReadConfiguration("1");
            string gg = new Guid("fc43f1b7-0b68-4538-a7a8-a07877cb8001").ToString("D");
            //ReadConfiguration("5");
            ReadConfiguration(gg);
            ListConfigurations = configurationListName();
            cbList.Add("StopIIS");
            cbList.Add("Copy");
            cbList.Add("Execute Sql");
            cbList.Add("Execute Cmd");
            //Default selected configuration
            SelectedConfiguration = ListConfigurations[0];
            
        }
        // Write value if a field value is null or empty
        public static string EmptyField = "Empty Field";
        //Xml File Path
        private string xmlFilePth;
        //Variable to be bound with the view
        private int id;
        private string action;
        private string server;
        private string source;
        private string sourceFolderPath;
        private string destination;
        private string description;
        private string site;
        private MyData selectedAction;
        private string selectedConfiguration;
        private string newConfigurationName;
        //boolean variables to controls visibility
        private bool isBtnVisible;
        private bool isBtnAddVisible;
        private bool isDetailsVisible;
        private bool isFormVisible;
        private bool isSaveNameVisible;

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

        private ObservableCollection<string> listConfigurations = new ObservableCollection<string>();
        public ObservableCollection<string> ListConfigurations
        {
            get
            {
                return listConfigurations;
            }

            set
            {
                listConfigurations = value;
                OnPropertyChanged("ListConfigurations");
            }
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
                        ListEntries.Add(new MyData() { Id = Id, Action = Action, Description = Description, Destination = Destination, Server = Server, Site = Site, Source = Source,SourceFolderPath=SourceFolderPath });
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
                        IsSaveNameVisible = false;

                        Action = string.IsNullOrEmpty(SelectedAction.Action) ? ("Empty Field") : SelectedAction.Action;
                        Server = string.IsNullOrEmpty(SelectedAction.Server) ? ("Empty Field") : SelectedAction.Server;
                        Source = string.IsNullOrEmpty(SelectedAction.Source) ? ("Empty Field") : SelectedAction.Source;
                        Destination = string.IsNullOrEmpty(SelectedAction.Destination) ? ("Empty Field") : SelectedAction.Destination;
                        Description = string.IsNullOrEmpty(SelectedAction.Description) ? ("Empty Field") : SelectedAction.Description;
                        SourceFolderPath = string.IsNullOrEmpty(SelectedAction.SourceFolderPath) ? ("Empty Field") : SelectedAction.SourceFolderPath;
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
                        IsSaveNameVisible = false;


                        Action = "";
                        Server = "";
                        Source = "";
                        Destination = "";
                        Description = "";
                        SourceFolderPath = "";
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
                        IsSaveNameVisible = false;

                        Action =  SelectedAction.Action;
                        Server = SelectedAction.Server;
                        Description = SelectedAction.Description;
                        Source = SelectedAction.Source;
                        Destination = SelectedAction.Destination;
                        SourceFolderPath = SelectedAction.SourceFolderPath;

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

        /// <summary>
        /// Generate an XML Configuration from the datagrid data
        /// </summary>
        private ICommand saveConfiguration;
        public ICommand SaveConfiguration
        {
            get
            {
                if (saveConfiguration == null)
                {
                    saveConfiguration = new RelayCommand<object>((obj) =>
                    {
                        var configurationFile = XDocument.Load(xmlFilePth);
                        var rootConfigurations = configurationFile.Descendants("Configurations").FirstOrDefault();
                        var newConfigurationNode = new XElement("Configuration");
                        
                        newConfigurationNode.Add(new XAttribute("name", NewConfigurationName));
                        newConfigurationNode.Add(new XAttribute("confId",  Guid.NewGuid()));
                        foreach (var node in ListEntries)
                        {
                            var newMydataNode = new XElement("MyData");
                            var newIdNode = new XElement("Id", node.Id);
                            var newActionNode = new XElement("Action", node.Action);
                            var newServerNode = new XElement("Server", node.Server);
                            var newSourceNode = new XElement("Source", node.Source);
                            var newDestinationNode = new XElement("Destination", node.Destination);
                            var newDescriptionNode = new XElement("Description", node.Description);
                            var newSourceFolderPathNode = new XElement("SourceFolderPath", node.SourceFolderPath);
                            var newSiteNode = new XElement("Site", node.Site);
                            newMydataNode.Add(newIdNode);
                            newMydataNode.Add(newActionNode);
                            newMydataNode.Add(newServerNode);
                            newMydataNode.Add(newSourceNode);
                            newMydataNode.Add(newDestinationNode);
                            newMydataNode.Add(newDescriptionNode);
                            newMydataNode.Add(newSourceFolderPathNode);
                            newMydataNode.Add(newSiteNode);
                            newConfigurationNode.Add(newMydataNode);
                        }
                        
                        rootConfigurations.Add(newConfigurationNode);
                        rootConfigurations.Save(xmlFilePth);

                        //Arrange the view
                        ListConfigurations.Clear();
                        ListConfigurations = configurationListName();
                        SelectedConfiguration = ListConfigurations.Last();
                        IsOpenDialog = false;
                        IsSaveNameVisible = false;
                    });
                }
                return saveConfiguration;
            }
        }

        private ICommand openSaveDialog;
        public ICommand OpenSaveDialog
        {
            get
            {
                if (openSaveDialog == null)
                {
                    openSaveDialog = new RelayCommand<object>((obj) =>
                    {
                        IsOpenDialog = true;
                        IsSaveNameVisible = true;
                        IsFormVisible = false;
                        isDetailsVisible = false; 
                    });
                }
            return openSaveDialog;
            }
        }

        //private ICommand persistToXml;

        //public ICommand PersistToXml
        //{
        //    get
        //    {
        //        if (persistToXml == null)
        //        {
        //            persistToXml = new RelayCommand<object>((obj) =>
        //            {
        //                SaveFileDialog saveFileDialog = new SaveFileDialog();
        //                saveFileDialog.Filter = "XML File (*.xml) | *.xml";
        //                saveFileDialog.DefaultExt = "xml";
        //                if (saveFileDialog.ShowDialog() == true)
        //                {
        //                    XmlSerializer serialiser = new XmlSerializer(ListEntries.GetType());
        //                    TextWriter Filestream = new StreamWriter(saveFileDialog.FileName);
        //                    //write to the file
        //                    serialiser.Serialize(Filestream, ListEntries);
        //                    // Close the file
        //                    Filestream.Close();
        //                }


        //            });
                    
        //        }
        //        return persistToXml;
        //    }
        //}

        //private ICommand readXmlFile;
        //public ICommand ReadXmlFile
        //{
        //    get
        //    {
        //        if (readXmlFile == null)
        //        {
        //            readXmlFile = new RelayCommand<object>((obj) =>
        //            {
        //                var uploadConfigXml = new OpenFileDialog();
        //                uploadConfigXml.Filter = "XML Files (*.xml)|*.xml";
        //                uploadConfigXml.ShowDialog();
        //            });
        //        }
        //                return readXmlFile;
        //    }
        //}

        private ICommand getSourceFilePath;
        public ICommand GetSourceFilePath
        {
            get
            {
                if (getSourceFilePath == null)
                {
                    getSourceFilePath = new RelayCommand<object>((obj) =>
                    {
                        var getSourceFilePath = new OpenFileDialog();
                        getSourceFilePath.ShowDialog();
                        Source = getSourceFilePath.FileName;
                        SourceFolderPath = Path.GetDirectoryName(Source);
                    });
                }
                return getSourceFilePath;
            }
        }
        private ICommand getDestinationFilePath;
        public ICommand GetDestinationFilePath
        {
            get
            {
                if (getDestinationFilePath == null)
                {
                    getDestinationFilePath = new RelayCommand<object>((obj) =>
                    {
                        var destinationFilePath = new OpenFileDialog();
                        destinationFilePath.ShowDialog();
                    });
                }
                return getDestinationFilePath;
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

        public bool IsSaveNameVisible
        {
            get
            {
                return isSaveNameVisible;
            }

            set
            {
                isSaveNameVisible = value;
                OnPropertyChanged("IsSaveNameVisible");
            }
        }

        public string NewConfigurationName
        {
            get
            {
                return newConfigurationName;
            }

            set
            {
                newConfigurationName = value;
                OnPropertyChanged("NewConfigurationName");
            }
        }

        public string SelectedConfiguration
        {
            get
            {
                //if (selectedConfiguration != null)
                //{
                //    ReadConfiguration("9c1c11ab-e0f4-4662-b38e-c52c7fe6df82");
                //}
                return selectedConfiguration;
            }

            set
            {
                selectedConfiguration = value;
                OnPropertyChanged("SelectedConfiguration");
            }
        }

        ///########################################
        ///########################################
        ///######## Methods internally used #######
        ///########################################
        ///########################################

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

        ///// <summary>
        ///// Method to load the configuration file from
        ///// an XML file and bind it to the datagrid
        ///// </summary>
        ///// <param name="path"></param>
        //private void LoadXmlConfig(string path)
        //{
        //    XmlSerializer serialiser = new XmlSerializer(ListEntries.GetType());
        //    // Create a new file stream for reading the XML file
        //    FileStream ReadFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

        //    // Load the object saved above by using the Deserialize function
        //    var LoadedConfig = (ObservableCollection<MyData>)serialiser.Deserialize(ReadFileStream);
        //    ListEntries = LoadedConfig;
        //    // Cleanup
        //    ReadFileStream.Close();
        //}


        /// <summary>
        /// Read a configuration from the XML Configuration File
        /// based on the given id
        /// </summary>
        /// <param name="id">id of the configuration</param>
        private void ReadConfiguration(string id)
        {
            var configFile = XDocument.Load(xmlFilePth);
            var config = from data in configFile.Descendants("Configuration")
                         .Where(item => ((string)item.Attribute("confId")).CompareTo(id) == 0)
                         .Descendants("MyData")
                         select new
                         {
                             XmlId = data.Element("Id")?.Value,
                             XmlAction = data.Element("Action")?.Value,
                             XmlServer = data.Element("Server")?.Value,
                             XmlSource = data.Element("Source")?.Value,
                             XmlDestination = data.Element("Destination")?.Value,
                             XmlSourceFolderPath = data.Element("SourceFolderPath")?.Value,
                             XmlDescription = data.Element("Description")?.Value,
                             XmlSite = data.Element("Site")?.Value
                         };
            foreach (var item in config)
            {
                var dataConfig = new MyData()
                {
                    Action = item.XmlAction,
                    Description = item.XmlDescription,
                    Destination = item.XmlDestination,
                    Server = item.XmlServer,
                    Site = item.XmlSite,
                    Source = item.XmlSource,
                    SourceFolderPath = item.XmlSourceFolderPath,
                    Id = Convert.ToInt32(item.XmlId)
                };
                ListEntries.Add(dataConfig);
            }
        }

        /// <summary>
        /// Return the number of saved configurations
        /// </summary>
        /// <returns></returns>
        private int ConfigurationListCount()
        {
            var configFile = XDocument.Load(xmlFilePth);
            return configFile.Descendants("Configuration").Count();

        }

        /// <summary>
        /// Read Configurations name to bind with the comboBox
        /// </summary>
        private ObservableCollection<string> configurationListName()
        {
            var configFile = XDocument.Load(xmlFilePth);
            var confList = from configurations in configFile.Descendants("Configuration")
                           select new
                           {
                               Name = configurations.Attribute("name")?.Value
                           };
            foreach (var item in confList)
            {
                ListConfigurations.Add(item.Name);
            }
            return listConfigurations;
        }

    }
}
