using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TreeViewOpt
{
    /// <summary>
    /// Interaction logic for FileExplorerWpf.xaml
    /// </summary>
    public partial class FileExplorerWpf : Window
    {
        public FileExplorerWpf()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Selecy your path" })
            {
                if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.WebBrs.Navigate(fbd.SelectedPath);
                    this.textBox.Text = fbd.SelectedPath;

                }
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.WebBrs.CanGoBack)
            {
                WebBrs.GoBack();
            }
        }

        private void ForwardBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WebBrs.CanGoForward)
            {
                WebBrs.GoForward();
            }
        }
    }
}
