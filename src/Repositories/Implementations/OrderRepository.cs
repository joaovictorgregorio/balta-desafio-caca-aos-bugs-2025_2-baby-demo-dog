using BugStore.Data;
using BugStore.Models;
using BugStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Repositories.Implementations;

/// <summary>
/// Implementação do repositório de pedidos
/// CONCEITO: Orders têm relacionamentos, então precisamos usar Include()
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context) => _context = context;
    
    public async Task<Order?> GetOrderByIdAsync(Guid id)
        => await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
    
    
    public async Task<Order?> GetOrderByIdWithDetailsAsync(Guid id)
    {
        // CONCEITO: Include() faz JOIN no banco, trazendo dados relacionados
        // ThenInclude() vai mais fundo, pegando Product de cada OrderLine
        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.Lines)
                .ThenInclude(ol => ol.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
    
    public async Task<Order> AddOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }
}