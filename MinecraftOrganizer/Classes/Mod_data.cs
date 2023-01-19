using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftOrganizer.Classes
{
    public class Mod_data
    {
        [DataMember]
        public int schemaVersion { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public List<String> provides { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string environment { get; set; }
        [DataMember]
        public string license { get; set; }
        [DataMember]
        public string icon { get; set; }
        [DataMember]
        public Dictionary<string, string> contact { get; set; }
        [JsonIgnore]
        public List<String> authors { get; set; }
        [JsonIgnore]
        //[JsonNumberHandling(JsonNumberHandling.WriteAsString)]
        public Dictionary<string, string> depends { get; set; }
        [JsonIgnore]
        public string description { get; set; }
        [DataMember]
        public List<Dictionary<string, string>> jars { get; set; }

    }

    public class Mod_data_local
    {
        [DataMember]
        public int schemaVersion { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public List<String> provides { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string environment { get; set; }
        [DataMember]
        public string license { get; set; }
        [DataMember]
        public string icon { get; set; }
        [DataMember]
        public Dictionary<string, string> contact { get; set; }
        [JsonIgnore]
        public List<String> authors { get; set; }
        [JsonIgnore]
        //[JsonNumberHandling(JsonNumberHandling.WriteAsString)]
        public Dictionary<string, string> depends { get; set; }
        [JsonIgnore]
        public string description { get; set; }
        [DataMember]
        public List<Dictionary<string, string>> jars { get; set; }
        public string path_folder { get; set; }

    }

}
