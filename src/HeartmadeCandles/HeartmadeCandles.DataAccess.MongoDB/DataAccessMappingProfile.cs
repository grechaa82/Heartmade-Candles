using AutoMapper;
using HeartmadeCandles.Core.Models;
using HeartmadeCandles.DataAccess.MongoDB.Collections;

namespace HeartmadeCandles.DataAccess.MongoDB
{
    public class DataAccessMappingProfile : Profile
    {
        public DataAccessMappingProfile()
        {
            CreateMap<Candle, CandleCollection>().ReverseMap();
            CreateMap<Decor, DecorCollection>().ReverseMap();
            CreateMap<LayerColor, LayerColorCollection>().ReverseMap();
            CreateMap<Smell, SmellCollection>().ReverseMap();
        }
    }
}   