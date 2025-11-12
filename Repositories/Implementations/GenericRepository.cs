using Microsoft.EntityFrameworkCore;
using MiniEcom.Data;
using MiniEcom.Repositories.Interfaces;

namespace MiniEcom.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _db;
        public GenericRepository(AppDbContext db) { _db = db; }
        public async Task AddAsync(T entity) { _db.Set<T>().Add(entity); await _db.SaveChangesAsync(); }
        public async Task DeleteAsync(T entity) { _db.Set<T>().Remove(entity); await _db.SaveChangesAsync(); }
        public async Task<T?> GetByIdAsync(int id) => await _db.Set<T>().FindAsync(id);
        public async Task<IEnumerable<T>> ListAsync() => await _db.Set<T>().ToListAsync();
        public async Task UpdateAsync(T entity) { _db.Set<T>().Update(entity); await _db.SaveChangesAsync(); }
    }
}
