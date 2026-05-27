using FIAP.AquaGuard.Domain.Enums;

namespace FIAP.AquaGuard.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; } = UserRole.Resident;
    public Guid? CityId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public City? City { get; private set; }

    private readonly List<City> _responsibleCities = new();
    public IReadOnlyCollection<City> ResponsibleCities => _responsibleCities;

    private User() { }

    public User(string name, string email, string passwordHash, UserRole role = UserRole.Resident, Guid? cityId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("User name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("User email is required.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.");

        Id = Guid.NewGuid();
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = role;
        CityId = cityId;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateProfile(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("User name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("User email is required.");

        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdatePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.");

        PasswordHash = passwordHash;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ChangeRole(UserRole role)
    {
        Role = role;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void AssignCity(City city)
    {
        if (city is null)
            throw new ArgumentNullException(nameof(city));

        CityId = city.Id;
        City = city;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
