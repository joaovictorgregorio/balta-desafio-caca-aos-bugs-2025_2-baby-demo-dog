using BugStore.Data;
using BugStore.Repositories.Interfaces;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Repositories.Implementations;

/// Implementação CONCRETA do ICustomerRepository
/// CONCEITO: Esta classe faz o trabalho "sujo" de conversar com o banco de dados
/// Se precisarmos trocar o banco (SQL Server para PostgreSQL), só mudamos aqui!
public class CustomerRepository : ICustomerRepository
{
    // CONCEITO: Campo privado readonly - só pode ser definido no construtor
    // Isso garante que não será modificado acidentalmente
    private readonly AppDbContext _context;
    
    /// CONCEITO: INJEÇÃO DE DEPENDÊNCIA via construtor
    /// O ASP.NET Core automaticamente fornece o AppDbContext aqui
    public CustomerRepository(AppDbContext context) => _context = context;

    public async Task<List<Customer>> GetAllCustomersAsync() 
        => await _context.Customers.AsNoTracking().ToListAsync();
    
    // CONCEITO: FirstOrDefaultAsync retorna o primeiro ou null
    // É mais seguro que First() que lança exceção se não encontrar
    public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        => await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        // CONCEITO: Add marca a entidade para inserção
        await _context.Customers.AddAsync(customer);
        
        // CONCEITO: SaveChangesAsync efetiva a operação no banco, antes disso, é só memória!
        await _context.SaveChangesAsync();
        
        return customer;
    }

    public async Task<bool> UpdateCustomerAsync(Customer customer)
    {
        // CONCEITO: Update marca a entidade como modificada
        _context.Customers.Update(customer);

        // CONCEITO: SaveChangesAsync retorna número de linhas afetadas, se > 0, deu certo!
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteCustomerAsync(Guid id)
    {
        var customer = await GetCustomerByIdAsync(id);

        if (customer == null) return false;

        _context.Customers.Remove(customer);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> ExistsCustomerByEmailAsync(string email, Guid? excludeId = null)
    {
        // CONCEITO: Query flexível - exclui o próprio ID ao atualizar
        var query = _context.Customers.AsNoTracking();
        
        if (excludeId.HasValue) query = query.Where(c => c.Id != excludeId.Value);
        
        return await query.AnyAsync(c => c.Email == email);
    }
}