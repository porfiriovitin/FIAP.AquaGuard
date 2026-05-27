namespace FIAP.AquaGuard.Domain.Entities;

public class City
{
    public Guid Id { get; private set; }
    public string Zipcode { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public Guid? ResponsibleUserId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public User? ResponsibleUser { get; private set; }

    private readonly List<User> _users = new();
    public IReadOnlyCollection<User> Users => _users;

    private readonly List<RiskAnalysis> _riskAnalyses = new();
    public IReadOnlyCollection<RiskAnalysis> RiskAnalyses => _riskAnalyses;

    private readonly List<Sensor> _sensors = new();
    public IReadOnlyCollection<Sensor> Sensors => _sensors;

    private City() { }

    public City(string zipcode, string name, string state, decimal latitude, decimal longitude)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("City name is required.");

        if (state.Length != 2)
            throw new ArgumentException("State must have 2 characters.");

        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Invalid latitude.");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Invalid longitude.");

        Zipcode = zipcode;
        Name = name.Trim();
        State = state.ToUpper();
        Latitude = latitude;
        Longitude = longitude;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void AssignResponsibleUser(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        ResponsibleUserId = user.Id;
        ResponsibleUser = user;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateLocation(decimal latitude, decimal longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Invalid latitude.");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Invalid longitude.");

        Latitude = latitude;
        Longitude = longitude;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
