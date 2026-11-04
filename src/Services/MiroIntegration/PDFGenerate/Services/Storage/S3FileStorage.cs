using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using PdfWorker.Configuration;

namespace PdfWorker.Services.Storage;

public sealed class S3FileStorage
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Options _options;

    public S3FileStorage(
     IAmazonS3 s3Client,
     IOptions<S3Options> options)
    {
        _s3Client = s3Client;
        _options = options.Value;
    }
    public async Task UploadAsync(
        byte[] file,
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(file);

        var request = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = objectKey,
            InputStream = stream,
            ContentType = "application/pdf"
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);
    }
}