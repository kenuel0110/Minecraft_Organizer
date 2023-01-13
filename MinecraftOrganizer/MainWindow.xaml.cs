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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinecraftOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (this.WindowState == WindowState.Normal)
            {
                image_restore.Source = new BitmapImage(new Uri(@"/icons/ic_restore.png", UriKind.Relative));
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                image_restore.Source = new BitmapImage(new Uri(@"/icons/ic_maximize.png", UriKind.Relative));
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_restore_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal) {
                this.WindowState = WindowState.Maximized;
                image_restore.Source = new BitmapImage(new Uri(@"/icons/ic_restore.png", UriKind.Relative));
            }
            else if (this.WindowState == WindowState.Maximized) {
                this.WindowState = WindowState.Normal;
                image_restore.Source = new BitmapImage(new Uri(@"/icons/ic_maximize.png", UriKind.Relative));
            }
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            // Begin dragging the window
            this.DragMove();
        }
    }
}
