using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MVCAuth.ModelViews;
using MVCAuth.Services.Jwt;

namespace MVCAuth.Services
{
    public class LandLordService(RealEstateRentalContext db, IMemoryCache cache, GiveToken giveToken)
    {
        private readonly Int32 _cacheDuration = 300;
        private String _landLordChacheKey = "LandLord_All";

        public async Task<IEnumerable<LandLord>> GetLandLords(int pagenumber, int pagesize)
        {
            int maxPages = (int)db.LandLords.Count() % pagesize == 0 ? (int)db.LandLords.Count() / pagesize : (int)(db.LandLords.Count() / pagesize) + 1;
            if (pagenumber < 1 || pagenumber > maxPages)
            {
                pagenumber = 1;
            }
            IEnumerable<LandLord> landLords = null;
            if (!cache.TryGetValue(_landLordChacheKey + $"_{pagenumber}", out landLords))
            {
                landLords = await db.LandLords.Skip(pagesize * (pagenumber - 1)).Take(pagesize).ToListAsync();
                if (landLords != null)
                {
                    cache.Set(_landLordChacheKey + $"_{pagenumber}", landLords, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheDuration)));
                }
            }
            return landLords;
        }

        public async Task<IEnumerable<LandLord>> GetAllLandLords()
        {

            IEnumerable<LandLord> landLords = null;

            landLords = await db.LandLords.ToListAsync();
  
            return landLords;
        }
        
        
        public async Task<LandLord> GetLandLord(Int32 id)
        {
            LandLord landLord = null;
            if (!cache.TryGetValue("LandLord" + id, out landLord))
            {
                landLord = await db.LandLords.FirstOrDefaultAsync(l => l.Llid == id);
                if (landLord != null)
                {
                    cache.Set("LandLord" + landLord.Llid, landLord,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheDuration)));
                }
            }

            return landLord;
        }

        public LandLord GetLandLord(string login, string password)
        {
            LandLord landLord = null;
            if (!cache.TryGetValue("LandLord" + "id", out landLord))
            {
                landLord = db.LandLords.FirstOrDefault(ll => ll.Login == login && ll.Password == password);
                if (landLord != null)
                {
                    cache.Set("LandLord" + "id", landLord,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheDuration)));
                }
            }

            return landLord;
        }

        public async Task AddLandLord(LandLord landLord)
        {
            db.LandLords.Add(landLord);
            Int32 n = await db.SaveChangesAsync();
            if (n > 0)
            {
                cache.Set("LandLord" + landLord.Llid, landLord, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDuration)
                });
            }
        }

        public async Task<Int32> UpdateLandLord(LandLord landLord)
        {
            if (!db.LandLords.Any(l => l.Llid == landLord.Llid))
            {
                return 0;
            }

            // db.LandLords.Update(landLord);
            db.Entry(landLord).State = EntityState.Modified;
            Int32 n = await db.SaveChangesAsync();

            return n;
        }

        public async Task<Int32> DeleteLandLord(Int32 id)
        {
            LandLord landLord = null;
            landLord = await db.LandLords.FirstOrDefaultAsync(l => l.Llid == id);
            if (landLord == null)
            {
                return 0;
            }
            db.LandLords.Remove(landLord);
            Int32 n = await db.SaveChangesAsync();
            return n;
        }

        public async Task<string> LoginLandlord(UserModelView user)
        {
            LandLord landLord = await db.LandLords.FirstOrDefaultAsync(ll => ll.Login == user.Login & ll.Password == user.Password);

            if (landLord == null)
            {
                return null;
            }

            var tokenWithInfo = await giveToken.Token(user);
            return tokenWithInfo;

        }

        public async Task<IEnumerable<LandLord>> GetLandLordsWithInfo(int pagenumber, int pagesize)
        {
            int maxPages = (int)db.LandLords.Count() % pagesize == 0 ? (int)db.LandLords.Count() / pagesize : (int)(db.LandLords.Count() / pagesize) + 1;
            if (pagenumber < 1 || pagenumber > maxPages)
            {
                pagenumber = 1;
            }
            
            IEnumerable<LandLord> landLords = null;
            
            landLords = await db.LandLords
                .Include(landlord => landlord.LandLordsAdditionalInfo)
                .Skip(pagesize * (pagenumber - 1))
                .Take(pagesize)
                .OrderBy(landlord => landlord.Llid)
                .ToListAsync();
            return landLords;
        }
        
    }
    
}
