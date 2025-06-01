using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Mvc;
using S3Demo.Models;

namespace S3Demo.Controllers;

[Route("api/files")]
[ApiController]
public class FileController(IAmazonS3 s3Client) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
    {
        var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        if (!bucketExists)
        {
            return NotFound($"Bucket {bucketName} does not exist.");
        }
        
        var request = new PutObjectRequest()
        {
            BucketName = bucketName,
            Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
            InputStream = file.OpenReadStream()
        };
        
        request.Metadata.Add("Content-Type", file.ContentType);
        await s3Client.PutObjectAsync(request);
        
        return Ok($"File {prefix}/{file.FileName} uploaded to S3 successfully!");
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllFilesAsync(string bucketName, string? prefix)
    {
        var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        if (!bucketExists)
        {
            return NotFound($"Bucket {bucketName} does not exist.");
        }
        
        var request = new ListObjectsV2Request()
        {
            BucketName = bucketName,
            Prefix = prefix
        };
        
        var result = await s3Client.ListObjectsV2Async(request);
        var s3Objects = result.S3Objects.Select(s =>
        {
            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = s.Key,
                Expires = DateTime.UtcNow.AddMinutes(1)
            };
            return new S3ObjectDto()
            {
                Name = s.Key.ToString(),
                PresignedUrl = s3Client.GetPreSignedURL(urlRequest),
            };
        });
        
        return Ok(s3Objects);
    }
    
    [HttpGet("get-by-key")]
    public async Task<IActionResult> GetFileByKeyAsync(string bucketName, string key)
    {
        var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        
        if (!bucketExists)
        {
            return NotFound($"Bucket {bucketName} does not exist.");
        }
        
        var s3Object = await s3Client.GetObjectAsync(bucketName, key);
        
        return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteFileAsync(string bucketName, string key)
    {
        var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        if (!bucketExists)
        {
            return NotFound($"Bucket {bucketName} does not exist");
        }
        
        await s3Client.DeleteObjectAsync(bucketName, key);
        
        return NoContent();
    }
}