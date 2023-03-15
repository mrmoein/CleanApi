using CleanApi.Application.Common.Models;
using MediatR;

namespace CleanApi.Application.User.Queries.GetCurrent;

public record GetCurrentUserQueryRequest : IRequest<ServiceResult<GetCurrentUserQueryResponse>>;