namespace PermissionsAttributeTask.DataAccess.Entities;

public class UserProfile
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public string? About { get; set; }
    
}