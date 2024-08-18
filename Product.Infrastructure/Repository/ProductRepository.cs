using Microsoft.EntityFrameworkCore;
using ProductInfrastructure;
using Product.Domain.Interfaces.IRepository;

namespace Product.Infrastructure.Repository
{
    public class ProductRepository<T>: IProductRepository<T> where T : class
    {
        private readonly ProductManagementDbContext _context;
        private readonly DbSet<T> _dbSet;

        public ProductRepository(ProductManagementDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException("Entity not found");
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
