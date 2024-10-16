using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories;

public class SmellRepository : ISmellRepository
{
    private readonly AdminDbContext _context;

    public SmellRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<(Maybe<Smell[]>, long)> GetAll(PaginationSettings pagination)
    {
        long totalCount = await _context.Decor.CountAsync();

        var items = await _context.Smell
            .AsNoTracking()
            .Skip(pagination.PageSize * pagination.PageIndex)
            .Take(pagination.PageSize)
            .ToArrayAsync();

        if (!items.Any())
        {
            return (Maybe<Smell[]>.None, totalCount);
        }

        var result = items
            .Select(SmellMapping.MapToSmell)
            .ToArray();

        return (result, totalCount);
    }

    public async Task<Maybe<Smell>> Get(int smellId)
    {
        var item = await _context.Smell
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == smellId);

        if (item == null)
        {
            return Maybe<Smell>.None;
        }

        var smell = SmellMapping.MapToSmell(item);

        return smell;
    }

    public async Task<Maybe<Smell[]>> GetByIds(int[] smellIds)
    {
        var items = await _context.Smell
            .AsNoTracking()
            .Where(c => smellIds.Contains(c.Id))
            .ToArrayAsync();

        if (!items.Any())
        {
            return Maybe<Smell[]>.None;
        }

        var result = items
            .Select(SmellMapping.MapToSmell)
            .ToArray();

        return result;
    }

    public async Task<Result> Create(Smell smell)
    {
        var result = SmellMapping.MapToSmellEntity(smell);

        await _context.Smell.AddAsync(result);
        var created = await _context.SaveChangesAsync();

        return created > 0
            ? Result.Success()
            : Result.Failure($"Smell {smell.Title} was not created");
    }

    public async Task<Result> Update(Smell smell)
    {
        var item = SmellMapping.MapToSmellEntity(smell);

        _context.Smell.Update(item);
        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"Smell {smell.Title} was not updated");
    }

    public async Task<Result> Delete(int smellId)
    {
        var item = await _context.Smell.FirstOrDefaultAsync(c => c.Id == smellId);

        if (item == null)
        {
            return Result.Failure($"Smell by id: {smellId} does not exist");
        }

        _context.Smell.Remove(item);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? Result.Success()
            : Result.Failure($"Smell by id: {smellId} was not deleted");
    }

    public async Task<Result> UpdateCandleSmell(int candleId, Smell[] smells)
    {
        var existingSmells = await _context.CandleSmell
            .Where(c => c.CandleId == candleId)
            .ToArrayAsync();

        var smellsToDelete = existingSmells
            .Where(es => smells.All(s => s.Id != es.SmellId))
            .ToArray();

        var smellsToAdd = smells
            .Where(s => existingSmells.All(es => es.SmellId != s.Id))
            .Select(s => new CandleEntitySmellEntity { CandleId = candleId, SmellId = s.Id })
            .ToArray();

        if (!smellsToDelete.Any() && !smellsToAdd.Any())
        {
            Result.Failure($"There are no Smells of candle by id: {candleId} that need to be updated");
        }

        _context.CandleSmell.RemoveRange(smellsToDelete);
        _context.CandleSmell.AddRange(smellsToAdd);

        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"Smells of candle by id {candleId} were not updated");
    }
}