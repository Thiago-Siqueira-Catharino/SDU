using Microsoft.AspNetCore.Identity;

namespace SDU.Contexts.Auth.Domain.Entities;

public class User : IdentityUser
{
    public String Name { get; set; }
}