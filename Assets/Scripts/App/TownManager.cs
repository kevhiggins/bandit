using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App
{
    class TownManager
    {
        private List<Town> towns;

        public TownManager()
        {
            towns = new List<Town>(Object.FindObjectsOfType<Town>());
        }

        public List<Town> GetTowns()
        {
            return towns;
        }

        public Town GetDifferentTown(Town town)
        {          
            List<Town> filteredTowns = towns.Where(x => x != town).ToList();
            var townIndex = Random.Range(0, filteredTowns.Count);
            return filteredTowns.ElementAt(townIndex);
        }
    }
}
