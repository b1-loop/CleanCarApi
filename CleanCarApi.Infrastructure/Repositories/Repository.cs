using CleanCarApi.Domain.Interfaces;
using CleanCarApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanCarApi.Infrastructure.Repositories
{
    // Generisk implementation av IRepository — fungerar för alla entiteter
    public class Repository<T> : IRepository<T> where T : class
    {
        // DbContext används för att kommunicera med databasen
        protected readonly AppDbContext _context;

        // DbSet är tabellen vi jobbar mot
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            // Hämtar rätt tabell baserat på typen T
            _dbSet = context.Set<T>();
        }

        // Hämtar alla rader från tabellen
        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        // Hämtar en rad baserat på id, returnerar null om den inte finns
        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        // Lägger till en ny rad i tabellen
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Markerar entiteten som modifierad så EF Core vet att den ska uppdateras
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Tar bort en rad från tabellen
        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}