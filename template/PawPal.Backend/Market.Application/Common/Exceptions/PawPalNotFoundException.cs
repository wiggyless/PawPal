namespace PawPal.Application.Common.Exceptions;

public sealed class PawPalNotFoundException : Exception
{
    public PawPalNotFoundException(string message) : base(message) { }
}
