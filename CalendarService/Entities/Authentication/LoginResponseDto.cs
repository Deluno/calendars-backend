using Newtonsoft.Json;

namespace CalendarService.Entities.Authentication;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;

    [JsonIgnore]
    public string? RefreshToken { get; set; }

    public DateTime Expires { get; set; }
}