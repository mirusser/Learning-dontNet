using MinimalAPI_Les.Models;

namespace MinimalAPI_Les.Data;

public interface ICommandRepo
{
    Task SaveChangesAsync();
    Task<Command?> GetCommandByIdAsync(int id);
    Task<IEnumerable<Command>> GetAllCommandsAsync();
    Task CreateCommandAsync(Command command);

    void DeleteCommand(Command command);
}