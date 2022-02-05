using Zropbox.Exceptions;
using Zropbox.Models;
using Microsoft.EntityFrameworkCore;

namespace Zropbox.Repositories
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
            CDNEntry entry = await Context.CDNEntries.Include(x => x.Parent).Include(x => x.UploadedBy).AsQueryable().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entry == null)
            {
                throw new ResourceNotFoundException();
            }
            return entry;
        }

        public async Task<List<CDNEntry>> GetItemsInDirectory(int id)
        {
            return await Context.CDNEntries.Include(x => x.Parent).Include(x => x.CDNTempEntry).AsQueryable().OrderByDescending(x => x.UploadedAt).Where(x => x.Parent != null && x.Parent.Id == id).ToListAsync();
        }

        public async Task<List<CDNEntry>> GetItemsInRootDirectory(int userId)
        {
            return await Context.CDNEntries.Include(x => x.Parent).Include(x => x.UploadedBy).Include(x => x.CDNTempEntry).AsQueryable().OrderByDescending(x => x.UploadedAt).Where(x => x.UploadedBy != null && x.UploadedBy.Id == userId && x.Parent == null).ToListAsync();
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
