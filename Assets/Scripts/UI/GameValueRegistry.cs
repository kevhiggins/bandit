using System;
using System.Collections.Generic;

namespace Bandit.UI
{
    class GameValueRegistry
    {
        public static Dictionary<string, Dictionary<string, string>> registry = new Dictionary
            <string, Dictionary<string, string>>
        {
            {"General", new Dictionary<string, string> {{"total_gold", "Total Gold"}}}
        };

        private Dictionary<string, GameValueEntry> entries = new Dictionary<string, GameValueEntry>();


        public GameValueRegistry()
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> entry in registry)
            {
                foreach (KeyValuePair<string, string> innerEntry in entry.Value)
                {
                    var gameValueEntry = new GameValueEntry();
                    entries.Add(innerEntry.Key, gameValueEntry);
                }
            }
        }

        public void SetRegistryValue(string name, string value)
        {
            CheckEntryExistence(name);
            entries[name].UpdateValue(value);
        }

        public void RegisterHandler(string name, EntryValueUpdate handler)
        {
            CheckEntryExistence(name);
            entries[name].RegisterHandler(handler);
        }

        public string GetValue(string name)
        {
            CheckEntryExistence(name);
            return entries[name].Value;
        }

        private void CheckEntryExistence(string name)
        {
            if (!entries.ContainsKey(name))
            {
                throw new Exception("Registry entry with name " + name + " does not exist.");
            }
        }
    }
}