using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleS3.Core.Communication.Bucket;
using SimpleS3.Core.Communication.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleS3.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketController : ControllerBase
    {
        private readonly IBucketRepository _bucketRepository;

        public BucketController(IBucketRepository bucketRepository)
        {
            _bucketRepository = bucketRepository;
        }

        [HttpPost]
        [Route("create/{bucketName}")]
        public async Task<ActionResult<CreateBucketResponse>> CreateS3Bucket([FromRoute] string bucketName)
        {
            var bucketExists = await _bucketRepository.DoesS3BucketExists(bucketName);

            if (bucketExists) return BadRequest("The S3 Bucket already exists.");

            var result = await _bucketRepository.CreateBucket(bucketName);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<IEnumerable<ListS3BucketReponse>>> ListS3Buckets()
        {
            var result = await _bucketRepository.ListBuckets();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{bucketName}")]
        public async Task<IActionResult> DeleteS3Bucket([FromRoute] string bucketName)
        {
            var bucketExists = await _bucketRepository.DoesS3BucketExists(bucketName);

            if (!bucketExists) return NotFound();

            await _bucketRepository.DeleteBucket(bucketName);

            return Ok();
        }
    }
}