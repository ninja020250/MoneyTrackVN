namespace MoneyTrack.Application.Models.Auth;

public class Otp
{
    public string Email { get; set; }
    public string Code { get; set; }
    public DateTime ExpiryTime { get; set; }
}