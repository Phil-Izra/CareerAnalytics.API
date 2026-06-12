namespace CareerAnalytics.Domain.Common;

public sealed class DomainException : Exception
{
    public string Code { get; }

    public DomainException(string code, string message) : base(message)
    {
        Code = code;
    }

    public static DomainException Create(string code, string message) => new(code, message);
}
