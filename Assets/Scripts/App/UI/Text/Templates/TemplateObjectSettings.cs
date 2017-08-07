using System;
using Object = UnityEngine.Object;

namespace App.UI.Text.Templates
{
    [Serializable]
    public class TemplateObjectSettings
    {
        public string key;

        public Object sourceObject;
    }
}