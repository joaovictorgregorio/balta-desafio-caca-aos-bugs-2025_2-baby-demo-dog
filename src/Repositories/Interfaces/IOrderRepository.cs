using BugStore.Models;

namespace BugStore.Repositories.Interfaces;

/// Interface para operações com Orders
/// CONCEITO: Orders são mais complexos porque incluem OrderLines
public interface IOrderRepository
{
    Task<Order?> GetOrderByIdAsync(Guid id);
    
    /// CONCEITO: Include significa trazer dados relacionados (OrderLines e Products)
    /// Isso evita múltiplas consultas ao banco
    Task<Order?> GetOrderByIdWithDetailsAsync(Guid id);
    
    Task<Order> AddOrderAsync(Order order);
}