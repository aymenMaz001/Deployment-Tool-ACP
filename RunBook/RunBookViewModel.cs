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
            
            ListConfigurations = ConfigurationListName();
            cbList.Add("StopIIS");
            cbList.Add("Copy");
            cbList.Add("Execute Sql");
            cbList.Add("Execute Cmd");
            //Default selected configuration
            SelectedConfiguration = ListConfigurations.FirstOrDefault();

            var configurationFile = XDocument.Load(xmlFilePth);
            //SelectedConfiguration.Key = configurationFile.Descendants("Configuration").FirstOrDefault().Attribute("confId").Value;
            ReadConfiguration(SelectedConfiguration.Key);

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
        //private KeyValuePair<string,string> selectedConfiguration = new KeyValuePair<string, string>();
        private KeyValueData selectedConfiguration = new KeyValueData();
        private string newConfigurationName;
        //boolean variables to controls visibility
        private bool isBtnVisible;
        private bool isBtnAddVisible;
        private bool isDetailsVisible;
        private bool isFormVisible;
        private bool isSaveNameVisible;


        ///########################################
        ///########################################
        ///######## ViewModel Properties  #########
        ///########################################
        ///########################################

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

        private ObservableCollection<KeyValueData> listConfigurations = new ObservableCollection<KeyValueData>();
        public ObservableCollection<KeyValueData> ListConfigurations
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
                return id;
            }

            set
            {
                id = value;
                OnPropertyChanged("Id");
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

        public KeyValueData SelectedConfiguration
        {
            get
            {
                return selectedConfiguration;

            }

            set
            {
                if (value != null)
                {
                    selectedConfiguration = value;
                    OnPropertyChanged("SelectedConfiguration");
                    ReadConfiguration(SelectedConfiguration.Key);
                }
            }
        }

        /// <summary>
        /// Boolean Properties to Control Components visibility
        /// </summary>
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

        ///########################################
        ///########################################
        ///######## ViewModel Commands  ###########
        ///########################################
        ///########################################

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
                        ListConfigurations = ConfigurationListName();
                        SelectedConfiguration = ListConfigurations.Last();
                        IsOpenDialog = false;
                        IsSaveNameVisible = false;
                        NewConfigurationName = "";
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

        private ICommand deleteConfiguration;
        public ICommand DeleteConfiguration
            {
            get
            {
                if (deleteConfiguration == null)
                {
                    deleteConfiguration = new RelayCommand<object>((obj) =>
                    {
                        ListConfigurations.Remove(ListConfigurations.FirstOrDefault((item) => item.Key == selectedConfiguration.Key));
                        //Rearrange the combobox
                        var configurationFile = XDocument.Load(xmlFilePth);
                        configurationFile.Descendants("Configuration").FirstOrDefault(item => item.Attribute("confId").Value.ToString() == SelectedConfiguration.Key).Remove();
                        configurationFile.Save(xmlFilePth);
                        ListConfigurations.Clear();
                        ListConfigurations = ConfigurationListName();
                        SelectedConfiguration = ListConfigurations.FirstOrDefault();
                    });
                }
                return deleteConfiguration;
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
            ListEntries.Clear();
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
        /// Read Configurations name to bind with the comboBox
        /// </summary>
        private ObservableCollection<KeyValueData> ConfigurationListName()
        {
            var configFile = XDocument.Load(xmlFilePth);
            var confList = from configurations in configFile.Descendants("Configuration")
                           select new
                           {
                               Name = configurations.Attribute("name")?.Value,
                               KeyConf = configurations.Attribute("confId")?.Value
                           };
            foreach (var item in confList)
            {
                ListConfigurations.Add(new KeyValueData(item.KeyConf, item.Name));
            }
            return ListConfigurations;
        }

    }
}
