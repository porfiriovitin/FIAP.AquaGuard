using FIAP.Aquaguard.Application.Features.Sensors.Shared;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Sensors.UpdateSensor;

public class UpdateSensorUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestUpdateSensor> _validator;

    public UpdateSensorUseCase( ISensorRepository sensorRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<RequestUpdateSensor> validator)
    {
        _sensorRepository = sensorRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ResponseSensor> ExecuteAsync(RequestUpdateSensor request)
    {
        Validate(request, _validator);

        var sensor = await _sensorRepository.GetByIdAsync(request.SensorId);
        if (sensor is null)
            throw new ErrorOnValidationException("Sensor nao encontrado.");

        if (request.RequestedByRole == UserRole.Manager)
        {
            var manager = await _userRepository.GetByIdAsync(request.RequestedByUserId);
            if (manager is null)
                throw new ErrorOnValidationException("Usuario solicitante nao encontrado.");

            if (manager.CityId is null || manager.CityId != sensor.CityId)
                throw new ErrorOnValidationException("Manager so pode alterar sensores da propria cidade.");
        }

        sensor.UpdateLocation(request.Latitude, request.Longitude, request.PlaceName.Trim());
        await _sensorRepository.UpdateAsync(sensor);
        await _unitOfWork.CommitAsync();

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

    private static void Validate(RequestUpdateSensor request, IValidator<RequestUpdateSensor> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
