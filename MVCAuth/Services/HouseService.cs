using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MVCAuth.Models;

namespace MVCAuth.Services;

public class HouseService(RealEstateRentalContext db, IMemoryCache cache)
{
    private readonly Int32 _cacheDuration = 300;
    private readonly String _houseChacheKey = "Houses";

    public async Task<IEnumerable<House>> GetHouses(int pagenumber, int pagesize)
    {
        int maxPages = (int)db.Houses.Count() % pagesize == 0 ? 
            (int)db.Houses.Count() / pagesize : (int)(db.Houses.Count() / pagesize) + 1;
        if (pagenumber < 1 || pagenumber > maxPages)
        {
            pagenumber = 1;
        }
        
        IEnumerable<House> houses = null;
        houses = await db.Houses.Include(ll => ll.Ll)
            .ThenInclude(Ll => Ll.LandLordsAdditionalInfo)
            .Skip(pagesize * (pagenumber - 1))
            .Take(pagesize)
            .OrderBy(house => house.Pid)
            .ToListAsync();
        return houses;
    }

    public async Task<House> GetHouse(Int32 id)
    {
        House house = null;
        if (!cache.TryGetValue("House" + id, out house))
        {
            house = await db.Houses.FirstOrDefaultAsync(l => l.Pid == id);
            if (house != null)
            {
                cache.Set("House" + house.Pid, house,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheDuration)));
            }
        }

        return house;
    }

    public async Task AddHouse(House house)
    {
        db.Houses.Add(house);
        Int32 n = await db.SaveChangesAsync();
        if (n > 0)
        {
            cache.Set("House" + house.Pid, house, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDuration)
            });
        }
    }
    
    public async Task<Int32> UpdateHouse(House house)
    {
        if (!db.Houses.Any(l => l.Pid == house.Pid))
        {
            return 0;
        }
        
        db.Houses.Update(house);
        Int32 n = await db.SaveChangesAsync();
        
        if (n > 0)
        {
            cache.Set("House" + house.Pid, house, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDuration)
            });
        }

        return n;
    }
    
    public async Task<Int32> DeleteHouse(Int32 id)
    {
        House house = null;
        house = await db.Houses.FirstOrDefaultAsync(l => l.Pid == id);
        if (house == null)
        {
            return 0;
        }
        db.Houses.Remove(house);
        Int32 n = await db.SaveChangesAsync();
        return n;
    }
    
    public async Task<List<House>> GetByFilter(string city, decimal averageCost)
    {
        var q = db.Houses
            .Include(ll => ll.Ll)
            .ThenInclude(Ll => Ll.LandLordsAdditionalInfo)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(city))
        {
            q = q.Where(f => f.City == city);
        }

        if (averageCost > 0)
        {
            q = q.Where(flat => flat.CostPerDay > averageCost);
        }

        return await q.ToListAsync();
    }
}