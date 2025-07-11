using Microsoft.AspNetCore.Mvc.Rendering;

namespace smelite_app.ViewModels.Craft
{
    public class CraftIndexViewModel
    {
        public IEnumerable<CraftListItemViewModel> Crafts { get; set; } = new List<CraftListItemViewModel>();

        public int? CraftTypeId { get; set; }
        public int? LocationId { get; set; }
        public string? SearchString { get; set; }

        public SelectList? CraftTypes { get; set; }
        public SelectList? Locations { get; set; }
    }
}
