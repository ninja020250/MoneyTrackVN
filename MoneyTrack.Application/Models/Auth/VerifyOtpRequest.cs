namespace MoneyTrack.Application.Models.Auth;

public class VerifyOtpRequest
{
    public string Otp { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}