using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Companies;

public sealed class Company : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public string? Industry { get; private set; }
    public string? Description { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? Website { get; private set; }
    public string? Country { get; private set; }

    private Company() { }

    public static Company Create(string name, string? industry = null, string? country = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw DomainException.Create("Company.NameEmpty", "Company name cannot be empty.");

        return new Company
        {
            Name = name.Trim(),
            Industry = industry?.Trim(),
            Country = country?.Trim()
        };
    }

    public void Update(
        string? industry,
        string? description,
        string? logoUrl,
        string? website,
        string? country)
    {
        Industry = industry?.Trim();
        Description = description?.Trim();
        LogoUrl = logoUrl?.Trim();
        Website = website?.Trim();
        Country = country?.Trim();
        Touch();
    }
}
