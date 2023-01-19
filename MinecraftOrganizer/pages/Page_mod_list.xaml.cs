using AngleSharp;
using CurseForge.APIClient;
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
using System.Windows.Media.Effects;
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
        List<Classes.Mod_data_local> mod_data = new List<Classes.Mod_data_local>();
        Dictionary<string, string> hyperLink = new Dictionary<string, string>();
        Dictionary<string, string> mods_links = new Dictionary<string, string>();
        private string apiKey = "$2a$10$DqpmSQtob4Ey/h24eDJYpeTz8YNFvqSP94HX48qrQ3fuufrp/0ZJW";
        private int partnerId = 0;
        private string contactEmail = "zeroonezero0110@gmail.com";
        #endregion

        #region parser
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
        public List<string> AngleSharpMod(string Html)
        {
            List<string> hrefTags = new List<string>();

            var parser = new AngleSharp.Html.Parser.HtmlParser();
            var document = parser.ParseDocument(Html);
            foreach (AngleSharp.Dom.IElement element in document.QuerySelectorAll("img"))
            {
                hrefTags.Add(element.GetAttribute("src"));
            }

            return hrefTags;
        }
        public List<string> AngleSharpModInfo(string Html)
        {
            List<string> hrefTags = new List<string>();

            var parser = new AngleSharp.Html.Parser.HtmlParser();
            var document = parser.ParseDocument(Html);

            var mod_info = document.GetElementsByClassName("box__body")[17].QuerySelectorAll("div");
            MessageBox.Show(mod_info[0].ToString());
            for (var i = 0; i < mod_info.Count(); i++)
            {
                string name = mod_info[i].QuerySelector("h2").TextContent.Trim();
                hrefTags.Add(name);
                MessageBox.Show(name);
            }

            return hrefTags;
        }
        #endregion


        public Page_mod_list()
        {
            InitializeComponent();

            init_program();

        }

        async Task init_program()
        {
            border_loading.Visibility = Visibility.Visible;
            BlurEffect blur = new BlurEffect();         //создание эффекта блюр на "фон"
            blur.Radius = 6;
            grid_main.Effect = blur;             //применение эффекта
            init_profile();
            //if (!Directory.Exists($"profiles\\{selected_name}\\mods"))
            //{
            await Task.Run(() => get_data_mods());
            //await Task.Run(() => update_data_mods());
            //}
            init_list_mod();

            border_loading.Visibility = Visibility.Hidden;
            blur.Radius = 0;
            grid_main.Effect = blur;             //отключение эффекта
        }

        private void update_data_mods()
        {
            int index = 0;
            string[] alldirectory = Directory.GetDirectories($"profiles\\{selected_name}\\mods");
            foreach (var directory in alldirectory)
            {
                if (Directory.GetFiles(directory).Count() == 2)
                {
                    mod_data[index] = new Classes.Mod_data_local()
                    {
                        schemaVersion = mod_data[index].schemaVersion,
                        id = mod_data[index].id,
                        provides = mod_data[index].provides,
                        description = mod_data[index].description,
                        name = mod_data[index].name,
                        version = mod_data[index].version,
                        environment = mod_data[index].environment,
                        license = mod_data[index].license,
                        icon = Directory.GetFiles(directory)[1],
                        contact = mod_data[index].contact,
                        authors = mod_data[index].authors,
                        depends = mod_data[index].depends,
                        jars = mod_data[index].jars,
                        path_folder = mod_data[index].path_folder
                    };
                    MessageBox.Show(Directory.GetFiles(directory)[1]);

                }
                else if (Directory.GetFiles(directory).Count() == 1)
                {
                    mod_data[index] = new Classes.Mod_data_local()
                    {
                        schemaVersion = mod_data[index].schemaVersion,
                        id = mod_data[index].id,
                        provides = mod_data[index].provides,
                        description = mod_data[index].description,
                        name = mod_data[index].name,
                        version = mod_data[index].version,
                        environment = mod_data[index].environment,
                        license = mod_data[index].license,
                        icon = null,
                        contact = mod_data[index].contact,
                        authors = mod_data[index].authors,
                        depends = mod_data[index].depends,
                        jars = mod_data[index].jars,
                        path_folder = mod_data[index].path_folder
                    };
                }
                index++;
            }
            /*
            foreach (var link in mods_links) 
            {
                for (int a = 0; a > mod_data.Count(); a++) 
                {
                    mod_data[a] = new Classes.Mod_data_local()
                    {
                        schemaVersion = mod_data[a].schemaVersion,
                        id = mod_data[a].id,
                        provides = mod_data[a].provides,
                        description = mod_data[a].description,
                        name = mod_data[a].name,
                        version = mod_data[a].version,
                        environment = mod_data[a].environment,
                        license = mod_data[a].license,
                        icon = null,
                        contact = mod_data[a].contact,
                        authors = mod_data[a].authors,
                        depends = mod_data[a].depends,
                        jars = mod_data[a].jars,
                        path_folder = mod_data[a].path_folder
                    };
                    //MessageBox.Show($"{link.Key}\n{mod.name}");
                    MessageBox.Show($"{mod_data[a].name}\n{link.Key}");
                    if (link.Key == mod_data[a].name)
                    {
                        mod_data[a] = new Classes.Mod_data_local()
                        {
                            schemaVersion = mod_data[a].schemaVersion,
                            id = mod_data[a].id,
                            provides = mod_data[a].provides,
                            description = mod_data[a].description,
                            name = mod_data[a].name,
                            version = mod_data[a].version,
                            environment = mod_data[a].environment,
                            license = mod_data[a].license,
                            icon = $"{mod_data[a].path_folder}\\icon.jpg",
                            contact = mod_data[a].contact,
                            authors = mod_data[a].authors,
                            depends = mod_data[a].depends,
                            jars = mod_data[a].jars,
                            path_folder = mod_data[a].path_folder
                        };*/

            /*var context = BrowsingContext.New(Configuration.Default);
            var doc = context.OpenAsync(link.Value.ToString());

            MessageBox.Show($"_______________________{link.Key}\n{mod.name}");
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                System.Threading.Thread.Sleep(1000);
                string htmlCode = client.DownloadString(link.Key);
                List<string> mod_inf = AngleSharpModInfo(htmlCode);
            }*/
            //}
            //MessageBox.Show($"{mod_data[a].path_folder}\\icon.jpg");
            //}
            //}
        }

        private void get_data_mods()
        {
            mods_links.Clear();
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

            string[] allmods = Directory.GetDirectories($"profiles\\{selected_name}\\mods");

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
                        new Classes.Mod_data_local()
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
                            jars = mod_data_json.jars,
                            path_folder = mod
                        });
                }
                else
                {
                    mod_data.Add(
                            new Classes.Mod_data_local()
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
                                jars = new List<Dictionary<string, string>>(),
                                path_folder = mod
                            });
                }
            }
        }


        //minecraft-inside parsing it`s in get_data_mods()
        /*List<string> link_mods = new List<string>();
        foreach (var i in mod_data)
        {
            string id_mod = i.name.Replace(' ', '-').ToLower();
            string web_link_search = $"https://minecraft-inside.ru/search/?q={id_mod}";

            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                System.Threading.Thread.Sleep(1000);
                string htmlCode = client.DownloadString(web_link_search);

                IEnumerable<string> links_mod = AngleSharp(htmlCode);

                link_mods.Clear();
                foreach (string link in links_mod)
                {
                    if (link.EndsWith($"{id_mod}.html") == true)
                    {
                        string[] id_mod_string = id_mod.Split('-');
                        if (link_string.Count() - id_mod_string.Count() == 3)
                        {
                            link_mods.Add($"https://minecraft-inside.ru{link}");
                            string link_mod = $"https://minecraft-inside.ru{link}";
                        }
                    }
                }

                if (link_mods.Count() > 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    string modhtmlCode = client.DownloadString(link_mods[0]);
                    List<string> images_mod = AngleSharpMod(modhtmlCode);

                    //MessageBox.Show($"https://minecraft-inside.ru{images_mod[1]}");
                    if(mods_links.ContainsKey(i.name) == false)
                        mods_links.Add(i.name, images_mod[1]);
                    client.DownloadFile($"https://minecraft-inside.ru{images_mod[1]}", $"{i.path_folder}\\icon.jpg");
                }
                int ass = 1;
                foreach (var a in links_mod) 
                {
                    MessageBox.Show($"{ass}\n{a}\n{i.name}");
                    ass += 1;
                }
            }*/



        private void init_list_mod()
        {
            mod_data.OrderBy(x => x.name);
            lv_mods.ItemsSource = mod_data;
            lv_mods.Items.Refresh();

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
                            //selected_mods.Add(m);
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
                if(item_mod.icon != null)
                    image_icon.Source = new BitmapImage(new Uri(@item_mod.icon, UriKind.Relative));
                else
                    image_icon.Source = new BitmapImage(new Uri("/icons/ic_not_found.png", UriKind.Relative));
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

        private void lv_links_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            foreach (var i in hyperLink)
            {
                if (lv_links.SelectedValue != null)
                {
                    if (lv_links.SelectedValue.ToString() == i.ToString())
                    {
                        NavigationService.Navigate(new pages.Page_browser(i.Value));
                    }
                }
            }
        }
    }
}
