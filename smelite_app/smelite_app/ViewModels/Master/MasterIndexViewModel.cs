using Microsoft.AspNetCore.Mvc.Rendering;

namespace smelite_app.ViewModels.Master
{
    public class MasterIndexViewModel
    {
        public IEnumerable<MasterListItemViewModel> Masters { get; set; } = new List<MasterListItemViewModel>();

        public int? CraftTypeId { get; set; }
        public int? LocationId { get; set; }
        public string? SearchString { get; set; }

        public SelectList? CraftTypes { get; set; }
        public SelectList? Locations { get; set; }
    }
}
