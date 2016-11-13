namespace GraphQL.Annotations.StarWarsApp.Model
{
    public class HumanAppearance
    {
        public int HumanAppearanceId { get; set; }
        public Episode Episode { get; set; }
        public string Title { get; set; }
        public int HumanId { get; set; }
        public Human Human { get; set; }
    }
}