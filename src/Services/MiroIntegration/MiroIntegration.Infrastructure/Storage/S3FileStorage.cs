using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using MiroIntegration.Application.Abstractions;

namespace MiroIntegration.Infrastructure.Storage;

public sealed class S3FileStorage(
    IAmazonS3 s3Client,
    IOptions<S3Options> options) : IPdfFileStorage
{
    private readonly S3Options _options = options.Value;

    public async Task<Stream> OpenReadAsync(
        string objectKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(objectKey))
            throw new ArgumentException("Object key cannot be empty.", nameof(objectKey));

        var response = await s3Client.GetObjectAsync(
            new GetObjectRequest
            {
                BucketName = _options.BucketName,
                Key = objectKey
            },
            cancellationToken);

        return response.ResponseStream;
    }
}
