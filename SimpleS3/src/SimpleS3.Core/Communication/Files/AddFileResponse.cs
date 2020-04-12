using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleS3.Core.Communication.Files
{
    public class AddFileResponse
    {
        public IList<string> PreSignedURL { get; set; }
    }
}
