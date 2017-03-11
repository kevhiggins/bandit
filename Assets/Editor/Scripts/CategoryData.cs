namespace App.Editor
{
    class CategoryData
    {
        public string[] Names { get; private set; }
        public string[] Titles { get; private set; }

        public CategoryData(string[] names, string[] titles)
        {
            Names = names;
            Titles = titles;
        }
    }
}
