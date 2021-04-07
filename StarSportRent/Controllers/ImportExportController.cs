using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImportExportController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IImportExport importExport;

        public ImportExportController(IRepository repository, IImportExport importExport)
        {
            this.repository = repository;
            this.importExport = importExport;
        }


        [HttpPost]
        public async Task<IActionResult> Import([FromForm]IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
                    using (FileStream fileStream = System.IO.File.Create(fileName))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                    if(await this.importExport.Import(file.FileName))
                    {
                        System.IO.File.Delete(fileName);
                        return this.Ok();
                    }
                    else 
                    {
                        System.IO.File.Delete(fileName);
                        return this.NotFound(new ErrorMessage { message = "Error" });
                    }
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
            
        }

        [HttpGet]
        public async Task<FileResult> Export()
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    byte[] cintent = await importExport.Export();
                    return File(cintent,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Db.xlsx");
                }
                throw new Exception();
            }
            else
            {
                throw new Exception();
            }
        }
        public string TokenFromHeader(HttpRequest request)
        {
            var re = Request;
            var headers = re.Headers;
            string token = "";
            if (headers.ContainsKey("token"))
            {
                token = headers["token"];
            }
            return token;
        }
    }
}
