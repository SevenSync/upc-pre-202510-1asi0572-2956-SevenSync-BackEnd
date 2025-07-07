using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;

namespace MaceTech.API.Shared.Domain.Models.Abstractions;

public abstract class AuditEntity : IEntityWithCreatedUpdatedDate
{
    public DateTimeOffset? CreatedDate { get; set; }

    public DateTimeOffset? UpdatedDate { get; set; }
}