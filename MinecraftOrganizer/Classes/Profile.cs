using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftOrganizer.Classes
{
    public class Profile
    {
        public string name { get; set; }
        public string path { get; set; }
        public List<String> mods { get; set; }
    }
    public class Profile_file
    {
        public string name { get; set; }
        public string path { get; set; }
        public string mods { get; set; }
    }
}
