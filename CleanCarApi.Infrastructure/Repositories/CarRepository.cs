using CleanCarApi.Domain.Entities;
using CleanCarApi.Domain.Interfaces;
using CleanCarApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanCarApi.Infrastructure.Repositories
{

// Utökar den generiska repot med Car-specifika queries
    public class CarRepository : Repository<Car>, IRepository<Car>
    {
        public CarRepository(AppDbContext context) : base(context) { }

        // Override GetAllAsync för att inkludera Brand-data i varje Car (eager loading)
        public new async Task<IEnumerable<Car>> GetAllAsync()
            => await _dbSet
                .Include(c => c.Brand)
                .ToListAsync();

        // Override GetByIdAsync för att inkludera Brand även vid hämtning av enskild bil
        public new async Task<Car?> GetByIdAsync(int id)
            => await _dbSet
                .Include(c => c.Brand)
                .FirstOrDefaultAsync(c => c.Id == id);
    }

}