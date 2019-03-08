using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using WebUI.Models;
using WebUI.Models.ViewModels;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _environment;
        public readonly IConfiguration _configuration;

        private readonly string _host;
        private readonly string _port;
        private readonly string _publicUploadFolder;

        public HomeController(IHostingEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
            _host = _configuration.GetSection("AppSettings").GetSection("Hosting").GetSection("Host").Value;
            _port = _configuration.GetSection("AppSettings").GetSection("Hosting").GetSection("Port").Value;
            _publicUploadFolder = _configuration.GetSection("AppSettings").GetSection("PublicUploadFolder").Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            var model = new UploadViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Upload(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var uniqueFileName = GenerateUniqueFileName(model.File.FileName);
                var physicalfilePath = GetPhysicalFilePath(uniqueFileName);

                SaveFile(model, physicalfilePath);

                var uploadedFilePath = GetUploadedFilePath(uniqueFileName);

                SetUpModelOnSuccess(model, uploadedFilePath);

                return View(model);
            }

            return View(model);
        }

        private string GenerateUniqueFileName(string fileName)
        {
            var guid = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(fileName);
            var uniqueFileName = guid + extension;

            return uniqueFileName;

        }

        private string GetUploadedFilePath(string uniqueFileName)
        {
            return $"{_host}:{_port}/{_publicUploadFolder}/{uniqueFileName}";
        }

        private string GetPhysicalFilePath(string uniqueFileName)
        {
            return Path.Combine(_environment.WebRootPath, _publicUploadFolder, uniqueFileName);
        }

        private void SaveFile(UploadViewModel model, string physicalfilePath)
        {
            using (var stream = new FileStream(physicalfilePath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }
        }

        private void SetUpModelOnSuccess(UploadViewModel model, string uploadedFilePath)
        {
            model.Success = true;
            model.FullPath = uploadedFilePath;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
