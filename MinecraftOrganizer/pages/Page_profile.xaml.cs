using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for Page_profile.xaml
    /// </summary>
    public partial class Page_profile : Page
    {
        public Page_profile()
        {
            InitializeComponent();

            tb_folder_path.IsReadOnly = false;
            tb_folder_path.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\mods";
            tb_folder_path.IsReadOnly = true;
        }

        private void btn_folder_path_Click(object sender, RoutedEventArgs e)
        {
            var ookiiDialog = new VistaFolderBrowserDialog();
            if (ookiiDialog.ShowDialog() == true)
            {
                tb_folder_path.IsReadOnly = false;
                tb_folder_path.Text = ookiiDialog.SelectedPath;
                tb_folder_path.IsReadOnly = true;
            }
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            bool try_another_name = false;
            if (File.Exists("profiles.json"))
            {
                string fileName = "profiles.json";
                string jsonString = File.ReadAllText(fileName);

                List<Classes.Profile> profile_list = JsonSerializer.Deserialize<List<Classes.Profile>>(jsonString);

                foreach (var i in profile_list) {
                    if (i.name == tb_name_profile.Text) {
                        try_another_name = true;
                    }
                }
            }

            if (tb_name_profile.Text == "" || try_another_name == true)
            {
                tb_name_profile.BorderBrush = Brushes.DarkRed;
                tb_name_profile.Foreground = Brushes.DarkRed;
            }
            else {
                tb_name_profile.BorderBrush = Brushes.Gray;
                tb_name_profile.Foreground = Brushes.White;

                if (File.Exists("profiles.json"))
                {
                    string fileName = "profiles.json";
                    string jsonString = File.ReadAllText(fileName);

                    List<Classes.Profile> profile_list = JsonSerializer.Deserialize<List<Classes.Profile>>(jsonString);

                    profile_list.Add(new Classes.Profile()
                    {
                        set = true,
                        name = tb_name_profile.Text,
                        path = tb_folder_path.Text,
                        mods = new List<string>()
                    });
                    string json = JsonSerializer.Serialize(profile_list);
                    File.WriteAllText("profiles.json", json);
                    NavigationService.Navigate(new pages.Page_mod_list());
                }
                else
                {
                    List<Classes.Profile> profile_list = new List<Classes.Profile>();

                    profile_list.Add(new Classes.Profile()
                    {
                        set = true,
                        name = tb_name_profile.Text,
                        path = tb_folder_path.Text,
                        mods = new List<string>()
                    });
                    string json = JsonSerializer.Serialize(profile_list);
                    File.WriteAllText("profiles.json", json);
                    NavigationService.Navigate(new pages.Page_mod_list());
                }

                if (!Directory.Exists("profiles"))
                {
                    Directory.CreateDirectory("profiles");
                    if (!Directory.Exists($"profiles/{tb_name_profile.Text}"))
                    {
                        Directory.CreateDirectory($"profiles/{tb_name_profile.Text}");
                    }
                }


                //Classes.Profile data_profile_old = JsonSerializer.Deserialize<Classes.Profile>(jsonString);
                /*profile_list.Add(new Classes.Profile()
                {
                    name = data_profile_old.name,
                    path = data_profile_old.path,
                    mods = data_profile_old.mods
                });*/


            }
        }
    }
}
