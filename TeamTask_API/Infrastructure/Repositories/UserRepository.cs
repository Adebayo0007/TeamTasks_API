using Microsoft.EntityFrameworkCore;
using TeamTask_API.Data;
using TeamTask_API.Domain.Entities;
using TeamTask_API.Domain.Repositories;

namespace TeamTask_API.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public Task<User?> GetByEmailAsync(string email) =>
        _db.Users.FirstOrDefaultAsync(u => u.Email == email);

    public Task<User?> GetByIdAsync(Guid id) =>
        _db.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task AddAsync(User user) => await _db.Users.AddAsync(user);

    public Task<bool> SaveChangesAsync() => _db.SaveChangesAsync().ContinueWith(t => t.Result > 0);
}
