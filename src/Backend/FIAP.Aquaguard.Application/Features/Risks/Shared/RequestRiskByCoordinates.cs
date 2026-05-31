namespace FIAP.Aquaguard.Application.Features.Risks.Shared;

public record RequestRiskCoordinatesInput(double Latitude, double Longitude);

public record RequestRiskByCoordinates(RequestRiskCoordinatesInput Coordinates);
