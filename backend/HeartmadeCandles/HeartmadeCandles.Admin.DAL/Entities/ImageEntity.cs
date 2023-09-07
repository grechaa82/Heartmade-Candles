using System.ComponentModel.DataAnnotations.Schema;

namespace HeartmadeCandles.Admin.DAL.Entities
{
    public class ImageEntity
    {
        [Column("fileName")]
        public string FileName { get; set; }

        [Column("alternativeName")]
        public string AlternativeName { get; set; }
    }
}