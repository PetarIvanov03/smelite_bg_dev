namespace smelite_app.ViewModels.Master
{
    public class EditCraftViewModel : CraftViewModel
    {
        public int Id { get; set; }

        public List<CraftImageItemViewModel> ExistingImages { get; set; } = new();

        public List<int> RemoveImageIds { get; set; } = new();
    }
}
