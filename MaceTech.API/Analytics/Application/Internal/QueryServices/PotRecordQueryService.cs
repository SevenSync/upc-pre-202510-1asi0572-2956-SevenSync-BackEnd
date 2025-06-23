using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Model.Queries;
using MaceTech.API.Analytics.Domain.Repositories;

namespace MaceTech.API.Analytics.Application.Internal.QueryServices;

public class PotRecordQueryService(IPotRecordRepository potRecordRepository) : IPotRecordQueryService
{
    public async Task<PotRecord?> Handle(GetLatestPotRecordByDeviceIdQuery query)
    {
        return await potRecordRepository.FindLatestByDeviceIdAsync(query.DeviceId);
    }
}