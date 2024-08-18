using Product.Domain.Models;
using Product.Domain.Interfaces.IRepository;
using Product.Domain.Interfaces.IServices;
using Microsoft.Extensions.Caching.Memory;

namespace Product.Domain.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository<Products> _productRepository;
        private readonly IMemoryCache _cache;
        public ProductService(IProductRepository<Products> productRepository, IMemoryCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<Products>> GetAllProductsAsync()
        {
            if (!_cache.TryGetValue("products", out List<Products> products))
            {
                // If not cached, retrieve data from repository
                products = (await _productRepository.GetAllAsync()).ToList();

                // Set cache options
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                // Cache the data
                _cache.Set("products", products, cacheOptions);
            }
            return products;
        }

        public async Task<Products> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Products> CreateProductAsync(Products product)
        {
            return await _productRepository.AddAsync(product);
        }

        public async Task<Products> UpdateProductAsync(Products product)
        {
            return await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}
