using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController( FileExtensionContentTypeProvider fileExtensionContentTypeProvider )
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }


        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var filePath = "filetodownload.pdf";
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            if(!_fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);

            return File(bytes, contentType, Path.GetFileName(filePath));

        }

        [HttpPost]
        public async Task<ActionResult> CreateFile(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }

            if(file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf")
            {
                return BadRequest("No file or an invalid file has ben inputted");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), $"uploaded_file_{Guid.NewGuid()}.pdf");

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("File has been uploaded successfully.");
           
            
        }
    }
}
