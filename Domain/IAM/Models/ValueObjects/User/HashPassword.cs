using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.IAM.Models.ValueObjects.User;

[ComplexType]
public record HashPassword(string Value);