

namespace CleanCarApi.Application.DTOs
{
    // DTO för inloggning — tar emot användarnamn och lösenord
    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
