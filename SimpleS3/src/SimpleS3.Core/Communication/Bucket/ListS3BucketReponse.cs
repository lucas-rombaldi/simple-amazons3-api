using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleS3.Core.Communication.Bucket
{
    public class ListS3BucketReponse
    {
        public string BucketName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
