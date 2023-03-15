using System.ComponentModel.DataAnnotations;

namespace CleanApi.Domain.Entities;

public class UserInfo : BaseAuditableEntity
{
    [Key] [Required] [StringLength(36)] public string Id { get; set; } = string.Empty;

    [StringLength(256)] public string UserName { get; set; } = string.Empty;

    [StringLength(256)] public string FirstName { get; set; } = string.Empty;

    [StringLength(256)] public string LastName { get; set; } = string.Empty;

    public string FullName => FirstName + " " + LastName;
}