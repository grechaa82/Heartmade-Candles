using CSharpFunctionalExtensions;
using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface ILayerColorService
{
    Task<(Result<LayerColor[]>, long)> GetAll(PaginationSettings pagination);

    Task<Maybe<LayerColor>> Get(int layerColorId);

    Task<Result> Create(LayerColor layerColor);

    Task<Result> Update(LayerColor layerColor);

    Task<Result> Delete(int layerColorId);
}