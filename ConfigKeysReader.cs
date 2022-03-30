
using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

namespace Shortcuts
{
    public class ShortcutEntry
    {
        public ShortcutEntry(string key, string val)
        {
            this.Key = key;
            this.Value = val;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class Shortcuts
    {
        public List<KeyValuePair<string,string>> Keys { get; set; }
    }
    public class ConfigKeysReader
    {
        public Shortcuts Entries { get; private set; }
        public ConfigKeysReader()
        {
            ReadusingFromAppSettings();
        }
        public ConfigKeysReader(string fname)
        {
            ReadusingFromConfigFile(fname);
        }
        void ReadusingFromAppSettings()
        {
            Entries = new Shortcuts();
            Entries.Keys = new List<KeyValuePair<string, string>>();
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                Entries.Keys.Add(new KeyValuePair<string, string>(key, ConfigurationManager.AppSettings[key]));
            }
        }
        void ReadusingFromConfigFile(string fname) 
        {
            //string jsontext = File.ReadAllText(fname);
            //using results = Newtonsoft.Json.JsonConvert.DeserializeObject <using> (jsontext);
            //if (results != null)
            //{
            //    Entries = results;
            //}
        }
    }
}
