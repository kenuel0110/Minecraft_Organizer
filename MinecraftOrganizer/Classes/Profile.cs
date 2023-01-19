using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftOrganizer.Classes
{
    public class Profile
    {
        public bool set { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string verion { get; set; }
        public string toolchain { get; set; }
        public string sync_site { get; set; }
        public List<List<Classes.Mod_data_local>> mods { get; set; }

    }

}
