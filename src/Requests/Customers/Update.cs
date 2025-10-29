namespace BugStore.Requests.Customers;

public class Update
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}