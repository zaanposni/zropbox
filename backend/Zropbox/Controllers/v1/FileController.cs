using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zropbox.Models;
using Zropbox.Repositories;
using Zropbox.Exceptions;


namespace Zropbox.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : SimpleController<FileController>
    {
        public FileController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetFile([FromRoute] int id)
        {

            CDNEntry entry = await DirectoryRepository.CreateDefault(ServiceProvider).GetEntry(id);
            if (entry.IsDir)
            {
                throw new ResourceNotFoundException();
            }
            if (!entry.IsPublic)
            {
                await ValidateLogin();
                User currentUser = await GetCurrentUser();
                if (entry.UploadedBy != currentUser && !currentUser.IsAdmin)
                {
                    throw new UnauthorizedException();
                }
            }

            FileServe fileInfo = await FileRepository.CreateDefault(ServiceProvider).GetFile(id);

            HttpContext.Response.Headers.Add("Content-Disposition", fileInfo.ContentDisposition.ToString());
            HttpContext.Response.Headers.Add("Content-Type", fileInfo.ContentType);

            return File(fileInfo.FileContent, fileInfo.ContentType);
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> UploadedFile([FromRoute] int id, [FromForm] FileUpload file)
        {
            await ValidateLogin();
            User currentUser = await GetCurrentUser();

            if (id != 0)
            {
                CDNEntry dirEntry = await DirectoryRepository.CreateDefault(ServiceProvider).GetEntry(id);
                if (!dirEntry.IsDir)
                {
                    return BadRequest();
                }
                if (dirEntry.UploadedBy != currentUser && !currentUser.IsAdmin)
                {
                    throw new UnauthorizedException();
                }
            }

            return Ok(new DirectoryItemView(await FileRepository.CreateDefault(ServiceProvider).UploadFile(file.File, id, currentUser, file.IsPublic, file.Name)));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFile([FromRoute] int id)
        {
            await ValidateLogin();
            User currentUser = await GetCurrentUser();

            if (id == 0)
            {
                return BadRequest();
            }

            CDNEntry dirEntry = await DirectoryRepository.CreateDefault(ServiceProvider).GetEntry(id);
            if (dirEntry.UploadedBy != currentUser && !currentUser.IsAdmin)
            {
                throw new UnauthorizedException();
            }

            await FileRepository.CreateDefault(ServiceProvider).DeleteEntry(id);

            return Ok();

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditFile([FromRoute] int id, [FromBody] FileEditDto dto)
        {
            await ValidateLogin();
            User currentUser = await GetCurrentUser();
            if (id == 0)
            {
                return BadRequest();
            }

            CDNEntry entry = await DirectoryRepository.CreateDefault(ServiceProvider).GetEntry(id);
            if (entry.UploadedBy != currentUser && !currentUser.IsAdmin)
            {
                throw new UnauthorizedException();
            }

            entry.Name = dto.Name;
            entry.IsPublic = dto.IsPublic;

            return Ok(new DirectoryItemView(await FileRepository.CreateDefault(ServiceProvider).EditEntry(entry)));
        }
    }
}