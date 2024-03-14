using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.UserAndAuth.DAL.Entities;

public class UserEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("email")]
    [Required]
    public required string Email { get; set; }

    [Column("userName")]
    [Required]
    public required string UserName { get; set; }

    [Column("passwordHash")]
    [Required]
    public required string PasswordHash { get; set; }

    [Column("registrationDate")]
    [Required]
    public DateTime RegistrationDate { get; set; }
}
