﻿using CurseForge.APIClient;
using HtmlAgilityPack;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
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
    /// Interaction logic for Page_mod_list.xaml
    /// </summary>
    public partial class Page_mod_list : Page
    {
        #region global
        private string selected_path = "";
        private string selected_name = "";
        List<String> selected_mods = new List<String>();
        List<Classes.Mod_data> mod_data = new List<Classes.Mod_data>();
        Dictionary<string, string> hyperLink = new Dictionary<string, string>();
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
             
                /*var cfApiClient = new ApiClient(apiKey, partnerId, contactEmail);
                var mod = await cfApiClient.GetModAsync(modId);
                */


                //FastZip fz = new FastZip();
                //fz.ExtractZip(i, $"profiles\\{selected_name}\\mods\\{System.IO.Path.GetFileNameWithoutExtension(i)}", "");

                //System.IO.Compression.ZipFile.ExtractToDirectory(i, $"profiles\\{selected_name}\\mods\\{System.IO.Path.GetFileNameWithoutExtension(i)}");
            }

            string[] allmods = Directory.GetDirectories($"profiles\\{selected_name}\\mods");
            lv_mods.ItemsSource = mod_data;

            var options = new JsonSerializerSettings
            {
                Converters = { new Classes.ReplacingStringWritingConverter("\n", "") }
            };
            foreach (var mod in allmods)
            {
                if ((File.Exists($"{mod}\\fabric.mod.json")))
                {
                    string fileName = $"{mod}\\fabric.mod.json";
                    string jsonString = File.ReadAllText(fileName);
                    //MessageBox.Show(fileName);
                    Classes.Mod_data mod_data_json = JsonConvert.DeserializeObject<Classes.Mod_data>(jsonString, options);
                    mod_data.Add(
                        new Classes.Mod_data()
                        {
                            schemaVersion = mod_data_json.schemaVersion,
                            id = mod_data_json.id,
                            provides = mod_data_json.provides,
                            description = mod_data_json.description,
                            name = mod_data_json.name,
                            version = mod_data_json.version,
                            environment = mod_data_json.environment,
                            license = mod_data_json.license,
                            icon = mod_data_json.icon,
                            contact = mod_data_json.contact,
                            authors = mod_data_json.authors,
                            depends = mod_data_json.depends,
                            jars = mod_data_json.jars
                        });
                }
                else {
                    mod_data.Add(
                            new Classes.Mod_data()
                            {
                                schemaVersion = 0,
                                id = "",
                                provides = new List<string>(),
                                description = "",
                                name = new DirectoryInfo(mod).Name,
                                version = "",
                                environment = "",
                                license = "",
                                icon = "",
                                contact = new Dictionary<string, string>(),
                                authors = new List<string>(),
                                depends = new Dictionary<string, string>(),
                                jars = new List<Dictionary<string, string>>()
                            });
                }
            }

            foreach (var i in mod_data)
            {
                string id_mod = i.name.Replace(' ', '-').ToLower();
                string web_link_search = $"https://minecraft-inside.ru/search/?q={id_mod}";

                using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                {

                    string htmlCode = client.DownloadString(web_link_search);

                    IEnumerable<string> links_mod = AngleSharp(htmlCode);
                    string result = null;
                    if (links_mod.Where(s => s == $"{id_mod}.html").FirstOrDefault() != null)
                        result = links_mod.Where(s => s == $"{id_mod}.html").FirstOrDefault();
                    if (result != null) 
                    {
                        string link_mod = $"https://minecraft-inside.ru/{result}";
                        MessageBox.Show(link_mod);
                    }
                    /*int ass = 1;
                    foreach (var a in links_mod) 
                    {
                        MessageBox.Show($"{ass}\n{a}\n{i.name}");
                        ass += 1;
                    }*/
                }
                //links_mod[9]
            }

            mod_data.OrderBy(x => x.name);
            lv_mods.Items.Refresh();

        }

        public IEnumerable<string> AngleSharp(string Html)
        {
            List<string> hrefTags = new List<string>();

            var parser = new AngleSharp.Html.Parser.HtmlParser();
            var document = parser.ParseDocument(Html);
            foreach (AngleSharp.Dom.IElement element in document.QuerySelectorAll("a"))
            {
                hrefTags.Add(element.GetAttribute("href"));
            }

            return hrefTags;
        }

        private void init_profile()
        {
            if (File.Exists("profiles.json"))
            {
                string fileName = "profiles.json";
                string jsonString = File.ReadAllText(fileName);

                List<Classes.Profile> profile_list = System.Text.Json.JsonSerializer.Deserialize<List<Classes.Profile>>(jsonString);
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

        private void btn_edit_profiles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_delete_mod_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_edit_mod_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_add_mod_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lv_mods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lv_mods.SelectedIndex != -1)
            {
                grid_mod_data.Visibility = Visibility.Visible;
                var item_mod = mod_data[lv_mods.SelectedIndex];
                image_icon.Source = new BitmapImage(new Uri(@item_mod.icon, UriKind.Relative));
                if (item_mod.environment != null)
                    if (item_mod.environment.ToLower() == "client")
                        tb_is_client.Visibility = Visibility.Visible;
                    else
                        tb_is_client.Visibility = Visibility.Hidden;
                tb_name.Text = item_mod.name;
                tb_version.Text = item_mod.version;
                string authors = "";
                if (item_mod.authors != null)
                {
                    foreach (var i in item_mod.authors)
                        authors += $"{i} ";
                }
                tb_authors.Text = authors;
                tb_description.Text = item_mod.description;

                hyperLink.Clear();
                lv_links.ItemsSource = hyperLink;
                if (item_mod.contact.Count > 0)
                {
                    sp_links.Visibility = Visibility.Visible;

                    foreach (var i in item_mod.contact)
                    {
                        hyperLink.Add(i.Key, i.Value);
                    }
                    lv_links.Items.Refresh();
                }
                else
                    sp_links.Visibility = Visibility.Hidden;

                tb_license.Text = item_mod.license;

                string depends = "";
                if (item_mod.depends != null)
                {
                    foreach (var i in item_mod.depends)
                        depends += $"{i.Key}: {i.Value}\n";
                }
                tb_depends.Text = depends;
            }
        }

        private void lv_links_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var i in hyperLink) {
                if (lv_links.SelectedValue != null) {
                    if (lv_links.SelectedValue.ToString() == i.ToString())
                    {
                        NavigationService.Navigate(new pages.Page_browser(i.Value));
                    }
                }
            }
        }
    }
}
