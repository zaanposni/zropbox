using CDN.Exceptions;
using CDN.Models;
using Microsoft.EntityFrameworkCore;

namespace CDN.Repositories
{
    public class DirectoryRepository : BaseRepository<DirectoryRepository>
    {
        private DirectoryRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public static DirectoryRepository CreateDefault(IServiceProvider serviceProvider)
        {
            return new DirectoryRepository(serviceProvider);
        }

        public async Task<CDNEntry> GetEntry(int id)
        {
            CDNEntry entry = await Context.CDNEntries.AsQueryable().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entry == null)
            {
                throw new ResourceNotFoundException();
            }
            return entry;
        }

        public async Task<List<CDNEntry>> GetItemsInDirectory(int id)
        {
            return await Context.CDNEntries.AsQueryable().Where(x => x.Id == id).ToListAsync();
        }

        public async Task<CDNEntry> CreateSubDir(CDNEntry entry, int parentId = 0)
        {
            if (parentId == 0)
            {
                entry.Parent = null;
            }
            else
            {
                CDNEntry parentDir = await GetEntry(parentId);
                entry.Parent = parentDir;
            }

            entry.Size = 0;
            entry.IsDir = true;
            entry.UploadedAt = DateTime.UtcNow;

            Context.CDNEntries.Add(entry);
            await Context.SaveChangesAsync();

            return entry;
        }

        public async Task DeleteDir(int id)
        {
            CDNEntry dir = await GetEntry(id);

            Context.CDNEntries.Remove(dir);
            await Context.SaveChangesAsync();
        }
    }
}
