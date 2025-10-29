using BugStore.Models;

namespace BugStore.Responses.Customers;

public class Update
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Customer? Data { get; set; }
}