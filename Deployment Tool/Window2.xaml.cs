using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TreeViewOpt
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            //CreateALine();
            //CreateARectangle();
        }

        public void backButton(object sender, RoutedEventArgs e)
        {
            // Show message box when button is clicked
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }

        public void CreateALine()
        {
            // Create a Line
            Line redLine = new Line();
            redLine.X1 = 0;
            redLine.Y1 = 0;
            //redLine.X2 = Window.Width;
            //redLine.Y2 = Window.Height;

            redLine.X2 = myCanvas.Width;
            redLine.Y2 = myCanvas.Height;

            redLine.Stretch = Stretch.Fill;
            // Create a red Brush
            SolidColorBrush redBrush = new SolidColorBrush();
            redBrush.Color = Colors.Red;

            // Set Line's width and color
            redLine.StrokeThickness = 4;
            redLine.Stroke = redBrush;

            // Add line to the Grid.
            //LayoutRoot.Children.Add(redLine);
            myCanvas.Children.Add(redLine);
        }

        public void CreateARectangle()
        {
            // Create a Rectangle  
            Rectangle blueRectangle = new Rectangle();
            blueRectangle.Height = 100;
            blueRectangle.Width = 200;
            Canvas.SetLeft(blueRectangle, 20);
            Canvas.SetTop(blueRectangle, 20);
            // Create a blue and a black Brush  
            SolidColorBrush blueBrush = new SolidColorBrush();
            blueBrush.Color = Colors.Blue;
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;
            // Set Rectangle's width and color  
            blueRectangle.StrokeThickness = 4;
            blueRectangle.Stroke = blackBrush;
            // Fill rectangle with blue color  
            blueRectangle.Fill = blueBrush;
            // Add Rectangle to the Grid.  
            //LayoutRoot.Children.Add(blueRectangle);
            myCanvas.Children.Add(blueRectangle);
        }
    }
}
