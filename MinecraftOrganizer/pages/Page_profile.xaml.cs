using CmlLib.Core;
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
using System.Windows.Media.Effects;
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
        #region
        public List<string> verions_minecraft = new List<string>();
        public Dictionary<string, string> combobox_data = new Dictionary<string, string>();
        public List<string> sync_site = new List<string>();
        public List<string> toolchain = new List<string>();
        public string combobox_field = "";
        #endregion
        public Page_profile()
        {
            InitializeComponent();
            get_versions();
            toolchain.Add("Fabric");
            sync_site.Add("CurseForge");
            lv_combobox.ItemsSource = combobox_data;

            tb_sync_site.Text = sync_site[0];
            tb_tool.Text = toolchain[0];

            tb_folder_path.IsReadOnly = false;
            tb_folder_path.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\mods";
            tb_folder_path.IsReadOnly = true;
        }

        private async Task get_versions()
        {
            var path = new MinecraftPath(); // use default directory
            var launcher = new CMLauncher(path);
            var versions = await launcher.GetAllVersionsAsync();

            foreach (var item in versions)
                verions_minecraft.Add(item.Name);

            tb_mc_version.Text = verions_minecraft[0];
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

                    List<Classes.Profile> file_list = JsonSerializer.Deserialize<List<Classes.Profile>>(jsonString);

                    List<Classes.Profile> profile_list = new List<Classes.Profile>();
                    foreach (var i in file_list)
                    {
                        profile_list.Add(
                            new Classes.Profile()
                            {
                                set = false,
                                name = i.name,
                                path = i.path,
                                verion = i.verion,
                                toolchain = i.toolchain,
                                sync_site = i.sync_site,
                                mods = i.mods
                            });
                    }

                    profile_list.Add(new Classes.Profile()
                    {
                        set = true,
                        name = tb_name_profile.Text,
                        path = tb_folder_path.Text,
                        verion = tb_mc_version.Text,
                        toolchain = tb_tool.Text,
                        sync_site = tb_sync_site.Text,
                        mods = new List<List<Classes.Mod_data_local>>()
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
                        verion = tb_mc_version.Text,
                        toolchain = tb_tool.Text,
                        sync_site = tb_sync_site.Text,
                        mods = new List<List<Classes.Mod_data_local>>()
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

        private void tb_mc_version_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            combobox_data.Clear();
            border_combobox.Visibility = Visibility.Visible;
            BlurEffect blur = new BlurEffect();
            blur.Radius = 6;
            grid_main.Effect = blur;

            foreach (var i in verions_minecraft) 
            {
                if (tb_mc_version.Text == i)
                    combobox_data.Add(i, "/icons/ic_done.png");
                else
                    combobox_data.Add(i, "");
            }

            lv_combobox.Items.Refresh();
            combobox_field = "tb_mc_version";
        }

        private void tb_tool_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            combobox_data.Clear();
            border_combobox.Visibility = Visibility.Visible;
            BlurEffect blur = new BlurEffect();
            blur.Radius = 6;
            grid_main.Effect = blur;

            foreach (var i in toolchain)
            {
                if (tb_tool.Text == i)
                    combobox_data.Add(i, "/icons/ic_done.png");
                else
                    combobox_data.Add(i, "");
            }

            lv_combobox.Items.Refresh();
            combobox_field = "tb_tool";
        }

        private void tb_sync_site_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            combobox_data.Clear();
            border_combobox.Visibility = Visibility.Visible;
            BlurEffect blur = new BlurEffect();
            blur.Radius = 6;
            grid_main.Effect = blur;

            foreach (var i in sync_site)
            {
                if (tb_sync_site.Text == i)
                    combobox_data.Add(i, "/icons/ic_done.png");
                else
                    combobox_data.Add(i, "");
            }

            lv_combobox.Items.Refresh();
            combobox_field = "tb_sync_site";
        }

        private void lv_combobox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (combobox_field) 
            {
                case "tb_mc_version":
                    var item_v = lv_combobox.SelectedItem.ToString().Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)
                       .Select(part => part.Split(','))
                       .ToDictionary(split => split[0], split => split[1]);
                    foreach (var i in item_v)
                        tb_mc_version.Text = i.Key;
                    break;
                case "tb_tool":
                    var item_t = lv_combobox.SelectedItem.ToString().Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)
                       .Select(part => part.Split(','))
                       .ToDictionary(split => split[0], split => split[1]);
                    foreach (var i in item_t)
                        tb_tool.Text = i.Key;
                    break;
                case "tb_sync_site":
                    var item_s = lv_combobox.SelectedItem.ToString().Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)
                       .Select(part => part.Split(','))
                       .ToDictionary(split => split[0], split => split[1]);
                    foreach (var i in item_s)
                        tb_sync_site.Text = i.Key;
                    break;
            }
            border_combobox.Visibility = Visibility.Hidden;
            BlurEffect blur = new BlurEffect();
            blur.Radius = 0;
            grid_main.Effect = blur;
        }

        private void btn_close_combomenu_Click(object sender, RoutedEventArgs e)
        {
            border_combobox.Visibility = Visibility.Hidden;
            BlurEffect blur = new BlurEffect();
            blur.Radius = 0;
            grid_main.Effect = blur;
        }
    }
}
