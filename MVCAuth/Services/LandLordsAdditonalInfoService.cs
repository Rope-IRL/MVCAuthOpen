using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MVCAuth.Services;

public class LandLordsAdditionalInfoService(RealEstateRentalContext db, IMemoryCache cache)
{
    private readonly Int32 _cacheDuration = 300;
    private readonly String _landLordsChacheKey = "LandLordsAdditionalInfo_All";

    public async Task<IEnumerable<LandLordsAdditionalInfo>> GetLandLordsAdditionalInfo()
    {

        IEnumerable<LandLordsAdditionalInfo> landLordsAdditional = null;

            landLordsAdditional = await db.LandLordsAdditionalInfos
                .ToListAsync();

        return landLordsAdditional;
    }
}