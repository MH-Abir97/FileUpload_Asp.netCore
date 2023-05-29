using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/Demo")]
    public class ImageController : Controller
    {
      
        private IWebHostEnvironment _webHostEnvironment;
        private IHttpContextAccessor httpContextAccessor;
        public ImageController(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor _httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            httpContextAccessor = _httpContextAccessor;
        }

        [Produces("application/json")]
        [HttpPost("Upload")]
        public IActionResult Upload(IFormFile[] files)
        {
            try
            {
                var baseUrl = httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host + httpContextAccessor.HttpContext.Request.PathBase;
                var fileInformation = new List<FileUpload>();

                foreach (var file in files)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", file.FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    fileInformation.Add(new FileUpload
                    {
                        FileName = Path.Combine(baseUrl, "uploads", file.FileName),
                        FileLength= file.Length,
                    });
                }

                return Ok(fileInformation);
              
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
