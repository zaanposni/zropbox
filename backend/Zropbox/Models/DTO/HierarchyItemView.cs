namespace Zropbox.Models.DTO
{
    public class HierarchyItemView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRoot { get; set; }
        public int ParentId { get; set; }

        public HierarchyItemView(CDNEntry entry)
        {
            Id = entry.Id;
            Name = entry.Name;
            IsRoot = entry.Parent == null;
            ParentId = entry.Parent?.Id ?? 0;
        }
    }
}
