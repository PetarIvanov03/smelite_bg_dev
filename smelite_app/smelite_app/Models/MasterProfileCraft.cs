namespace smelite_app.Models
{
    public class MasterProfileCraft
    {
        public int MasterProfileId { get; set; }
        public MasterProfile MasterProfile { get; set; }

        public int CraftId { get; set; }
        public Craft Craft { get; set; }
    }

}
