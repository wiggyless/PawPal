namespace PawPal.Application.Common.Exceptions;

public sealed class PawPalConflictException : Exception
{
    public PawPalConflictException(string message) : base(message) { }
}
