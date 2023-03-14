using System.ComponentModel.DataAnnotations;
using CleanApi.Application.Common.Models;
using MediatR;

namespace CleanApi.Application.User.Queries.GetCurrent;

public record GetCurrentCommandRequest: IRequest<ServiceResult<GetCurrentCommandResponse>>;