using Microsoft.EntityFrameworkCore;
using MVCAuth.Models;
using MVCAuth.ModelViews;

namespace MVCAuth.Services;

public class FlatService
{
    private RealEstateRentalContext db;
    private readonly Int32 cacheDuration = 252;
    private String FlatChacheKey = "Flats";
    private RealEstateRentalContext dbContext;

    public FlatService(RealEstateRentalContext db)
    {
        this.db = db;
    }

  

    public async Task<IEnumerable<Flat>> GetFlats(int pagenumber, int pagesize)
    {
        int maxPages = (int)db.Flats.Count() % pagesize == 0 ? 
            (int)db.Flats.Count() / pagesize : (int)(db.Flats.Count() / pagesize) + 1;
        if (pagenumber < 1 || pagenumber > maxPages)
        {
            pagenumber = 1;
        }
        IEnumerable<Flat> flats = null;
        flats = await db.Flats.Include(ll => ll.Ll)
            .ThenInclude(Ll => Ll.LandLordsAdditionalInfo)
            .Skip(pagesize * (pagenumber - 1))
            .Take(pagesize)
            .OrderBy(flat => flat.Fid)
            .ToListAsync();
        return flats;
    }
    public async Task<IEnumerable<Flat>> GetAllFlats()
    {

        IEnumerable<Flat> flats = null;
        flats = await db.Flats
            .ToListAsync();
        return flats;
    }

    public async Task<IEnumerable<Flat>> GetViewModelFlats(int pagenumber, int pagesize)
    {
        int maxPages = (int)db.Flats.Count() % pagesize == 0 ? 
            (int)db.Flats.Count() / pagesize : (int)(db.Flats.Count() / pagesize) + 1;
        if (pagenumber < 1 || pagenumber > maxPages)
        {
            pagenumber = 1;
        }
        IEnumerable<Flat> flats = null;
        flats = await db.Flats.Include(ll => ll.Ll)
            .ThenInclude(Ll => Ll.LandLordsAdditionalInfo)
            .Skip(pagesize * (pagenumber - 1))
            .Take(pagesize)
            .OrderBy(flat => flat.Fid)
            .ToListAsync();
        return flats;
    }
    
    public async Task<Flat> GetFlat(Int32 id)
    {
        Flat flat = null;

            flat = await db.Flats.FirstOrDefaultAsync(l => l.Fid == id);
 

        return flat;
    }

    public async Task AddFlat(Flat flat)
    {
        db.Flats.Add(flat);
        Int32 n = await db.SaveChangesAsync();
 

    }

    public async Task<Int32> UpdateFlat(Flat flat)
    {
        if (!db.Flats.Any(l => l.Fid == flat.Fid))
        {
            return 0;
        }

        db.Flats.Update(flat);
        Int32 n = await db.SaveChangesAsync();


        return n;
    }

    public async Task<Int32> DeleteFlat(Int32 id)
    {
        Flat flat = null;
        flat = await db.Flats.FirstOrDefaultAsync(l => l.Fid == id);
        Console.WriteLine(flat);
        if (flat == null)
        {
            return 0;
        }
        db.Flats.Remove(flat);
        Int32 n = await db.SaveChangesAsync();
        return n;
    }
    
    public async Task<List<Flat>> GetByFilter(string city, decimal averageCost)
    {
        var q = db.Flats
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