using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zropbox.Models;
using Zropbox.Repositories;
using Zropbox.Models.DTO;
using Zropbox.Exceptions;

namespace Zropbox.Controllers
{
    [Route("api/directory")]
    [ApiController]
    [Authorize]
    public class DirectoryController : SimpleController<DirectoryController>
    {
        public DirectoryController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDirectory([FromRoute] int id)
        {
            await ValidateLogin();
            User currentUser = await GetCurrentUser();

            DirectoryRepository repo = DirectoryRepository.CreateDefault(ServiceProvider);

            // Auth check
            if (id != 0)
            {
                CDNEntry currentItem = await repo.GetEntry(id);
                if (!currentItem.IsDir)
                {
                    return BadRequest();
                }
                if (currentUser != currentItem.UploadedBy && !currentUser.IsAdmin)
                {
                    throw new UnauthorizedException();
                }
            }

            // Sub items for directory
            List<CDNEntry> items = new();
            if (id == 0)
            {
                items = await repo.GetItemsInRootDirectory(currentUser.Id);
            }
            else
            {
                items = await repo.GetItemsInDirectory(id);
            }

            // Hierarchy
            List<CDNEntry> parents = new();
            if (id != 0)
            {
                CDNEntry currentEntry = await repo.GetEntry(id);
                while (true)
                {
                    parents.Insert(0, currentEntry);
                    if (currentEntry.Parent == null) break;
                    currentEntry = await repo.GetEntry(currentEntry.Parent.Id);
                }
            }

            // Create view
            return Ok(new DirectoryView()
            {
                CurrentItemId = id,
                Hierarchy = parents.Select(x => new HierarchyItemView(x)).ToList(),
                Items = items.Select(x => new DirectoryItemView(x)).ToList()
            });
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> CreateDirectory([FromRoute] int id, [FromBody] DirectoryCreateDto dto)
        {
            await ValidateLogin();
            User currentUser = await GetCurrentUser();

            DirectoryRepository repo = DirectoryRepository.CreateDefault(ServiceProvider);

            if (id != 0)
            {
                CDNEntry currentItem = await repo.GetEntry(id);

                if (currentUser != currentItem.UploadedBy && !currentUser.IsAdmin)
                {
                    throw new UnauthorizedException();
                }
            }

            CDNEntry newEntry = new()
            {
                Name = dto.Name,
                IsDir = true,
                Size = 0,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = currentUser,
                IsPublic = dto.IsPublic,
            };

            return Ok(new DirectoryItemView(await repo.CreateSubDir(newEntry, id)));
        }
    }
}
