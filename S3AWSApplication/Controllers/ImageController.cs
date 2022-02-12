using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using S3AWSApplication.Service;

namespace S3AWSApplication.Controllers
{
    [Route("api/[controller]")]// for create / delete file inside the bucket 
   // [Route("api/S3Bucket")]// for create / delete bucket 
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IS3Service _service;
        public ImageController(IS3Service service)
        {
              _service = service;
        }

        // upload file to S3
        [HttpPost]
        [Route("AddFile/{bucketName}")]
        public async Task<IActionResult> AddFile([FromRoute] string bucketName)
        {
            await _service.UploadFileAsync(bucketName);
            return Ok();
        }
        // delete file from S3
        [HttpDelete]
        [Route("DeleteFile/{bucketName}")]
        public async Task<IActionResult> DeleteFile([FromRoute] string bucketName)
        {
             await  _service.DeleteObjectAsync(bucketName);
            return Ok();
        }

        //create S3 buckets
        [HttpPost("CreateBucket/{bucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string bucketName)
        {
          //  var response = await amazons3.PutBucketAsync(bucketName);
            var response = await _service.CreateBucketAsync(bucketName);
            return Ok(response);
        }
        // delete bucket
        [HttpDelete]
        [Route("DeleteBucket/{bucketName}")]
        public async Task<IActionResult> DeleteBucket([FromRoute] string bucketName)
        {
            var response = await _service.DeleteBucketAsync(bucketName);
            return Ok(response);
        }
    }
}
