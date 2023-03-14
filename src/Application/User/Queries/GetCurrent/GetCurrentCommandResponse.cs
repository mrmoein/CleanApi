using CleanApi.Application.Common.Mappings;
using CleanApi.Domain.Entities;

namespace CleanApi.Application.User.Queries.GetCurrent;

public class GetCurrentCommandResponse : IMapFrom<UserInfo>
{
    public string Id { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;
    
    public IList<string> Roles { get; set; } = new List<string>();
}