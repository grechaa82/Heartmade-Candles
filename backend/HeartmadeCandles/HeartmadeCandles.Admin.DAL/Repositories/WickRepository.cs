using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories;

public class WickRepository : IWickRepository
{
    private readonly AdminDbContext _context;

    public WickRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<(Maybe<Wick[]>, long)> GetAll(PaginationSettings pagination)
    {
        long totalCount = await _context.Wick.CountAsync();

        var items = await _context.Wick
            .AsNoTracking()
            .Skip(pagination.PageSize * pagination.PageIndex)
            .Take(pagination.PageSize)
            .ToArrayAsync();

        if (!items.Any())
        {
            return (Maybe<Wick[]>.None, totalCount);
        }

        var result = items
            .Select(WickMapping.MapToWick)
            .ToArray();

        return (result, totalCount);
    }

    public async Task<Maybe<Wick>> Get(int wickId)
    {
        var item = await _context.Wick
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == wickId);

        if (item == null)
        {
            return Maybe<Wick>.None;
        }

        var wick = WickMapping.MapToWick(item);

        return wick;
    }

    public async Task<Maybe<Wick[]>> GetByIds(int[] wickIds)
    {
        var items = await _context.Wick
            .AsNoTracking()
            .Where(c => wickIds.Contains(c.Id))
            .ToArrayAsync();

        if (!items.Any())
        {
            return Maybe<Wick[]>.None;
        }

        var result = items
            .Select(WickMapping.MapToWick)
            .ToArray();

        return result;
    }

    public async Task<Result> Create(Wick wick)
    {
        var item = WickMapping.MapToWickEntity(wick);

        await _context.Wick.AddAsync(item);
        var created = await _context.SaveChangesAsync();

        return created > 0
            ? Result.Success()
            : Result.Failure($"Wick {wick.Title} was not created");
    }

    public async Task<Result> Update(Wick wick)
    {
        var item = WickMapping.MapToWickEntity(wick);

        _context.Wick.Update(item);
        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"Wick {wick.Title} was not updated");
    }

    public async Task<Result> Delete(int wickId)
    {
        var item = await _context.Wick.FirstOrDefaultAsync(c => c.Id == wickId);

        if (item == null)
        {
            return Result.Failure($"Wick by id: {wickId} does not exist");
        }

        _context.Wick.Remove(item);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? Result.Success()
            : Result.Failure($"Wick by id: {wickId} was not deleted");
    }

    public async Task<Result> UpdateCandleWick(int candleId, Wick[] wicks)
    {
        var existingWicks = await _context.CandleWick
            .Where(c => c.CandleId == candleId)
            .ToArrayAsync();

        var wicksToDelete = existingWicks
            .Where(ew => wicks.All(w => w.Id != ew.WickId))
            .ToArray();

        var wicksToAdd = wicks
            .Where(w => existingWicks.All(ew => ew.WickId != w.Id))
            .Select(w => new CandleEntityWickEntity { CandleId = candleId, WickId = w.Id })
            .ToArray();

        if (!wicksToDelete.Any() && !wicksToAdd.Any())
        {
            Result.Failure($"There are no Wicks of candle by id: {candleId} that need to be updated");
        }

        _context.CandleWick.RemoveRange(wicksToDelete);
        _context.CandleWick.AddRange(wicksToAdd);

        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"Wicks of candle by id {candleId} were not updated");
    }
}