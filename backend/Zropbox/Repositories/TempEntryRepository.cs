using Microsoft.EntityFrameworkCore;
using Zropbox.Exceptions;
using Zropbox.Models;

namespace Zropbox.Repositories
{
    public class TempEntryRepository : BaseRepository<TempEntryRepository>
    {
        private TempEntryRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }
        public static TempEntryRepository CreateDefault(IServiceProvider serviceProvider)
        {
            return new TempEntryRepository(serviceProvider);
        }

        public async Task<CDNEntry> GetEntryByHash(string hash)
        {
            CDNTempEntry entry = await Context.CDNTempEntries.Include(x => x.CDNEntry).AsQueryable().Where(x => x.Hash == hash).FirstOrDefaultAsync();
            if (entry == null)
            {
                throw new ResourceNotFoundException();
            }
            if (entry.ValidUntil < DateTime.UtcNow)
            {
                await DeleteEntry(entry);
                throw new ResourceNotFoundException();
            }
            return entry.CDNEntry;
        }

        private string GetRandomHash()
        {
            Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<CDNTempEntry> CreateTempAccess(CDNEntry entry, DateTime validUntil)
        {
            CDNTempEntry tempEntry = await Context.CDNTempEntries.Include(x => x.CDNEntry).AsQueryable().Where(x => x.CDNEntry.Id == entry.Id).FirstOrDefaultAsync();
            if (tempEntry != null)
            {
                return tempEntry;
            }

            string newHash;
            while(true)
            {
                newHash = GetRandomHash();
                CDNTempEntry existingHash = await Context.CDNTempEntries.AsQueryable().Where(x => x.Hash == newHash).FirstOrDefaultAsync();
                if (existingHash == null)
                {
                    break;
                }
            }

            CDNTempEntry newEntry = new()
            {
                CDNEntry = entry,
                Hash = newHash,
                ValidUntil = validUntil
            };

            Context.CDNTempEntries.Add(newEntry);
            await Context.SaveChangesAsync();

            return newEntry;
        }

        public async Task<CDNTempEntry> RenewTempEntry(int entryId, DateTime validUntil)
        {
            CDNTempEntry tempEntry = await Context.CDNTempEntries.Include(x => x.CDNEntry).AsQueryable().Where(x => x.CDNEntry.Id == entryId).FirstOrDefaultAsync();
            if (tempEntry == null)
            {
                throw new ResourceNotFoundException();
            }

            tempEntry.ValidUntil = validUntil;

            Context.CDNTempEntries.Update(tempEntry);
            await Context.SaveChangesAsync();

            return tempEntry;
        }

        public async Task DeleteEntry(CDNTempEntry tempEntry)
        {
            Context.CDNTempEntries.Remove(tempEntry);
            await Context.SaveChangesAsync();
        }
        public async Task DeleteEntry(int entryId)
        {
            CDNTempEntry tempEntry = await Context.CDNTempEntries.Include(x => x.CDNEntry).AsQueryable().Where(x => x.CDNEntry.Id == entryId).FirstOrDefaultAsync();
            if (tempEntry == null)
            {
                throw new ResourceNotFoundException();
            }
            await DeleteEntry(tempEntry);
        }

        public async Task<int> DeleteOldEntries()
        {
            var oldEntries = await Context.CDNTempEntries.AsQueryable().Where(x => x.ValidUntil < DateTime.UtcNow).ToListAsync();
            Context.CDNTempEntries.RemoveRange(oldEntries);
            await Context.SaveChangesAsync();
            return oldEntries.Count;
        }
    }
}
