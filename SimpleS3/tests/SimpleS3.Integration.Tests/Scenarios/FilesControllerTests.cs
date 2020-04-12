using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using SimpleS3.Api;
using SimpleS3.Core.Communication.Files;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleS3.Integration.Tests.Scenarios
{
    [Collection("api")]
    public class FilesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly string FILE_NAME = "IntegrationTest.jpg";

        private readonly HttpClient _client;

        public FilesControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAWSService<IAmazonS3>(new AWSOptions()
                    {
                        DefaultClientConfig =
                        {
                            ServiceURL = "http://localhost:9003"
                        },
                        Credentials = new BasicAWSCredentials("FAKE", "FAKE")
                    });
                });
            }).CreateClient();

            Task.Run(CreateBucket).Wait();
        }

        private async Task CreateBucket()
        {
            var uri = "api/bucket/create/testS3Bucket";
            var content = new StringContent("testS3Bucket");
            await _client.PostAsync(uri, content);
        }

        [Fact]
        public async Task should_return_ok_on_addfiles_endpoint()
        {
            var response = await UploadFileToS3Bucket();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task should_return_not_null_result_on_listing_files()
        {
            await UploadFileToS3Bucket();

            var response = await _client.GetAsync("api/files/testS3Bucket/list");

            ListFilesResponse[] result;
            using (var content = response.Content.ReadAsStringAsync())
            {
                result = JsonConvert.DeserializeObject<ListFilesResponse[]>(await content);
            }

            Assert.NotNull(result);
        }

        [Fact]
        public async Task should_return_ok_on_download_files()
        {
            await UploadFileToS3Bucket();

            var response = await _client.GetAsync($"api/files/testS3Bucket/download/{FILE_NAME}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task should_return_ok_on_delete_file()
        {
            await UploadFileToS3Bucket();

            var response = await _client.DeleteAsync($"api/files/testS3Bucket/delete/{FILE_NAME}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task should_return_ok_on_adding_json_object()
        {
            var jsonObjectRequest = new AddJsonObjectRequest()
            {
                Id = Guid.NewGuid(),
                Data = "Test-data",
                TimeSent = DateTime.UtcNow
            };

            var jsonObject = JsonConvert.SerializeObject(jsonObjectRequest);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/files/testS3Bucket/addjsonobject", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private async Task<HttpResponseMessage> UploadFileToS3Bucket()
        {
            var tempPath = Path.GetTempPath();
            var fullPath = $"{tempPath}{FILE_NAME}";
            var file = File.Create(fullPath);

            using HttpContent fileStreamContent = new StreamContent(file);
            using (var formData = new MultipartFormDataContent()
                {
                    { fileStreamContent, "formFiles", FILE_NAME }
                })
            {
                var response = await _client.PostAsync("api/files/testS3Bucket/add", formData);
                return response;
            }
        }
    }
}
