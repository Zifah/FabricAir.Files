﻿using Microsoft.AspNetCore.Mvc;
using FabricAir.Files.Api.Model;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using FabricAir.Files.Data.Entities;
using File = FabricAir.Files.Data.Entities.File;
using FabricAir.Files.Data.Repositories;
using FabricAir.Files.Common;
using System.ComponentModel.DataAnnotations;

namespace FabricAir.Files.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly FileRepository _fileRepository;
        public FilesController(FileRepository fileRepository)
        {
            Require.NotNull(fileRepository, nameof(fileRepository));
            _fileRepository = fileRepository;
        }

        [HttpGet("Users/{emailAddress}")]
        [SwaggerOperation("Fetch all files that a user has access to")]
        [Authorize(Roles = Constants.UserRoleAdministrator)]
        [ProducesDefaultResponseType(typeof(IEnumerable<FileDTO>))]
        public async Task<IActionResult> GetUserFiles([FromRoute][EmailAddress] string emailAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userFiles = await _fileRepository.GetFilesAsync(emailAddress);
            return Ok(ToDTO(userFiles));
        }

        [HttpGet]
        [SwaggerOperation("Fetch all files that the authenticated user has access to")]
        [ProducesDefaultResponseType(typeof(IEnumerable<FileDTO>))]
        public async Task<IActionResult> GetFiles()
        {
            var userFiles = await _fileRepository.GetFilesAsync(GetUserEmail());
            return Ok(ToDTO(userFiles));
        }

        [HttpGet("Groups/Users/{emailAddress}")]
        [SwaggerOperation("Fetch all file-groups that a user has access to")]
        [Authorize(Roles = Constants.UserRoleAdministrator)]
        [ProducesDefaultResponseType(typeof(IEnumerable<FileGroupDTO>))]
        public async Task<IActionResult> GetUserFileGroups([FromRoute][EmailAddress]string emailAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userFileGroups = await _fileRepository.GetFileGroupsAsync(emailAddress);
            return Ok(ToDTO(userFileGroups));
        }

        [HttpGet("Groups")]
        [SwaggerOperation("Fetch all file-groups that the authenticated user has access to")]
        [ProducesDefaultResponseType(typeof(IEnumerable<FileGroupDTO>))]
        public async Task<IActionResult> GetFileGroups()
        {
            var userFileGroups = await _fileRepository.GetFileGroupsAsync(GetUserEmail());
            return Ok(ToDTO(userFileGroups));
        }

        private string GetUserEmail()
        {
            return User!.Identity!.Name!;
        }
        private static IEnumerable<FileGroupDTO> ToDTO(IEnumerable<FileGroup> fileGroups) =>
            fileGroups.Select(fg => new FileGroupDTO(fg.Name));

        private static IEnumerable<FileDTO> ToDTO(IEnumerable<File> files) =>
            files.Select(f => new FileDTO(f.Name, f.Group.Name, f.URL));
    }
}
