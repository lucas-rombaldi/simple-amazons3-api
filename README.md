# Simple AmazonS3 
This is a simple WebAPI developed in ASP.NET Core 3.1 with the goal to simplify the use of AmazonS3 basic services, like managing buckets and its files. Considering that the Swagger UI is configured, you may check that it's really simple to use the API.

## Release Notes
### V1.0
This is a very basic solution to use the AmazonS3 service, considering the consumer just have an available account on AmazonAWS and knows the concept of buckets.
There are many things to improve to next versions (be my guest), like:
- Adjust Swagger UI for uploading an array of IFormFiles (this is a known issue).
- Add more unit and integration tests.
- And so on...

## Requirements

### Amazon
This solution requires an active account on Amazon AWS and a user with permissions to access the AmazonS3 service. Thus, it is needed that the credentials file exists in the default folder to use the correct IAM Role.

### Automated Test
Due to the use of the `Localstack` Amazon Services image from Docker to run the integration tests in the solution, the docker service must be available in the local machine.