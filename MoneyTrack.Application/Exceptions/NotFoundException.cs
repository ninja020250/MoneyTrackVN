namespace MoneyTrack.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, string key) : base($"{name} {key} is not found.")
    {
    }
}