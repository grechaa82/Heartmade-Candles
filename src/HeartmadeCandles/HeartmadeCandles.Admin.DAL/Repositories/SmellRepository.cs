using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories
{
    public class SmellRepository : ISmellRepository
    {
        private readonly AdminDbContext _context;

        public SmellRepository(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<Smell>> GetAll()
        {
            var result = new List<Smell>();

            var items = await _context.Smell
                .AsNoTracking()
                .ToArrayAsync();

            foreach (var item in items)
            {
                var smell = SmellMapping.MapToSmell(item);

                result.Add(smell);
            }

            return result;
        }
        
        public async Task<Smell> Get(int id)
        {
            var item = await _context.Smell
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var smell = SmellMapping.MapToSmell(item);

            return smell;
        }
        
        public async Task Create(Smell smell)
        {
            var result = SmellMapping.MapToSmellEntity(smell);

            await _context.Smell.AddAsync(result);
            await _context.SaveChangesAsync();
        }
        
        public async Task Update(Smell smell)
        {
            var item = SmellMapping.MapToSmellEntity(smell);

            _context.Smell.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _context.Smell.FirstOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                _context.Smell.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCandleSmell(int candleId, List<Smell> smells)
        {
            var smellsToDelete = _context.CandleSmell.Where(c => c.CandleId == candleId).ToList();
            _context.RemoveRange(smellsToDelete);
            await _context.SaveChangesAsync();

            var smellsToAdd = new List<CandleEntitySmellEntity>();
            foreach (var smell in smells)
            {
                var smellEntity = new CandleEntitySmellEntity()
                {
                    CandleId = candleId,
                    SmellId = smell.Id
                };

                smellsToAdd.Add(smellEntity);
            }

            _context.AddRange(smellsToAdd);
            await _context.SaveChangesAsync();
        }
    }
}
