using FIAP.Aquaguard.Application.Features.Cities.Shared;
using FIAP.AquaGuard.Domain.Entities;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Cities.CreateCity;

public class CreateCityUseCase
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestCreateCity> _validator;

    public CreateCityUseCase(ICityRepository cityRepository, IUnitOfWork unitOfWork, IValidator<RequestCreateCity> validator)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ResponseCity> ExecuteAsync(RequestCreateCity request)
    {
        Validate(request, _validator);

        var cityByName = await _cityRepository.GetByNameAsync(request.Name.Trim());
        if (cityByName is not null)
            throw new ErrorOnValidationException(ResourceMessagesException.CITY_NAME_ALREADY_EXISTS);

        var cityByZipcode = await _cityRepository.GetByZipCode(request.Zipcode.Trim());
        if (cityByZipcode is not null)
            throw new ErrorOnValidationException(ResourceMessagesException.CITY_ZIPCODE_ALREADY_REGISTERED);

        City city = new(
            zipcode: request.Zipcode.Trim(),
            name: request.Name.Trim(),
            state: request.State.Trim(),
            latitude: request.Latitude,
            longitude: request.Longitude);

        await _cityRepository.AddAsync(city);
        await _unitOfWork.CommitAsync();

        return new ResponseCity(
            Id: city.Id,
            Zipcode: city.Zipcode,
            Name: city.Name,
            State: city.State,
            Latitude: city.Latitude,
            Longitude: city.Longitude,
            ResponsibleUserId: city.ResponsibleUserId,
            IsPaidPlan: city.IsPaidPlan);
    }

    private static void Validate(RequestCreateCity request, IValidator<RequestCreateCity> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
