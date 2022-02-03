namespace Zropbox.Models
{
    public class DirectoryView
    {
        public int CurrentItemId { get; set; }
        public List<DirectoryItemView> Items { get; set; }
        public List<HierarchyItemView> Hierarchy { get; set; }
    }
}
