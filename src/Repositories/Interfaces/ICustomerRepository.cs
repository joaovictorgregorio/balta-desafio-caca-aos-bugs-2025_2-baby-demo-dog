using BugStore.Models;

namespace BugStore.Repositories.Interfaces;

/// <summary>
/// Interface que define o CONTRATO para operações com Customer no banco de dados
/// CONCEITO: Trabalhar com interfaces permite que qualquer implementação possa ser usada,
/// desde que siga este contrato. Isso é BAIXO ACOPLAMENTO!
/// </summary>
public interface ICustomerRepository
{
    /// Busca todos os clientes do banco
    /// CONCEITO: Métodos assíncronos (Task) permitem que a aplicação não trave enquanto aguarda o banco de dados responder
    Task<List<Customer>> GetAllCustomersAsync();
    
    /// Busca um cliente específico por ID
    /// CONCEITO: Task<Customer?> significa que pode retornar um Customer ou null
    Task<Customer?> GetCustomerByIdAsync(Guid id);
    
    /// Adiciona um novo cliente ao banco
    /// CONCEITO: Recebe o objeto, salva e retorna o mesmo objeto (agora com ID gerado)
    Task<Customer> AddCustomerAsync(Customer customer);
    
    /// Atualiza um cliente existente
    /// CONCEITO: Retorna bool indicando sucesso (true) ou falha (false)
    Task<bool> UpdateCustomerAsync(Customer customer);
    
    /// Remove um cliente do banco
    Task<bool> DeleteCustomerAsync(Guid id);
    
    /// Verifica se existe um cliente com determinado email
    /// CONCEITO: Útil para validações antes de criar/atualizar
    Task<bool> ExistsCustomerByEmailAsync(string email, Guid? excludeId = null);
}