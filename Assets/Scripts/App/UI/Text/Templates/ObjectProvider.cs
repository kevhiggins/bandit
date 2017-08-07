using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;

namespace App.UI.Text.Templates
{
    public class ObjectProvider : MonoBehaviour, INotifyPropertyChanged
    {
        public Object selected;
        public Object Selected {
            get { return selected; }
            set { selected = value; OnPropertyChanged("Selected"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}