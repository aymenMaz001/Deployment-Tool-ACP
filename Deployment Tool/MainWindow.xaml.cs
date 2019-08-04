using Deployment_Tool.Models;
using Microsoft.Web.Administration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TreeViewOpt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<User> items = new ObservableCollection<User>();
        

        public MainWindow()
        {
            InitializeComponent();
            //ListItems.ItemsSource = ItemList();
            items.Add(new User() { Name = "John Doe", Age = 42 });
            items.Add(new User() { Name = "Jane Doe", Age = 39 });
            items.Add(new User() { Name = "Sammy Doe", Age = 13 });
            ListItems.ItemsSource = items;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("Changed Value");
        }

        //void button_Click(object sender, RoutedEventArgs e)
        //{
        //    // Show message box when button is clicked
        //    MessageBox.Show("Hello, Windows Presentation Foundation!");
        //}

        public void Add_item(object sender, RoutedEventArgs e)
        {
            //ListItems.Items.Clear();
            User u = new User() { Name = textItem.Text, Age = 20 };
            //ListItems.Items.Add(u);
            items.Add(u);
        }

        //void Open_Window(object sender, RoutedEventArgs e)
        //{
        //    // Show message box when button is clicked
        //    Window2 win2 = new Window2();
        //    //Upload win2 = new Upload();
        //    win2.Show();
        //    this.Close();
        //    //MessageBox.Show("Hello, Windows Presentation Foundation!");
        //}

        //void ServerManager()
        //{
        //    ServerManager server = new ServerManager();
        //    server.Sites[0].Stop();
        //    server.Dispose();
        //}

    }
}
