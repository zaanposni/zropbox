using System.ComponentModel.DataAnnotations.Schema;

namespace CDN.Models
{
    public class CDNEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDir { get; set; }
        public int Size { get; set; }
        public DateTime UploadedAt { get; set; }
        [ForeignKey("UserId")]
        public User UploadedBy { get; set; }
        [ForeignKey("ParentId")]
        public CDNEntry? Parent { get; set; }
        public bool IsPublic { get; set; }
        public CDNTempEntry CDNTempEntry { get; set; }
    }
}
