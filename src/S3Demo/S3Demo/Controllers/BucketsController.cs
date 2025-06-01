using Amazon.S3;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Mvc;

namespace S3Demo.Controllers;

[Route("api/buckets")]
[ApiController]
public class BucketsController(IAmazonS3 s3Client) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateBucketAsync(string bucketName)
    {
        var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        
        if (bucketExists)
        {
            return BadRequest($"Bucket {bucketName} already exists.");
        }
        
        await s3Client.PutBucketAsync(bucketName);
        
        return Ok($"Bucket {bucketName} created.");
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllBucketAsync()
    {
        var data = await s3Client.ListBucketsAsync();
        var buckets = data.Buckets.Select(b => b.BucketName);
        
        return Ok(buckets);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteBucketAsync(string bucketName)
    {
        await s3Client.DeleteBucketAsync(bucketName);
        
        return NoContent();
    }
}