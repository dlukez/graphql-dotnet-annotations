namespace GraphQL.Annotations.StarWarsApp.Model
{
    public class DroidAppearance
    {
        public int DroidAppearanceId { get; set; }
        public Episode Episode { get; set; }
        public string Title { get; set; }
        public int DroidId { get; set; }
        public Droid Droid { get; set; }
    }
}