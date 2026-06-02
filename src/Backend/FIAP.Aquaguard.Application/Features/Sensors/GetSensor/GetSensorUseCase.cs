using FIAP.Aquaguard.Application.Features.Sensors.Shared;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Sensors.GetSensor;

public class GetSensorUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<RequestGetSensor> _validator;

    public GetSensorUseCase(ISensorRepository sensorRepository, IUserRepository userRepository, IValidator<RequestGetSensor> validator)
    {
        _sensorRepository = sensorRepository;
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<ResponseSensor> ExecuteAsync(RequestGetSensor request)
    {
        Validate(request, _validator);

        var sensor = await _sensorRepository.GetByIdAsync(request.SensorId);
        if (sensor is null)
            throw new ErrorOnValidationException("Sensor nao encontrado.");

        await ValidateScope(request.RequestedByUserId, sensor.CityId);

        return new ResponseSensor(
            Id: sensor.Id,
            CityId: sensor.CityId,
            PlaceName: sensor.PlaceName,
            Latitude: sensor.Latitude,
            Longitude: sensor.Longitude,
            SensorType: sensor.SensorType,
            Status: sensor.Status,
            InstalledAt: sensor.InstalledAt);
    }

    public async Task<ResponseSensors> ListAsync(Guid cityId, Guid requestedByUserId, UserRole requestedByRole, int page = 1, int pageSize = 10)
    {
        if (cityId == Guid.Empty)
            throw new ErrorOnValidationException("Id da cidade e obrigatorio.");

        await ValidateScope(requestedByUserId, cityId);

        var (items, total) = await _sensorRepository.GetAllAsync(page, pageSize, cityId);

        var sensors = items.Select(sensor => new ResponseSensor(
            Id: sensor.Id,
            CityId: sensor.CityId,
            PlaceName: sensor.PlaceName,
            Latitude: sensor.Latitude,
            Longitude: sensor.Longitude,
            SensorType: sensor.SensorType,
            Status: sensor.Status,
            InstalledAt: sensor.InstalledAt)).ToList();

        return new ResponseSensors(
            amount: sensors.Count,
            total: total,
            page: page,
            totalPages: (int)Math.Ceiling((double)total / pageSize),
            sensors: sensors);
    }

    private async Task ValidateScope(Guid requestedByUserId, Guid resourceCityId)
    {
        var cityEmployee = await _userRepository.GetByIdAsync(requestedByUserId);
        if (cityEmployee is null)
            throw new ErrorOnValidationException("Usuario solicitante nao encontrado.");

        if (cityEmployee.CityId is null || cityEmployee.CityId != resourceCityId)
            throw new ErrorOnValidationException("Manager so pode acessar sensores da propria cidade.");
    }

    private static void Validate(RequestGetSensor request, IValidator<RequestGetSensor> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
