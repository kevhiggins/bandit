using System;
using App.UI.Data;
using Object = UnityEngine.Object;

namespace App.UI.Text.Templates
{
    [Serializable]
    public class TemplateObjectSettings
    {
        public string key;

        public DataSelector sourceObject = new DataSelector();
    }
}