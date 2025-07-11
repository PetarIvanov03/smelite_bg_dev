namespace smelite_app.ViewModels.Master
{
    public class CraftDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PersonalInfo { get; set; }
        public string? PhotoUrl { get; set; }
        public List<string> Crafts { get; set; } = new();
    }
}
