﻿namespace Zropbox.Models
{
    public class TempEntryView
    {
        public int Id { get; set; }
        public string Hash { get; set; }
        public string Url { get; set; }
        public DateTime ValidUntil { get; set; }

        public TempEntryView(CDNTempEntry entry)
        {
            Id = entry.Id;
            Hash = entry.Hash;
            Url = $"/share/{Hash}";
            ValidUntil = entry.ValidUntil;
        }
    }
}
