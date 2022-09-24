using System.Collections.Generic;
using System.Threading.Tasks;
using SixMinAPI.Models;

namespace SixMinAPI.Data
{
    public interface ICommandRepository
    {
        Task SaveChanges();
        Task<Command?> GetCommandById(int id);
        Task<IEnumerable<Command>> GetAllCommands();
        Task CreateCommand(Command command);
        void DeleteCommand(Command command);
    }
}