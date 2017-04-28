using System;
using System.Collections.Generic;

namespace App.UI
{
    public class GameValueRegistry
    {
        public static GameValueRegistry Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameValueRegistry();
                }
                return instance;
            }
        }

        private static GameValueRegistry instance;

        // Switch the entries to an object, so we can more easily run queries via linq.
        // TODO move this into a configuration, so this can become a package.
        public static Dictionary<string, Dictionary<string, string>> registry = new Dictionary
            <string, Dictionary<string, string>>
        {
            {
                "General", new Dictionary<string, string>
                {
                    {"total_gold", "Total Gold"},
                    {"total_travelers", "Total Travelers"},
                    {"last_robbed_amount", "Last Robbed Amount"}
                }
            },
            {
                "Battle", new Dictionary<string, string>
                {
                    {"last_battle_hit_amount", "Last Battle Hit Amount"},
                    {"current_battle_round", "Current Round"},
                }
            },
            {
                "Miscellaneous", new Dictionary<string, string>
                {
                    {"total_bandits", "Total Bandits"},
                }
            },
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