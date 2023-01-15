using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
    /// Interaction logic for Page_mod_list.xaml
    /// </summary>
    public partial class Page_mod_list : Page
    {
        #region global
        private string selected_path = "";
        private string selected_name = "";
        List<String> selected_mods = new List<String>();
        #endregion


        public Page_mod_list()
        {
            InitializeComponent();

            init_profile();
            init_list_mod();
        }

        private void init_list_mod()
        {
            string[] allfiles = Directory.GetFiles(selected_path);
            if (Directory.Exists($"profiles\\{selected_name}\\mods"))
            {
                System.IO.Directory.Delete($"profiles\\{selected_name}\\mods", true);
            }

            foreach (var i in allfiles)
            {
                //MessageBox.Show($"{System.IO.Path.GetFileNameWithoutExtension(i)}\n {i}");

                string zipPath = i;
                string extractPath = $"profiles\\{selected_name}\\mods\\{System.IO.Path.GetFileNameWithoutExtension(i)}";
                
                Directory.CreateDirectory(extractPath);

                string archivePath = zipPath;
                using (FileStream fileStream = File.Open(archivePath, FileMode.Open))
                {
                    ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Update);
                    string fileName = "fabric.mod.json";
                    var file = archive.GetEntry(fileName);
                    if (file != null)
                    {
                        file.ExtractToFile(System.IO.Path.Combine(extractPath, fileName));
                    }
                }

                
                    //FastZip fz = new FastZip();
                    //fz.ExtractZip(i, $"profiles\\{selected_name}\\mods\\{System.IO.Path.GetFileNameWithoutExtension(i)}", "");

                    //System.IO.Compression.ZipFile.ExtractToDirectory(i, $"profiles\\{selected_name}\\mods\\{System.IO.Path.GetFileNameWithoutExtension(i)}");
                }
        }

        private void init_profile()
        {
            if (File.Exists("profiles.json"))
            {
                string fileName = "profiles.json";
                string jsonString = File.ReadAllText(fileName);

                List<Classes.Profile> profile_list = JsonSerializer.Deserialize<List<Classes.Profile>>(jsonString);
                List<String> profile_combobox = new List<String>();

                int selected_index = 0;
                int a = 0;
                foreach (var i in profile_list)
                {
                    if (i.set == true)
                    {
                        selected_index = a;
                        selected_path = i.path;
                        selected_name = i.name;
                        selected_mods.Clear();
                        foreach (var m in i.mods)
                        {
                            selected_mods.Add(m);
                        }
                    }
                    profile_combobox.Add(i.name);
                    a += 1;
                }

                cb_profile.ItemsSource = profile_combobox;
                cb_profile.SelectedIndex = selected_index;
            }
        }
    }
}
