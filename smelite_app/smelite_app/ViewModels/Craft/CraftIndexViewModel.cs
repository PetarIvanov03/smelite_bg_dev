using Microsoft.AspNetCore.Mvc.Rendering;

namespace smelite_app.ViewModels.Master
{
    public class CraftIndexViewModel
    {
        public IEnumerable<CraftListItemViewModel> Masters { get; set; } = new List<CraftListItemViewModel>();

        public int? CraftTypeId { get; set; }
        public int? LocationId { get; set; }
        public string? SearchString { get; set; }

        public SelectList? CraftTypes { get; set; }
        public SelectList? Locations { get; set; }
    }
}
