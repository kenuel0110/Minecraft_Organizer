using CefSharp.Wpf;
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

namespace MinecraftOrganizer.pages
{
    /// <summary>
    /// Interaction logic for Page_browser.xaml
    /// </summary>
    public partial class Page_browser : Page
    {
        #region global
        string global_link = "";
        #endregion
        public Page_browser(string link)
        {
            InitializeComponent();
            global_link = link;
            ChromiumWebBrowser browser = new ChromiumWebBrowser()
            {
                Address = link
            };
            browserContainer.Content = browser;
        }

        private void btn_go_back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack == true)
                NavigationService.GoBack();
        }

        private void btn_share_link_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(global_link);
        }
    }
}
