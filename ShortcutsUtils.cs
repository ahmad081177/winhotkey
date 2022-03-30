using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace Shortcuts
{
    internal class ShortcutsUtils
    {
        public static void RegisterHotKeys(Shortcuts entries)
        {
            KeyModifiers modifier = GetModifier(entries);
            foreach(KeyValuePair<string,string> e in entries.Keys)
            {
                if(GetKey(e, out Keys key))
                {
                    HotKeyManager.RegisterHotKey(key, modifier);
                }
            }
        }

        private static bool GetKey(KeyValuePair<string, string> e, out Keys key)
        {
            return Enum.TryParse<Keys>(e.Key, out key);
        }

        private static KeyModifiers GetModifier(Shortcuts entries)
        {
            KeyModifiers m = KeyModifiers.Control; //Default
            foreach(KeyValuePair<string,string> e in entries.Keys)
            {
                if ("modifier".Equals(e.Key.ToLower()))
                {
                    switch (e.Value.ToLower())
                    { 
                        case "ctrl":
                            m = KeyModifiers.Control;
                            break;
                        case "alt":
                            m = KeyModifiers.Alt;
                            break;
                        case "ctrlalt":
                        case "altctrl":
                            m = KeyModifiers.Alt | KeyModifiers.Control;
                            break;
                    }
                    break;
                }
            }
            return m;
        }
    }
}
