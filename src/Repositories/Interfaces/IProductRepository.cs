using BugStore.Models;

namespace BugStore.Repositories.Interfaces;

/// Interface para operações com Products
/// CONCEITO: Seguindo o mesmo padrão do CustomerRepository
/// Isso se chama CONSISTÊNCIA DE CÓDIGO - facilita manutenção!
public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product> AddProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Guid id);
    Task<bool> ExistsProductByNameAsync(string name, Guid? excludeId = null);
}