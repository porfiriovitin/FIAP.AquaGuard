namespace FIAP.Aquaguard.Application.Features.Cities.Shared;

public record ResponseCity(
    Guid Id,
    string Zipcode,
    string Name,
    string State,
    decimal Latitude,
    decimal Longitude,
    Guid? ResponsibleUserId,
    short IsPaidPlan);

public record ResponseCities(
    long amount,
    long total,
    long page,
    long totalPages,
    List<ResponseCity> cities);
