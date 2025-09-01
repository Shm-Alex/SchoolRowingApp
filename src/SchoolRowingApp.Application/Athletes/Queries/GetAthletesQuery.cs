using MediatR;
using SchoolRowingApp.Application.Athletes.Dto;

namespace SchoolRowingApp.Application.Athletes.Queries.GetAthletes;

public record GetAthletesQuery : IRequest<List<AthleteDto>>;
