using AutoMapper;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Models;
using HeartmadeCandles.DataAccess.MongoDB.Collections;
using MongoDB.Driver;

namespace HeartmadeCandles.DataAccess.MongoDB.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IMongoCollection<DecorCollection> _decorCollection;
        private readonly IMongoCollection<LayerColorCollection> _layerColorCollection;
        private readonly IMongoCollection<SmellCollection> _smellCollection;
        private readonly IMongoCollection<OrderCollection> _orderCollection;
        private readonly IMapper _mapper;

        public AdminRepository(IMongoDatabase mongoDatabase, IMapper mapper)
        {
            _decorCollection = mongoDatabase.GetCollection<DecorCollection>("Decor");
            _layerColorCollection = mongoDatabase.GetCollection<LayerColorCollection>("LayerColor");
            _smellCollection = mongoDatabase.GetCollection<SmellCollection>("Smell");
            _orderCollection = mongoDatabase.GetCollection<OrderCollection>("Order");
            _mapper = mapper;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : ModelBase
        {
            switch (typeof(T).Name)
            {
                case "Decor":
                    var decors = _decorCollection.Find(decor => true).ToEnumerable();
                    return (IEnumerable<T>)_mapper.Map<IEnumerable<DecorCollection>, IEnumerable<Decor>>(decors);

                case "LayerColor":
                    var layerColors = _layerColorCollection.Find(LayerColor => true).ToEnumerable();
                    return (IEnumerable<T>)_mapper.Map<IEnumerable<LayerColorCollection>, IEnumerable<LayerColor>>(layerColors);

                case "Smell":
                    var smells = _smellCollection.Find(smell => true).ToEnumerable();
                    return (IEnumerable<T>)_mapper.Map<IEnumerable<SmellCollection>, IEnumerable<Smell>>(smells);

                case "Order":
                    var order = _orderCollection.Find(order => true).ToEnumerable();
                    return (IEnumerable<T>)_mapper.Map<IEnumerable<OrderCollection>, IEnumerable<Order>>(order);

                default: return default;
            }
        }

        public async Task CreateAsync<T>(T t) where T : ModelBase
        {
            try
            {
                switch (t)
                {
                    case Candle:
                        break;
                    case Decor:
                        var decorCollection = _mapper.Map<Decor, DecorCollection>(t as Decor);
                        await _decorCollection.InsertOneAsync(decorCollection);
                        break;
                    case LayerColor:
                        var layerColorCollection = _mapper.Map<LayerColor, LayerColorCollection>(t as LayerColor);
                        await _layerColorCollection.InsertOneAsync(layerColorCollection);
                        break;
                    case Smell:
                        var smellCollection = _mapper.Map<Smell, SmellCollection>(t as Smell);
                        await _smellCollection.InsertOneAsync(smellCollection);
                        break;
                }
            }
            catch { }
        }

        public async Task UpdateAsync<T>(T t) where T : ModelBase
        {
            if (t is null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                switch (t)
                {
                    case Candle:
                        break;
                    case Decor:
                        if (IsExist(t))
                        {
                            var decorCollection = _mapper.Map<Decor, DecorCollection>(t as Decor);
                            await _decorCollection.ReplaceOneAsync(d => d.Id == t.Id, decorCollection);
                        }
                        break;
                    case LayerColor:
                        if (IsExist(t))
                        {
                            var layerColorCollection = _mapper.Map<LayerColor, LayerColorCollection>(t as LayerColor);
                            await _layerColorCollection.ReplaceOneAsync(d => d.Id == t.Id, layerColorCollection);
                        }
                        break;
                    case Smell:
                        if (IsExist(t))
                        {
                            var smellCollection = _mapper.Map<Smell, SmellCollection>(t as Smell);
                            await _smellCollection.ReplaceOneAsync(d => d.Id == t.Id, smellCollection);
                        }
                        break;
                }
            }
            catch { }
        }

        public async Task DeleteAsync<T>(T t) where T : ModelBase
        {
            try
            {
                switch (t)
                {
                    case Candle:
                        break;
                    case Decor:
                        if (IsExist(t))
                        {
                            var decorCollection = _mapper.Map<Decor, DecorCollection>(t as Decor);
                            await _decorCollection.DeleteOneAsync(decor => decor.Id == t.Id);
                        }
                        break;
                    case LayerColor:
                        if (IsExist(t))
                        {
                            var layerColorCollection = _mapper.Map<LayerColor, LayerColorCollection>(t as LayerColor);
                            await _layerColorCollection.DeleteOneAsync(layerColor => layerColor.Id == t.Id);
                        }
                        break;
                    case Smell:
                        if (IsExist(t))
                        {
                            var smellCollection = _mapper.Map<Smell, SmellCollection>(t as Smell);
                            await _smellCollection.DeleteOneAsync(smell => smell.Id == t.Id);
                        }
                        break;
                }
            }
            catch { }
        }

        private bool IsExist<T>(T t) where T : ModelBase
        {
            switch (t)
            {
                case Decor:
                    var decor = _decorCollection.Find(d => d.Id == t.Id).FirstAsync();
                    if (decor is null)
                        return false;
                    return true;

                case LayerColor:
                    var layerColor = _layerColorCollection.Find(d => d.Id == t.Id).FirstAsync();
                    if (layerColor is null)
                        return false;
                    return true;

                case Smell:
                    var smell = _smellCollection.Find(d => d.Id == t.Id).FirstAsync();
                    if (smell is null)
                        return false;
                    return true;

                default:
                    return false;
            }
        }
    }
}