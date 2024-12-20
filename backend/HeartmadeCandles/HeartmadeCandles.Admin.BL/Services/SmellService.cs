﻿using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services;

public class SmellService : ISmellService
{
    private readonly ISmellRepository _smellRepository;

    public SmellService(ISmellRepository smellRepository)
    {
        _smellRepository = smellRepository;
    }

    public async Task<(Result<Smell[]>, long)> GetAll(PaginationSettings pagination)
    {
        var (smellsMaybe, totalCount) = await _smellRepository.GetAll(pagination);

        if (!smellsMaybe.HasValue)
        {
            return (Result.Failure<Smell[]>("Smells not found"), totalCount);
        }

        return (Result.Success(smellsMaybe.Value), totalCount);
    }

    public async Task<Maybe<Smell>> Get(int smellId)
    {
        return await _smellRepository.Get(smellId);
    }

    public async Task<Result> Create(Smell smell)
    {
        return await _smellRepository.Create(smell);
    }

    public async Task<Result> Update(Smell smell)
    {
        return await _smellRepository.Update(smell);
    }

    public async Task<Result> Delete(int smellId)
    {
        return await _smellRepository.Delete(smellId);
    }
}