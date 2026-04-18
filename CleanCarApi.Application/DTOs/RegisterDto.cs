
namespace CleanCarApi.Application.DTOs
{
    // DTO för registrering av ny användare
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Rollen användaren ska tilldelas — "Admin" eller "User"
        public string Role { get; set; } = "User";
    }
}
