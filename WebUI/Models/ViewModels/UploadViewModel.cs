using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models.ViewModels
{
    public class UploadViewModel
    {
        [Required(ErrorMessage = "Please select a file to upload.")]
        public IFormFile File { set; get; }

        public bool Success { get; set; }

        public string FullPath { get; set; }

    }
}
