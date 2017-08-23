using System;

namespace App.UI.Data
{
    public class GenericDataSelector<T> : DataSelector
    {
        public override Type GenericType { get { return typeof(T); } }
    }
}