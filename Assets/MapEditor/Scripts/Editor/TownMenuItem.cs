using UnityEditor;
using UnityEngine;

namespace MapEditor.Editor
{
    public class TownMenuItem
    {
        [MenuItem("GameObject/Town")]
        public static void CreateTown()
        {
            GameObject gameObject = new GameObject("Town");
            gameObject.AddComponent<Town>();
        }
    }
}