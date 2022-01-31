namespace CDN.Models.DTO
{
    public class DirectoryItemView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDir { get; set; }
        public bool IsFile { get; set; }
        public bool IsRoot { get; set; }
        public string TempLink { get; set; }
        public DateTime TempLinkUntil { get; set; }
        public int Size { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedBy { get; set; }
        public bool IsPublic { get; set; }

        public DirectoryItemView(CDNEntry entry)
        {
            Id = entry.Id;
            Name = entry.Name;
            IsDir = entry.IsDir;
            IsFile = !entry.IsDir;
            IsRoot = entry.Parent == null;
            if (entry.CDNTempEntry != null)
            {
                TempLink = entry.CDNTempEntry.Hash;
                TempLinkUntil = entry.CDNTempEntry.ValidUntil;
            }
            Size = entry.Size;
            UploadedAt = entry.UploadedAt;
            UploadedBy = entry.UploadedBy?.Name ?? string.Empty;
            IsPublic = entry.IsPublic;
        }
    }
}
