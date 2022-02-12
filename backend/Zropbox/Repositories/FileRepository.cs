using Zropbox.Exceptions;
using Zropbox.Models;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using FuzzySharp;

namespace Zropbox.Repositories
{
    public class FileRepository : BaseRepository<FileRepository>
    {
        private FileRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public static FileRepository CreateDefault(IServiceProvider serviceProvider)
        {
            return new FileRepository(serviceProvider);
        }

        public string GetContentType(string path)
        {
            new FileExtensionContentTypeProvider().TryGetContentType(path, out string contentType);
            return contentType ?? "application/octet-stream";
        }
        public async Task<FileServe> GetFile(int entryId)
        {
            CDNEntry entry = await DirectoryRepository.CreateDefault(ServiceProvider).GetEntry(entryId);
            if (entry == null)
            {
                throw new ResourceNotFoundException();
            }
            return await GetFile(entry);
        }

        public async Task<FileServe> GetFile(CDNEntry entry)
        {
            if (entry == null || entry.IsDir)
            {
                throw new ResourceNotFoundException();
            }

            string filePath = Path.Join(Config.GetFileRootPath(), entry.Id.ToString());
            string fullFilePath = Path.GetFullPath(filePath);
            // https://stackoverflow.com/a/1321535/9850709
            if (fullFilePath != filePath)
            {
                throw new ResourceNotFoundException();
            }

            byte[] fileData;
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new ResourceNotFoundException();
                }
                fileData = await File.ReadAllBytesAsync(filePath);
                if (fileData == null)
                {
                    throw new ResourceNotFoundException();
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to read file");
                throw new ResourceNotFoundException();
            }

            string contentType = GetContentType(entry.Name);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = entry.Name,
                Inline = true,
            };

            return new FileServe
            {
                Name = entry.Name,
                ContentType = contentType,
                ContentDisposition = cd,
                FileContent = fileData
            };
        }

        public async Task<CDNEntry> UploadFile(IFormFile file, int parentId, User user, bool isPublic, string fileName)
        {
            CDNEntry parentEntry = null;
            if (parentId != 0)
            {
                parentEntry = await DirectoryRepository.CreateDefault(ServiceProvider).GetEntry(parentId);
                if (parentEntry == null || !parentEntry.IsDir)
                {
                    throw new ResourceNotFoundException();
                }
            }

            CDNEntry newEntry = new()
            {
                Name = fileName,
                IsDir = false,
                IsPublic = isPublic,
                Parent = parentEntry,
                Size = (int)file.Length,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = user
            };

            Context.CDNEntries.Add(newEntry);
            await Context.SaveChangesAsync();

            string filePath = Path.Join(Config.GetFileRootPath(), newEntry.Id.ToString());
            string fullFilePath = Path.GetFullPath(filePath);
            // https://stackoverflow.com/a/1321535/9850709
            if (fullFilePath != filePath)
            {
                throw new ResourceNotFoundException();
            }

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            return newEntry;
        }

        public async Task DeleteEntry(int entryId)
        {
            CDNEntry entry = await DirectoryRepository.CreateDefault(ServiceProvider).GetEntry(entryId);

            if (!entry.IsDir)
            {
                string filePath = Path.Join(Config.GetFileRootPath(), entryId.ToString());
                string fullFilePath = Path.GetFullPath(filePath);
                // https://stackoverflow.com/a/1321535/9850709
                if (fullFilePath != filePath)
                {
                    throw new ResourceNotFoundException();
                }

                try
                {
                    File.Delete(filePath);
                } catch (Exception ex)
                {
                    Logger.LogError(ex, $"Failed to delete file {entryId}");
                }
            }

            Context.CDNEntries.Remove(entry);
            await Context.SaveChangesAsync();
        }

        public async Task<CDNEntry> EditEntry(CDNEntry newEntry)
        {
            Context.CDNEntries.Update(newEntry);
            await Context.SaveChangesAsync();

            return newEntry;
        }

        public async Task<CDNEntry> MoveEntryToRoot(CDNEntry entry)
        {
            if (entry.Parent == null)
            {
                return entry;
            }

            // execute raw sql to avoid EF cascade delete
            // rage counter: 1
            await Context.Database.ExecuteSqlRawAsync($"UPDATE {nameof(Context.CDNEntries)} SET ParentId = NULL WHERE Id = {entry.Id}");

            return entry;
        }

        public async Task<List<CDNEntry>> ExecuteFuzzySearch(User user, string search)
        {
            List<CDNEntry> entries = await Context.CDNEntries.Include(x => x.Parent).Include(x => x.UploadedBy).AsQueryable().Where(x => x.UploadedBy == user && ! x.IsDir).ToListAsync();

            string[] entriesMapped = entries.Select(x => x.Name).ToArray();

            // https://github.com/JakeBayer/FuzzySharp#process
            var searchResults = Process.ExtractTop(search, entriesMapped, cutoff: 40, limit: 50);

            return searchResults.Select(x => entries[x.Index]).ToList();
        }
    }
}
