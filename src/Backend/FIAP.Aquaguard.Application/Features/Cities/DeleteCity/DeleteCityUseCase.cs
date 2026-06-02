using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Cities.DeleteCity;

public class DeleteCityUseCase
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestDeleteCity> _validator;

    public DeleteCityUseCase(ICityRepository cityRepository, IUnitOfWork unitOfWork, IValidator<RequestDeleteCity> validator)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task ExecuteAsync(RequestDeleteCity request)
    {
        Validate(request, _validator);

        var city = await _cityRepository.GetByIdAsync(request.CityId);
        if (city is null)
            throw new ErrorOnValidationException(ResourceMessagesException.CITY_NOT_FOUND);

        await _cityRepository.DeleteAsync(city);
        await _unitOfWork.CommitAsync();
    }

    private static void Validate(RequestDeleteCity request, IValidator<RequestDeleteCity> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
