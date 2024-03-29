using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.UserAndAuth.DAL.Entities;

[Table("Session")]
public class SessionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("userId")]
    [Required]
    public int UserId { get; set; }

    [Column("refreshToken")]
    [Required]
    public required string RefreshToken { get; set; }

    [Column("expireAt")]
    [Required]
    public DateTime ExpireAt { get; set; }

    public virtual UserEntity? User { get; set; }
}
