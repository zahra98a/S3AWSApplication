using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace S3AWSApplication.Models
{
    public class S3Response
    {
        public HttpStatusCode statusCode { get; set; }
        public string Message { get; set; }
    }
}
