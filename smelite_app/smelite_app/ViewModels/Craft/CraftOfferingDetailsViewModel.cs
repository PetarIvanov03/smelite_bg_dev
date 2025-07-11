namespace smelite_app.ViewModels.Master
{
    public class CraftOfferingDetailsViewModel
    {
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Package { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
