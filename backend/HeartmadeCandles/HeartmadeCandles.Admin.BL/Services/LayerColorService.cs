using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.BL.Services;

public class LayerColorService : ILayerColorService
{
    private readonly ILayerColorRepository _layerColorRepository;

    public LayerColorService(ILayerColorRepository layerColorRepository)
    {
        _layerColorRepository = layerColorRepository;
    }

    public async Task<Maybe<LayerColor[]>> GetAll()
    {
        return await _layerColorRepository.GetAll();
    }

    public async Task<Maybe<LayerColor>> Get(int layerColorId)
    {
        return await _layerColorRepository.Get(layerColorId);
    }

    public async Task<Result> Create(LayerColor layerColor)
    {
        return await _layerColorRepository.Create(layerColor);
    }

    public async Task<Result> Update(LayerColor layerColor)
    {
        return await _layerColorRepository.Update(layerColor);
    }

    public async Task<Result> Delete(int layerColorId)
    {
        return await _layerColorRepository.Delete(layerColorId);
    }
}