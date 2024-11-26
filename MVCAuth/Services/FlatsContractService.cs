
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MVCAuth.Models;

namespace MVCAuth.Services;

public class FlatsContractService(RealEstateRentalContext db, IMemoryCache cache)
{
    private readonly Int32 _cacheDuration = 300;
    private readonly String _flatsContractCacheKey = "FlatsContract";

    public async Task<IEnumerable<FlatsContract>> GetFlatsContractsAsync(int pagenumber, int pagesize)
    {
        int maxPages = (int)db.FlatsContracts.Count()%pagesize == 0 ? (int)db.FlatsContracts.Count()/pagesize : (int)(db.FlatsContracts.Count()/pagesize)+1;
        if (pagenumber < 1 || pagenumber > maxPages)
        {
            pagenumber = 1;
        }
        IEnumerable<FlatsContract> flatsContracts = null;
        if (!cache.TryGetValue(_flatsContractCacheKey + $"_{pagenumber}", out flatsContracts))
        {
            flatsContracts = await db.FlatsContracts.Skip(pagesize*(pagenumber-1)).Take(pagesize).ToListAsync();
            if (flatsContracts != null)
            {
                cache.Set(_flatsContractCacheKey + $"_{pagenumber}", flatsContracts, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheDuration)));
            }

        }
        return flatsContracts;
    }

    public async Task<IEnumerable<FlatsContract>> GetFlatsContractFullInfosAsync(int pagenumber, int pagesize)
    {
        int maxPages = (int)db.FlatsContracts
        .Count()%pagesize == 0 ? (int)db.FlatsContracts
        .Count()/pagesize : (int)(db.FlatsContracts
        .Count()/pagesize)+1;

        if (pagenumber < 1 || pagenumber > maxPages)
        {
            pagenumber = 1;
        }
        IEnumerable<FlatsContract> flatsContracts = null;
        flatsContracts = await db.FlatsContracts
            .Include(c => c.FidNavigation)
            .Include(c => c.LidNavigation)
            .ThenInclude(l => l.LesseesAdditionalInfo)
            .Include(c => c.Ll)
            .ThenInclude(ll => ll.LandLordsAdditionalInfo)
            .Skip(pagesize * (pagenumber - 1))
            .Take(pagesize)
            .OrderBy(c => c.Id)
            .ToListAsync();
            
        return flatsContracts;
    }


    public async Task<FlatsContract> GetFlatsContract(Int32 id)
    {
        FlatsContract flatsContract = null;
        if (!cache.TryGetValue("FlatsContract" + id, out flatsContract))
        {
            flatsContract = await db.FlatsContracts.FirstOrDefaultAsync(l => l.Id == id);
            if (flatsContract != null)
            {
                cache.Set("FlatsContract" + flatsContract.Id, flatsContract,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheDuration)));
            }
        }

        return flatsContract;
    }

    public async Task AddFlatsContract(FlatsContract flatsContract)
    {
        db.FlatsContracts.Add(flatsContract);
        Int32 n = await db.SaveChangesAsync();
        if (n > 0)
        {
            cache.Set("FlatsContract" + flatsContract.Id, flatsContract, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDuration)
            });
        }
    }
    
    public async Task<Int32> UpdateFlatsContract(FlatsContract flatsContract)
    {
        if (!db.FlatsContracts.Any(l => l.Id == flatsContract.Id))
        {
            return 0;
        }
        db.Entry(flatsContract).State = EntityState.Modified;
        db.FlatsContracts.Update(flatsContract);
        Int32 n = await db.SaveChangesAsync();
        
        if (n > 0)
        {
            cache.Set("FlatsContract" + flatsContract.Id, flatsContract, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDuration)
            });
        }

        return n;
    }
    
    public async Task<Int32> DeleteFlatsContract(Int32 id)
    {
        FlatsContract flatsContract = null;
        flatsContract = await db.FlatsContracts.FirstOrDefaultAsync(l => l.Id == id);
        if (flatsContract == null)
        {
            return 0;
        }
        db.FlatsContracts.Remove(flatsContract);
        Int32 n = await db.SaveChangesAsync();
        return n;
    }
}