using SimpleS3.Core.Communication.Bucket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleS3.Core.Communication.Interfaces
{
    public interface IBucketRepository
    {
        Task<bool> DoesS3BucketExists(string bucketName);

        Task<CreateBucketResponse> CreateBucket(string bucketName);

        Task<IEnumerable<ListS3BucketReponse>> ListBuckets();

        Task DeleteBucket(string bucketName);
    }
}
