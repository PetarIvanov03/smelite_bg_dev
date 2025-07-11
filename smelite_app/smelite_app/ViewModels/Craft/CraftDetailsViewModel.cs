namespace smelite_app.ViewModels.Craft
{
    public class CraftDetailsViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();
        public string? Description { get; set; }
        public List<CraftOfferingDetailsViewModel> Offerings { get; set; } = new();
        public string MasterName { get; set; } = string.Empty;
        public string? MasterInfo { get; set; }
        public string? MasterPhotoUrl { get; set; }
    }
}
