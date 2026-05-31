using FIAP.Aquaguard.Application.Features.Cities.Shared;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Cities.UpdateCityPaidPlan;

public class UpdateCityPaidPlanUseCase
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RequestUpdateCityPaidPlan> _validator;

    public UpdateCityPaidPlanUseCase(ICityRepository cityRepository, IUnitOfWork unitOfWork, IValidator<RequestUpdateCityPaidPlan> validator)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ResponseCity> ExecuteAsync(RequestUpdateCityPaidPlan request)
    {
        Validate(request, _validator);

        var city = await _cityRepository.GetByIdAsync(request.CityId);
        if (city is null)
            throw new ErrorOnValidationException(ResourceMessagesException.CITY_NOT_FOUND);

        if (request.IsPaidPlan == 1)
            city.BecomePaidPlan();
        else
            city.BecomeFreePlan();

        await _cityRepository.UpdateAsync(city);
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

    private static void Validate(RequestUpdateCityPaidPlan request, IValidator<RequestUpdateCityPaidPlan> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
