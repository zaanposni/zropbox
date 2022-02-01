using System.ComponentModel.DataAnnotations.Schema;

namespace Zropbox.Models
{
    public class CDNTempEntry
    {
        public int Id { get; set; }
        public string Hash { get; set; }
        public DateTime ValidUntil { get; set; }
        [ForeignKey("EntryId")]
        public CDNEntry CDNEntry { get; set; }
    }
}
