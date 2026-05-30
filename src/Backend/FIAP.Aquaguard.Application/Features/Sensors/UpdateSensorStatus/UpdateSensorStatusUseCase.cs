using FIAP.Aquaguard.Application.Features.Sensors.Shared;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Sensors.UpdateSensorStatus;

public class UpdateSensorStatusUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestUpdateSensorStatus> _validator;

    public UpdateSensorStatusUseCase(ISensorRepository sensorRepository, IUnitOfWork unitOfWork, IValidator<RequestUpdateSensorStatus> validator)
    {
        _sensorRepository = sensorRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ResponseSensor> ExecuteAsync(RequestUpdateSensorStatus request)
    {
        Validate(request, _validator);

        var sensor = await _sensorRepository.GetByIdAsync(request.SensorId);
        if (sensor is null)
            throw new ErrorOnValidationException("Sensor nao encontrado.");

        await _sensorRepository.ChangeSensorStatus(sensor, request.Status);
        await _unitOfWork.CommitAsync();

        sensor.ChangeStatus(request.Status);

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

    private static void Validate(RequestUpdateSensorStatus request, IValidator<RequestUpdateSensorStatus> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
