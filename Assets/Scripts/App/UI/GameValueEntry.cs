namespace App.UI
{
    public delegate void EntryValueUpdate(string value);
    class GameValueEntry
    {
        public string Value { get; private set; }
        private event EntryValueUpdate OnValueUpdate = value => { };

        public void UpdateValue(string newValue)
        {
            Value = newValue;
            OnValueUpdate(Value);
        }

        public void RegisterHandler(EntryValueUpdate handler)
        {
            OnValueUpdate += handler;
        }
    }
}
