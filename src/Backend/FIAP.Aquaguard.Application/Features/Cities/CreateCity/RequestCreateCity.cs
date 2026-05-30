namespace FIAP.Aquaguard.Application.Features.Cities.CreateCity;

public record RequestCreateCity(
    string Zipcode,
    string Name,
    string State,
    decimal Latitude,
    decimal Longitude);
