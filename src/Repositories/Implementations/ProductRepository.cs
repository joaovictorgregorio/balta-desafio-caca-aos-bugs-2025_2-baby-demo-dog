using BugStore.Data;
using BugStore.Models;
using BugStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Repositories.Implementations;

/// Implementação do repositório de produtos
/// CONCEITO: Mesmo padrão do CustomerRepository - CONSISTÊNCIA!
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) => _context = context;

    public async Task<List<Product>> GetAllProductsAsync() 
        => await _context.Products.AsNoTracking().ToListAsync();
    
    public async Task<Product?> GetProductByIdAsync(Guid id) 
        => await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product> AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = await GetProductByIdAsync(id);
        if (product == null) return false;
        
        _context.Products.Remove(product);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> ExistsProductByNameAsync(string title, Guid? excludeId = null)
    {
        var query = _context.Products.AsNoTracking();
        if (excludeId.HasValue) query = query.Where(p => p.Id != excludeId.Value);
        
        return await query.AnyAsync(p => p.Title == title);
    }
}