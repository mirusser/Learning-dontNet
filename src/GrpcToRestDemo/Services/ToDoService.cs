using Grpc.Core;
using GrpcToRestDemo.Data;
using GrpcToRestDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace GrpcToRestDemo.Services;

public class ToDoService(AppDbContext dbContext) : ToDoIt.ToDoItBase
{
    public override async Task<CreateToDoResponse> CreateToDo(
        CreateToDoRequest request,
        ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Title)
            || string.IsNullOrWhiteSpace(request.Description))
        {
            throw new RpcException(
                    new Status(
                        StatusCode.InvalidArgument,
                        "You must supply a valid object"));
        }

        ToDoItem item = new()
        {
            Title = request.Title,
            Description = request.Description,
        };

        await dbContext.AddAsync(item);
        await dbContext.SaveChangesAsync();

        return new()
        {
            Id = item.Id
        };
    }

    public override async Task<ReadToDoResponse> ReadToDo(
        ReadToDoRequest request,
        ServerCallContext context)
    {
        if (request.Id < 1)
        {
            throw new RpcException(
            new Status(
                StatusCode.InvalidArgument,
                "Id must be greater than 0"));
        }

        var toDoItem = await dbContext.ToDoItems
            .FirstOrDefaultAsync(i => i.Id == request.Id);

        if (toDoItem is null)
        {
            throw new RpcException(
                    new Status(
                        StatusCode.NotFound,
                        "Item not found"));
        }

        return new()
        {
            Id = toDoItem.Id,
            Description = toDoItem.Description,
            Title = toDoItem.Title,
            ToDoStatus = toDoItem.ToDoStatus,
        };
    }

    public override async Task<GetAllResponse> ListToDo(
        GetAllRequest request,
        ServerCallContext context)
    {
        var toDoResponses = await dbContext.ToDoItems
            .Select(i => new ReadToDoResponse()
            {
                Id = i.Id,
                Description = i.Description,
                Title = i.Title,
                ToDoStatus = i.ToDoStatus,
            })
            .ToListAsync();

        if (toDoResponses.Count == 0)
        {
            throw new RpcException(
                    new Status(
                        StatusCode.NotFound,
                        "Items not found"));
        }
        var response = new GetAllResponse();
        response.ToDo.AddRange(toDoResponses);

        return response;
    }

    public override async Task<UpdateToDoResponse> UpdateToDo(
        UpdateToDoRequest request,
        ServerCallContext context)
    {
        var toDoItem = await dbContext.ToDoItems
            .FirstOrDefaultAsync(i => i.Id == request.Id);

        if (toDoItem is null)
        {
            throw new RpcException(
                    new Status(
                        StatusCode.NotFound,
                        "Item not found"));
        }

        toDoItem.Title = request.Title;
        toDoItem.Description = request.Description;
        toDoItem.ToDoStatus = request.ToDoStatus;

        await dbContext.SaveChangesAsync();

        return new()
        {
            Id = request.Id
        };
    }

    public override async Task<DeleteToDoResponse> DeleteToDo(
        DeleteToDoRequest request,
        ServerCallContext context)
    {
        var toDoItem = await dbContext.ToDoItems
            .FirstOrDefaultAsync(i => i.Id == request.Id);

        if (toDoItem is null)
        {
            throw new RpcException(
                    new Status(
                        StatusCode.NotFound,
                        "Item not found"));
        }

        dbContext.Remove(toDoItem);
        await dbContext.SaveChangesAsync();

        return new()
        {
            Id = request.Id
        };
    }
}