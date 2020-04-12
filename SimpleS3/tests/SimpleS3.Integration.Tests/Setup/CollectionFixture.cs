using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimpleS3.Integration.Tests.Setup
{
    [CollectionDefinition("api")]
    public class CollectionFixture : ICollectionFixture<TestContext>
    {
    }
}
