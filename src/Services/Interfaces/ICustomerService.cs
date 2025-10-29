using BugStore.Models;

namespace BugStore.Services.Interfaces;

/// <summary>
/// Interface para serviço de clientes
/// CONCEITO: Services contêm REGRAS DE NEGÓCIO
/// Repositories só fazem operações de banco - Services decidem QUANDO e COMO fazer
/// </summary>
public interface ICustomerService
{
    Task<List<Customer>> GetAllCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(Guid id);
    
    /// <summary>
    /// CONCEITO: Retorna tupla (sucesso, mensagem, objeto)
    /// Isso permite comunicar erros sem usar exceções
    /// </summary>
    Task<(bool Success, string Message, Customer? Customer)> CreateCustomerAsync(Customer customer);
    Task<(bool Success, string Message, Customer? Customer)> UpdateCustomerAsync(Guid id, Customer customer);
    Task<(bool Success, string Message)> DeleteCustomerAsync(Guid id);
}