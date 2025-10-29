using BugStore.Models;
using BugStore.Services.Interfaces;

namespace BugStore.Handlers.Customers;

/// <summary>
/// Handler REFATORADO - Agora ele é SIMPLES e LIMPO!
/// CONCEITO: Handler só faz a "ponte" entre HTTP e Service
/// Não tem regras de negócio, não acessa banco diretamente
/// </summary>
public class Handler
{
    // CONCEITO: Handler depende de Service (não de Repository!)
    // Isso é ARQUITETURA EM CAMADAS
    private readonly ICustomerService _customerService;
    public Handler(ICustomerService customerService) => _customerService = customerService;
    
    /// <summary>
    /// CONCEITO: Método limpo - só converte Request -> Service -> Response
    /// </summary>
    public async Task<Responses.Customers.Create> CreateAsync(Requests.Customers.Create request)
    {
        // Converte Request para Model
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email
        };
        
        // Chama o Service (que tem as regras de negócio)
        var (success, message, created) = await _customerService.CreateCustomerAsync(customer);
        
        // Converte resultado para Response
        return new Responses.Customers.Create
        {
            Success = success,
            Message = message,
            Data = created
        };
    }
    
    public async Task<Responses.Customers.Update> UpdateAsync(Requests.Customers.Update request)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email
        };
        
        var (success, message, updated) = await _customerService.UpdateCustomerAsync(request.Id, customer);
        
        return new Responses.Customers.Update
        {
            Success = success,
            Message = message,
            Data = updated
        };
    }
    
    public async Task<Responses.Customers.Delete> DeleteAsync(Requests.Customers.Delete request)
    {
        var (success, message) = await _customerService.DeleteCustomerAsync(request.Id);
        
        return new Responses.Customers.Delete
        {
            Success = success,
            Message = message
        };
    }
    
    public async Task<Responses.Customers.Get> GetAllAsync()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        
        return new Responses.Customers.Get
        {
            Success = true,
            Message = "Clientes recuperados com sucesso",
            Data = customers
        };
    }
    
    public async Task<Responses.Customers.GetById> GetByIdAsync(Requests.Customers.GetById request)
    {
        var customer = await _customerService.GetCustomerByIdAsync(request.Id);
        
        if (customer == null)
        {
            return new Responses.Customers.GetById
            {
                Success = false,
                Message = "Cliente não encontrado",
                Data = null
            };
        }
        
        return new Responses.Customers.GetById
        {
            Success = true,
            Message = "Cliente encontrado",
            Data = customer
        };
    }
}