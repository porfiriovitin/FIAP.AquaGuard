namespace FIAP.Aquaguard.Application.Features.Cities.UpdateCity;

public record RequestUpdateCity(Guid CityId, decimal Latitude, decimal Longitude);
