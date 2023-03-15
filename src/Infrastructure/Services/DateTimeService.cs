using CleanApi.Application.Common.Interfaces;

namespace CleanApi.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}