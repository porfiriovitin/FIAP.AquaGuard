using FIAP.AquaGuard.Domain.Enums;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Sensors.DeleteSensor;

public class DeleteSensorUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestDeleteSensor> _validator;

    public DeleteSensorUseCase(ISensorRepository sensorRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<RequestDeleteSensor> validator)
    {
        _sensorRepository = sensorRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task ExecuteAsync(RequestDeleteSensor request)
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
                throw new ErrorOnValidationException("Manager so pode remover sensores da propria cidade.");
        }

        await _sensorRepository.DeleteAsync(sensor);
        await _unitOfWork.CommitAsync();
    }

    private static void Validate(RequestDeleteSensor request, IValidator<RequestDeleteSensor> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
