﻿using System.ComponentModel.DataAnnotations;

namespace Zropbox.Models.DTO
{
    public class FileUpload
    {
        [Required]
        [MaxLength(30)]
        public string FileName { get; set; }
        [Required]
        public IFormFile File { set; get; }
        [Required]
        public bool IsPublic { get; set; }
    }
}
