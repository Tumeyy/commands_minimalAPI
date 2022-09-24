using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using SixMinAPI.Models;
using System;

namespace SixMinAPI.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _dbContext;
        public CommandRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateCommandAsync(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            await _dbContext.AddAsync(command);
        }

        public void DeleteCommand(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            _dbContext.Commands.Remove(command);
        }

        public async Task<IEnumerable<Command>> GetAllCommandsAsync()
        {
            return await _dbContext.Commands!.ToListAsync();
        }

        public async Task<Command?> GetCommandByIdAsync(int id)
        {
            return await _dbContext.Commands.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}