using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;

namespace MaceTech.API.Shared.Domain.Models.Abstractions;

public abstract class AuditEntity : IEntityWithCreatedUpdatedDate
{
    [Column("CreatedAt")]
    public DateTimeOffset? CreatedDate { get; set; }

    [Column("UpdatedAt")]
    public DateTimeOffset? UpdatedDate { get; set; }
}