
namespace FIAP.AquaGuard.Domain.Entities;

public class City
{
    public Guid Id { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public Guid? ResponsibleUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public User? ResponsibleUser { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
    public ICollection<RiskAnalysis> RiskAnalyses { get; set; } = new List<RiskAnalysis>();
}
