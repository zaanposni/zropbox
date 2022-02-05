using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zropbox.Models;
using Zropbox.Repositories;
using Zropbox.Exceptions;

namespace Zropbox.Controllers
{
    [ApiController]
    public class TempAccessController : SimpleController<TempAccessController>
    {
        public TempAccessController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpGet("share/{hash}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetFile([FromRoute] string hash)
        {
            CDNEntry entry = await TempEntryRepository.CreateDefault(ServiceProvider).GetEntryByHash(hash);

            FileServe fileInfo = await FileRepository.CreateDefault(ServiceProvider).GetFile(entry);

            HttpContext.Response.Headers.Add("Content-Disposition", fileInfo.ContentDisposition.ToString());
            HttpContext.Response.Headers.Add("Content-Type", fileInfo.ContentType);

            return File(fileInfo.FileContent, fileInfo.ContentType);
        }

        [HttpPost("temp/{id}")]
        [Authorize]
        public async Task<IActionResult> CreateTempAccess([FromRoute] int entryId)
        {
            await ValidateLogin();
            User currentUser = await GetCurrentUser();

            CDNEntry entry = await DirectoryRepository.CreateDefault(ServiceProvider).GetEntry(entryId);

            if (entry.UploadedBy != currentUser && !currentUser.IsAdmin)
            {
                throw new UnauthorizedException();
            }
            if (entry.IsDir)
            {
                return BadRequest();
            }

            CDNTempEntry tempEntry = await TempEntryRepository.CreateDefault(ServiceProvider).CreateTempAccess(entry, DateTime.UtcNow.AddDays(1));

            return Ok(new TempEntryView(tempEntry));
        }

        [HttpPut("temp/{id}")]
        [Authorize]
        public async Task<IActionResult> RenewTempAccess([FromRoute] int entryId)
        {
            await ValidateLogin();
            User currentUser = await GetCurrentUser();

            CDNEntry entry = await DirectoryRepository.CreateDefault(ServiceProvider).GetEntry(entryId);

            if (entry.UploadedBy != currentUser && !currentUser.IsAdmin)
            {
                throw new UnauthorizedException();
            }
            if (entry.IsDir)
            {
                return BadRequest();
            }

            CDNTempEntry tempEntry = await TempEntryRepository.CreateDefault(ServiceProvider).RenewTempEntry(entryId, DateTime.UtcNow.AddDays(1));

            return Ok(new TempEntryView(tempEntry));
        }

        [HttpDelete("temp/{entryId}")]
        [Authorize]
        public async Task<IActionResult> RemoveTempAccess([FromRoute] int entryId)
        {
            await ValidateLogin();
            User currentUser = await GetCurrentUser();

            CDNEntry entry = await DirectoryRepository.CreateDefault(ServiceProvider).GetEntry(entryId);

            if (entry.UploadedBy != currentUser && !currentUser.IsAdmin)
            {
                throw new UnauthorizedException();
            }
            if (entry.IsDir)
            {
                return BadRequest();
            }

            await TempEntryRepository.CreateDefault(ServiceProvider).DeleteEntry(entryId);

            return Ok();
        }
    }
}
