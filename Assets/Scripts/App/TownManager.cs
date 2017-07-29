using System.Collections.Generic;
using System.Linq;
using App.Location;
using UnityEngine;

namespace App
{
    public class TownManager
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

        public Town GetRandomTown()
        {
            var townIndex = Random.Range(0, towns.Count);
            return towns.ElementAt(townIndex);
        }
    }
}
