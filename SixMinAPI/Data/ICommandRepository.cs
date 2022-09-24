using System.Collections.Generic;
using System.Threading.Tasks;
using SixMinAPI.Models;

namespace SixMinAPI.Data
{
    public interface ICommandRepository
    {
        Task SaveChangesAsync();
        Task<Command?> GetCommandByIdAsync(int id);
        Task<IEnumerable<Command>> GetAllCommandsAsync();
        Task CreateCommandAsync(Command command);
        void DeleteCommand(Command command);
    }
}