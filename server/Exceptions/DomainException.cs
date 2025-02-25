namespace Kermit.Exceptions;

public class DomainException : Exception
{
    public DomainException(string error) : base(error)
    {
    }
}
