namespace MaceTech.API.ARM.Domain.Model.Enums;

public enum PotStatus
{
    Healthy = 0,
    Warning = 1,
    Critical = 2,
    Deleted = 3,    //  Also known as "Destroyed"
}