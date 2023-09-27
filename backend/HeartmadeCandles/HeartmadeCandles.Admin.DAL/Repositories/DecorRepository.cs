using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories;

public class DecorRepository : IDecorRepository
{
    private readonly AdminDbContext _context;

    public DecorRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<Maybe<Decor[]>> GetAll()
    {
        var items = await _context.Decor
            .AsNoTracking()
            .ToArrayAsync();

        if (!items.Any())
        {
            return Maybe<Decor[]>.None;
        }

        var result = items
            .Select(DecorMapping.MapToDecor)
            .ToArray();

        return result;
    }

    public async Task<Maybe<Decor>> Get(int decorId)
    {
        var item = await _context.Decor
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == decorId);

        if (item == null)
        {
            return Maybe<Decor>.None;
        }

        var decor = DecorMapping.MapToDecor(item);

        return decor;
    }

    public async Task<Maybe<Decor[]>> GetByIds(int[] decorIds)
    {
        var items = await _context.Decor
            .AsNoTracking()
            .Where(c => decorIds.Contains(c.Id))
            .ToArrayAsync();

        if (!items.Any())
        {
            return Maybe<Decor[]>.None;
        }

        var result = items
            .Select(DecorMapping.MapToDecor)
            .ToArray();

        return result;
    }

    public async Task<Result> Create(Decor decor)
    {
        var item = DecorMapping.MapToDecorEntity(decor);

        await _context.Decor.AddAsync(item);
        var created = await _context.SaveChangesAsync();

        return created > 0
            ? Result.Success()
            : Result.Failure($"Decor {decor.Title} was not created");
    }

    public async Task<Result> Update(Decor decor)
    {
        var item = DecorMapping.MapToDecorEntity(decor);

        _context.Decor.Update(item);
        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"Decor {decor.Title} was not updated");
    }

    public async Task<Result> Delete(int decorId)
    {
        var item = await _context.Decor.FirstOrDefaultAsync(c => c.Id == decorId);

        if (item == null)
        {
            return Result.Failure($"Decor by id: {decorId} does not exist");
        }

        _context.Decor.Remove(item);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? Result.Success()
            : Result.Failure($"Decor by id: {decorId} was not deleted");
    }

    public async Task<Result> UpdateCandleDecor(int candleId, Decor[] decors)
    {
        var existingDecors = await _context.CandleDecor
            .Where(c => c.CandleId == candleId)
            .ToArrayAsync();

        var decorsToDelete = existingDecors
            .Where(ed => decors.All(d => d.Id != ed.DecorId))
            .ToArray();

        var decorsToAdd = decors
            .Where(d => existingDecors.All(ed => ed.DecorId != d.Id))
            .Select(d => new CandleEntityDecorEntity { CandleId = candleId, DecorId = d.Id })
            .ToArray();

        if (!decorsToDelete.Any() && !decorsToAdd.Any())
        {
            Result.Failure($"There are no Decors of candle by id: {candleId} that need to be updated");
        }

        _context.CandleDecor.RemoveRange(decorsToDelete);
        _context.CandleDecor.AddRange(decorsToAdd);

        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"Decors of candle by id {candleId} were not updated");
    }
}