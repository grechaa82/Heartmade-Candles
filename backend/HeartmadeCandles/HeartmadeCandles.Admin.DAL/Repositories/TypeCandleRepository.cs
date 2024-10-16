using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeartmadeCandles.Admin.DAL.Repositories;

public class TypeCandleRepository : ITypeCandleRepository
{
    private readonly AdminDbContext _context;

    public TypeCandleRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<Maybe<TypeCandle[]>> GetAll()
    {
        var items = await _context.TypeCandle
            .AsNoTracking()
            .ToArrayAsync();

        if (!items.Any())
        {
            return Maybe<TypeCandle[]>.None;
        }

        var result = items
            .Select(TypeCandleMapping.MapToCandleType)
            .ToArray();

        return result;
    }

    public async Task<Maybe<TypeCandle>> Get(int typeCandleId)
    {
        var item = await _context.TypeCandle
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == typeCandleId);

        if (item == null)
        {
            return Maybe<TypeCandle>.None;
        }

        var typeCandle = TypeCandleMapping.MapToCandleType(item);

        return typeCandle;
    }

    public async Task<Maybe<TypeCandle>> Get(string typeCandleTitle)
    {
        var item = await _context.TypeCandle
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Title == typeCandleTitle);

        if (item == null)
        {
            return Maybe<TypeCandle>.None;
        }

        var typeCandle = TypeCandleMapping.MapToCandleType(item);

        return typeCandle;
    }

    public async Task<Result> Create(TypeCandle typeCandle)
    {
        var item = TypeCandleMapping.MapToTypeCandleEntity(typeCandle);

        await _context.TypeCandle.AddAsync(item);
        var created = await _context.SaveChangesAsync();

        return created > 0
            ? Result.Success()
            : Result.Failure($"TypeCandle {typeCandle.Title} was not created");
    }

    public async Task<Result> Update(TypeCandle typeCandle)
    {
        var item = TypeCandleMapping.MapToTypeCandleEntity(typeCandle);

        _context.TypeCandle.Update(item);
        var updated = await _context.SaveChangesAsync();

        return updated > 0
            ? Result.Success()
            : Result.Failure($"TypeCandle {typeCandle.Title} was not updated");
    }

    public async Task<Result> Delete(int typeCandleId)
    {
        var item = await _context.TypeCandle.FirstOrDefaultAsync(c => c.Id == typeCandleId);

        if (item == null)
        {
            return Result.Failure($"TypeCandle by id: {typeCandleId} does not exist");
        }

        _context.TypeCandle.Remove(item);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0
            ? Result.Success()
            : Result.Failure($"TypeCandle by id: {typeCandleId} was not deleted");
    }
}