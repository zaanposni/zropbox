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


namespace CDN.Controllers.v1
{
    [Route("api/files")]
    [ApiController]
    [Authorize]
    public class FileController : SimpleController<FileController>
    {
        public FileController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetFile([FromRoute] int id)
        {
            await ValidateLogin();
            User currentUser = await GetCurrentUser();

            CDNEntry entry = await DirectoryRepository.CreateDefault(ServiceProvider).GetEntry(id);
            if (entry.IsDir)
            {
                return BadRequest();
            }
            if (entry.UploadedBy != currentUser && !currentUser.IsAdmin && !entry.IsPublic)
            {
                throw new UnauthorizedException();
            }

            FileServe fileInfo = await FileRepository.CreateDefault(ServiceProvider).GetFile(id);

            HttpContext.Response.Headers.Add("Content-Disposition", fileInfo.ContentDisposition.ToString());
            HttpContext.Response.Headers.Add("Content-Type", fileInfo.ContentType);

            return File(fileInfo.FileContent, fileInfo.ContentType);
        }

        [HttpPost("{id}")]
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

            return Ok(new DirectoryItemView(await FileRepository.CreateDefault(ServiceProvider).UploadFile(file.File, id, currentUser, file.IsPublic)));
        }
    }
}