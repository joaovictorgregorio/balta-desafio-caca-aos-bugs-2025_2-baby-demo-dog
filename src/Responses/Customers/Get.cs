using BugStore.Models;

namespace BugStore.Responses.Customers;

public class Get
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public List<Customer> Data { get; set; }
}