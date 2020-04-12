using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleS3.Core.Communication.Bucket
{
    public class CreateBucketResponse
    {
        public string RequestId { get; set; }
        public string BucketName { get; set; }
    }
}
