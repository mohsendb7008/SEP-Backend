namespace SEP_Backend.User;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }

    public void Update(User user)
    {
        Name = user.Name;
        Email = user.Email;
        Role = user.Role;
    }
}