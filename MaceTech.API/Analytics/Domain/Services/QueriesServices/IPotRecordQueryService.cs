using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Model.Queries;

namespace MaceTech.API.Analytics.Domain.Services.QueriesServices;

public interface IPotRecordQueryService
{
    Task<PotRecord?> Handle(GetLatestPotRecordByDeviceIdQuery query);
}