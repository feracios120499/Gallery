using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gallery
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            width = image.Width;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var images = GetFiles(folder.SelectedPath, "*.jpg|*.png", SearchOption.TopDirectoryOnly);
                int i = 1;
                this.images.Children.Clear();
                foreach (var item in images)
                {
                    i++;
                    progressBar.Value = (100 * i)/images.Count();
                    this.images.Children.Add(CreateImage(item));
                    await Task.Delay(100);
                }
            }
        }
        private string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {

            string[] searchPatterns = searchPattern.Split('|');
            List<string> files = new List<string>();
            foreach (string sp in searchPatterns)
            {

                try
                {
                    foreach (var item in System.IO.Directory.GetFiles(path, sp, searchOption))
                    {
                        files.Add(item);
                    }
                }
                catch { }
            }
           
            files.Sort();
            progressBar.Value = 100;
            return files.ToArray();
        }
        private Image CreateImage(string path)
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(path));
            image.Cursor = System.Windows.Input.Cursors.Hand;
            image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            
            return image;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image selectedImage = sender as Image;
            image.Source = selectedImage.Source;
        }
        double width = 350;
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            image.Width = width * slider.Value / 10;
            image.Stretch = Stretch.Fill;
        }
    }
}
