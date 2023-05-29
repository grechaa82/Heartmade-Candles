using HeartmadeCandles.Admin.Core.Models;
using HeartmadeCandles.Admin.DAL.Entities;

namespace HeartmadeCandles.Admin.DAL.Mapping
{
    internal class WickMapping
    {
        public static Wick MapToWick(WickEntity wickEntity)
        {
            var wick = Wick.Create(
                wickEntity.Title, 
                wickEntity.Description, 
                wickEntity.Price, 
                wickEntity.ImageURL, 
                wickEntity.IsActive, 
                wickEntity.Id);

            return wick.Value;
        }

        public static WickEntity MapToWickEntity(Wick wick)
        {
            var wickEntity = new WickEntity()
            {
                Id = wick.Id,
                Title = wick.Title,
                Description = wick.Description,
                Price = wick.Price,
                ImageURL = wick.ImageURL,
                IsActive = wick.IsActive,

            }; ;

            return wickEntity;
        }
    }
}
