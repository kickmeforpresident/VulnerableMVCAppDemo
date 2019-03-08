using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace WebUI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly string _webRootPath;
        private readonly string _publicUploadFolder;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _publicUploadFolder = _configuration.GetSection("AppSettings").GetSection("PublicUploadFolder").Value;
            _webRootPath = environment.WebRootPath;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            CreatePublicUploadFolder();

            string pathWithFolderName = CreateRandomFolder();

            CreateRandomFile(pathWithFolderName);
        }        

        private void CreatePublicUploadFolder()
        {
            var standardUploadFolder = $"{_webRootPath}/{_publicUploadFolder}/";
            Directory.CreateDirectory(standardUploadFolder);
        }

        private string CreateRandomFolder()
        {
            var randomFolderName = Guid.NewGuid().ToString();
            var pathWithFolderName = $"{_webRootPath}/{randomFolderName}/";
            Directory.CreateDirectory(pathWithFolderName);
            return pathWithFolderName;
        }

        private void CreateRandomFile(string pathWithFolderName)
        {
            var randomFileName = Guid.NewGuid().ToString();
            var extension = ".txt";
            var fullPath = $"{pathWithFolderName}{randomFileName}{extension}";
            using (var sw = new StreamWriter(fullPath))
            {
                var randomText = Guid.NewGuid().ToString();
                sw.WriteLine(randomText);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
