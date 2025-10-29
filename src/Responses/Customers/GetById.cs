namespace BugStore.Responses.Customers;

public class GetById
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}