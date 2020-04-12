using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleS3.Core.Communication.Files;
using SimpleS3.Core.Communication.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleS3.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesRepository _filesRepository;
        public FilesController(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
        }

        [HttpPost]
        [Route("{bucketName}/add")]
        public async Task<ActionResult<AddFileResponse>> AddFiles([FromRoute] string bucketName, [FromForm] IList<IFormFile> formFiles)
        {
            if (formFiles == null)
                return BadRequest("No files were sent.");

            var response = await _filesRepository.UploadFiles(bucketName, formFiles);

            if (response == null)
                return BadRequest();

            return Ok(response);
        }

        [HttpGet]
        [Route("{bucketName}/list")]
        public async Task<ActionResult<IEnumerable<ListFilesResponse>>> ListFiles([FromRoute] string bucketName)
        {
            var response = await _filesRepository.ListFiles(bucketName);

            return Ok(response);
        }

        [HttpGet]
        [Route("{bucketName}/download/{fileName}")]
        public async Task<IActionResult> DownloadFile([FromRoute] string bucketName, [FromRoute] string fileName)
        {
            await _filesRepository.DownloadFile(bucketName, fileName);

            return Ok();
        }

        [HttpDelete]
        [Route("{bucketName}/delete/{fileName}")]
        public async Task<ActionResult<DeleteFileReponse>> DeleteFile([FromRoute] string bucketName, [FromRoute] string fileName)
        {
            var response = await _filesRepository.DeleteFile(bucketName, fileName);

            return Ok(response);
        }

        [HttpPost]
        [Route("{bucketName}/addjsonobject")]
        public async Task<IActionResult> AddJsonObject([FromRoute] string bucketName, [FromBody] AddJsonObjectRequest request)
        {
            await _filesRepository.AddJsonObject(bucketName, request);

            return Ok();
        }

        [HttpGet]
        [Route("{bucketName}/getjsonobject")]
        public async Task<ActionResult<GetJsonObjectResponse>> GetJsonObject([FromRoute] string bucketName, [FromQuery] string fileName)
        {
            var response =  await _filesRepository.GetJsonObject(bucketName, fileName);

            return Ok(response);
        }
    }
}