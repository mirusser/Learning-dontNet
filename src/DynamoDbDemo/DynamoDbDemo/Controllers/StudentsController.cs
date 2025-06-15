using Amazon.DynamoDBv2.DataModel;
using DynamoDbDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDbDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController(IDynamoDBContext context) : ControllerBase
{
    [HttpGet("{studentId}")]
    public async Task<IActionResult> GetById(int studentId)
    {
        var student = await context.LoadAsync<Student>(studentId);
        if (student is null)
        {
            return NotFound();
        }
        
        return Ok(student);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var student = await context
            .ScanAsync<Student>(null)
            .GetRemainingAsync();
        
        return Ok(student);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateStudent(Student studentRequest)
    {
        var student = await context.LoadAsync<Student>(studentRequest.Id);
        if (student is not null)
        {
            return BadRequest($"Student with Id {studentRequest.Id} Already Exists");
        }
        await context.SaveAsync(studentRequest);
        
        return Ok(studentRequest);
    }
    
    [HttpDelete("{studentId}")]
    public async Task<IActionResult> DeleteStudent(int studentId)
    {
        var student = await context.LoadAsync<Student>(studentId);
        if (student is null)
        {
            return NotFound();
        }
        
        await context.DeleteAsync(student);
        
        return NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateStudent(Student studentRequest)
    {
        var student = await context.LoadAsync<Student>(studentRequest.Id);
        if (student is null)
        {
            return NotFound();
        }
        await context.SaveAsync(studentRequest);
        
        return Ok(studentRequest);
    }
}