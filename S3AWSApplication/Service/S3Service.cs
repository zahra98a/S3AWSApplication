using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using S3AWSApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace S3AWSApplication.Service
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;

        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }
        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
                if(await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName) == false)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };
                    var response = await _client.PutBucketAsync(putBucketRequest);
                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        statusCode = response.HttpStatusCode
                    };
                }
            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    statusCode = e.StatusCode,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    statusCode = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
            return new S3Response
            {
                statusCode = HttpStatusCode.InternalServerError,
                Message = "Somethings went wrong"
            };
        }

        public async Task<S3Response> DeleteBucketAsync(string bucketName)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName) == false)
                {
                    var deleteBucketRequest = new DeleteBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };
                    var response = await _client.DeleteBucketAsync(deleteBucketRequest.BucketName);
                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        statusCode = response.HttpStatusCode
                    };
                }
            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    statusCode = e.StatusCode,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    statusCode = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
            return new S3Response
            {
                statusCode = HttpStatusCode.InternalServerError,
                Message = "Somethings went wrong"
            };
        }


        private const string FilePath = "C:\\Users\\S00175911\\Documents\\fileToUpload.txt"; // file to be uploaded to s3
        private const string UploadWithKeyName = "UploadWithKeyName";
        private const string FileStreamUpload = "FileStreamUpload";
        private const string AdvancedUpload = "AdvancedUpload";
        
        public async Task UploadFileAsync(string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);

               //option 1
                await fileTransferUtility.UploadAsync(FilePath, bucketName);

                //option 2
                await fileTransferUtility.UploadAsync(FilePath, bucketName, UploadWithKeyName);

                //option 3
                using ( var fileToUpload = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload, bucketName, FileStreamUpload);
                }

                //option 4
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    FilePath = FilePath,
                    StorageClass = S3StorageClass.Standard,
                    PartSize = 6291456, //6mb
                    Key = AdvancedUpload,
                    CannedACL = S3CannedACL.NoACL
                };
                fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }
            catch (AmazonS3Exception e)
            {

                Console.WriteLine("Error encountered on server, '{0}'", e.Message);
            }
            catch (Exception e)
            {

                Console.WriteLine("Error encountered on server, '{0}'", e.Message);
            }
           
        }

        public async Task DeleteObjectAsync(string bucketName)
        {
            const string KeyName = "fileToUpload.txt"; //file to be deleted from s3 bucket
            try
            {


                var request = new DeleteObjectRequest { 
                BucketName = bucketName,
                Key = KeyName
                };

                var response = await _client.DeleteObjectAsync(request);
            }
            catch (AmazonS3Exception e)
            {

                Console.WriteLine("Error encountered on server, '{0}'", e.Message);
            }
            catch (Exception e)
            {

                Console.WriteLine("Error encountered on server, '{0}'", e.Message);
            }

        }
    }
}
