using BugStore.Models;
using BugStore.Repositories.Interfaces;
using BugStore.Services.Interfaces;

namespace BugStore.Services.Implementations;

public class CustomerService : ICustomerService
{
    // CONCEITO: Service depende de Repository (camada abaixo)
    // Mas usa INTERFACE, não implementação concreta = BAIXO ACOPLAMENTO!
    private readonly ICustomerRepository _customerRepository;
    
    public CustomerService(ICustomerRepository customerRepository) 
        => _customerRepository = customerRepository;
    
    // CONCEITO: Service pode simplesmente repassar para o Repository
    // Ou adicionar lógica extra (filtros, ordenação, etc)
    public async Task<List<Customer>> GetAllCustomersAsync()
        => await _customerRepository.GetAllCustomersAsync();
    
    public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        => await _customerRepository.GetCustomerByIdAsync(id);
    
    
    public async Task<(bool Success, string Message, Customer? Customer)> CreateCustomerAsync(Customer customer)
    {
        // Validação 1: Dados obrigatórios
        if (string.IsNullOrWhiteSpace(customer.Name))
            return (false, "Nome é obrigatório", null);
        
        if (string.IsNullOrWhiteSpace(customer.Email))
            return (false, "Email é obrigatório", null);
        
        // Validação 2: Regra de negócio - email único
        if (await _customerRepository.ExistsCustomerByEmailAsync(customer.Email))
            return (false, "Email já está em uso", null);
        
        // Tudo OK? Cria o cliente!
        var created = await _customerRepository.AddCustomerAsync(customer);
        return (true, "Cliente criado com sucesso", created);
    }
    
    public async Task<(bool Success, string Message, Customer? Customer)> UpdateCustomerAsync(Guid id, Customer customer)
    {
        // Validação 1: Cliente existe?
        var existing = await _customerRepository.GetCustomerByIdAsync(id);
        if (existing == null)
            return (false, "Cliente não encontrado", null);
        
        // Validação 2: Dados obrigatórios
        if (string.IsNullOrWhiteSpace(customer.Name))
            return (false, "Nome é obrigatório", null);
        
        if (string.IsNullOrWhiteSpace(customer.Email))
            return (false, "Email é obrigatório", null);
        
        // Validação 3: Email único (exceto para o próprio cliente)
        if (await _customerRepository.ExistsCustomerByEmailAsync(customer.Email, id))
            return (false, "Email já está em uso por outro cliente", null);
        
        // CONCEITO: Atualiza propriedades do objeto existente
        // Mantém o ID original!
        existing.Name = customer.Name;
        existing.Email = customer.Email;
        
        var updated = await _customerRepository.UpdateCustomerAsync(existing);
        if (!updated)
            return (false, "Erro ao atualizar cliente", null);
        
        return (true, "Cliente atualizado com sucesso", existing);
    }
    
    public async Task<(bool Success, string Message)> DeleteCustomerAsync(Guid id)
    {
        var existing = await _customerRepository.GetCustomerByIdAsync(id);
        if (existing == null)
            return (false, "Cliente não encontrado");
        
        var deleted = await _customerRepository.DeleteCustomerAsync(id);
        if (!deleted)
            return (false, "Erro ao excluir cliente");
        
        return (true, "Cliente excluído com sucesso");
    }
}