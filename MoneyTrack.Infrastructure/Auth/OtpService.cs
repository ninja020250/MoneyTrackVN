using System.Collections.Concurrent;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Models.Auth;

namespace MoneyTrack.Infrastructure.Auth;

public class OtpService : IOtpService
{
    private readonly ConcurrentDictionary<string, Otp> _otpStore = new();

    public string GenerateOtp(string email)
    {
        var otp = new Otp
        {
            Email = email.ToLower(),
            Code = new Random().Next(100000, 999999).ToString(),
            ExpiryTime = DateTime.UtcNow.AddMinutes(30) // OTP valid for 5 minutes
        };

        _otpStore[email.ToLower()] = otp;
        return otp.Code;
    }

    public bool VerifyOtp(string email, string code)
    {
        _otpStore.TryGetValue(email, out var otp);
        if (otp?.ExpiryTime < DateTime.UtcNow)
        {
            _otpStore.TryRemove(email, out _);
            return false;
        }

        if (otp?.Code == code)
        {
            _otpStore.TryRemove(email, out _);
            return true;
        }

        return false;
    }
}