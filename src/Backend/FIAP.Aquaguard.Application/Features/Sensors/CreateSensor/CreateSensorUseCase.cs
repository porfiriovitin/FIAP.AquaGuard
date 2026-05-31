using FIAP.Aquaguard.Application.Features.Sensors.Shared;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Sensors.CreateSensor;

public class CreateSensorUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestCreateSensor> _validator;

    public CreateSensorUseCase(ISensorRepository sensorRepository, IUserRepository userRepository, ICityRepository cityRepository, IUnitOfWork unitOfWork, IValidator<RequestCreateSensor> validator)
    {
        _sensorRepository = sensorRepository;
        _userRepository = userRepository;
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ResponseSensor> ExecuteAsync(RequestCreateSensor request)
    {
        Validate(request, _validator);

        var city = await _cityRepository.GetByIdAsync(request.CityId);
        if (city is null)
            throw new ErrorOnValidationException(ResourceMessagesException.CITY_NOT_FOUND);

        if (request.RequestedByRole == UserRole.Manager)
        {
            var manager = await _userRepository.GetByIdAsync(request.RequestedByUserId);
            if (manager is null)
                throw new ErrorOnValidationException(ResourceMessagesException.REQUEST_USER_NOT_FOUND);

            if (manager.CityId is null || manager.CityId != request.CityId)
                throw new ErrorOnValidationException(ResourceMessagesException.INVALID_SENSOR_REGISTER);
        }

        var sensor = new Sensor(
            cityId: request.CityId,
            placeName: request.PlaceName.Trim(),
            latitude: request.Latitude,
            longitude: request.Longitude,
            sensorType: request.SensorType,
            installedAt: request.InstalledAt);

        await _sensorRepository.AddAsync(sensor);
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

    private static void Validate(RequestCreateSensor request, IValidator<RequestCreateSensor> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
