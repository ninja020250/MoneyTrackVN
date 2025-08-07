namespace MoneyTrack.Application.Contracts.Infrastructure;

public interface IOtpService
{
    public string GenerateOtp(string email);

    public bool VerifyOtp(string email, string code);
}