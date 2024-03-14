using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.UserAndAuth.DAL.Entities;

public class TokenEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("userId")]
    [Required]
    public int UserId { get; set; }

    [Column("accessToken")]
    [Required]
    public required string AccessToken { get; set; }

    [Column("refreshToken")]
    [Required]
    public required string RefreshToken { get; set; }

    [Column("expireTime")]
    [Required]
    public DateTime ExpireTime { get; set; }
}
