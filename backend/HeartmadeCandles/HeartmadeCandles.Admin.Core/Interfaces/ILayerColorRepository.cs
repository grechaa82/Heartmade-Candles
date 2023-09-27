using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface ILayerColorRepository
{
    Task<Maybe<LayerColor[]>> GetAll();
    Task<Maybe<LayerColor>> Get(int layerColorId);
    Task<Maybe<LayerColor[]>> GetByIds(int[] layerColorIds);
    Task<Result> Create(LayerColor layerColor);
    Task<Result> Update(LayerColor layerColor);
    Task<Result> Delete(int layerColorId);
    Task<Result> UpdateCandleLayerColor(int candleId, LayerColor[] layerColors);
}