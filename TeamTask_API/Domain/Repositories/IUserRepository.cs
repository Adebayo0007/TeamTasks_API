using TeamTask_API.Domain.Entities;
namespace TeamTask_API.Domain.Repositories;
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task<bool> SaveChangesAsync();
}