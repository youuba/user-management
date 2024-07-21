using System.Diagnostics.CodeAnalysis;

namespace UMS.Application.Common.Models;

[ExcludeFromCodeCoverage]
public class UserDto
{
    public required int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;
}
