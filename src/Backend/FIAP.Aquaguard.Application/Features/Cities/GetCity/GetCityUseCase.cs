using FIAP.Aquaguard.Application.Features.Cities.Shared;
using FIAP.AquaGuard.Domain.Repositories;
using FIAP.AquaGuard.Exception.ExceptionsBase;
using FluentValidation;

namespace FIAP.Aquaguard.Application.Features.Cities.GetCity;

public class GetCityUseCase
{
    private readonly ICityRepository _cityRepository;
    private readonly IValidator<RequestGetCity> _validator;

    public GetCityUseCase(ICityRepository cityRepository, IValidator<RequestGetCity> validator)
    {
        _cityRepository = cityRepository;
        _validator = validator;
    }

    public async Task<ResponseCity> ExecuteAsync(RequestGetCity request)
    {
        Validate(request, _validator);

        var city = await _cityRepository.GetByIdAsync(request.CityId);
        if (city is null)
            throw new ErrorOnValidationException("Cidade nao encontrada.");

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

    public async Task<ResponseCities> ListCitiesAsync(int page = 1, int pageSize = 10)
    {
        var (items, total) = await _cityRepository.GetAllAsync(page, pageSize);

        var cities = items.Select(city => new ResponseCity(
            Id: city.Id,
            Zipcode: city.Zipcode,
            Name: city.Name,
            State: city.State,
            Latitude: city.Latitude,
            Longitude: city.Longitude,
            ResponsibleUserId: city.ResponsibleUserId,
            IsPaidPlan: city.IsPaidPlan)).ToList();

        return new ResponseCities(
            amount: cities.Count,
            total: total,
            page: page,
            totalPages: (int)Math.Ceiling((double)total / pageSize),
            cities: cities);
    }

    private static void Validate(RequestGetCity request, IValidator<RequestGetCity> validator)
    {
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(string.Join(" | ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
