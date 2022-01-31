using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CDN.Models;
using CDN.Repositories;
using CDN.Models.DTO;
using CDN.Exceptions;

namespace CDN.Controllers
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

            CDNEntry currentItem = await repo.GetEntry(id);

            if (currentUser.Name != currentItem.UploadedBy.Name && !currentUser.IsAdmin)
            {
                throw new UnauthorizedException();
            }

            List<CDNEntry> items = await repo.GetItemsInDirectory(id);

            List<CDNEntry> parents = new();
            CDNEntry currentEntry = currentItem;
            while (true)
            {
                parents.Insert(0, currentEntry);
                if (currentEntry.Parent == null) break;
                currentEntry = await repo.GetEntry(currentEntry.Parent.Id);
            }

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

            CDNEntry currentItem = await repo.GetEntry(id);

            if (currentUser.Name != currentItem.UploadedBy.Name && !currentUser.IsAdmin)
            {
                throw new UnauthorizedException();
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
