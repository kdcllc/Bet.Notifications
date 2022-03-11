namespace Bet.Notifications.Abstractions.Smtp;

/// <summary>
/// Address for specific Email and Name.
/// <see href="https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects"/>.
/// </summary>
public class Address : ValueObject
{
    public Address(string email, string name = "")
    {
        Email = email;
        Name = name;
    }

    /// <summary>
    /// The name of the person.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The email address.
    /// </summary>
    public string Email { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Email;
        yield return Name;
    }
}
