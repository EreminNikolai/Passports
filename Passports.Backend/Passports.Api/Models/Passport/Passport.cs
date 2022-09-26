using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Passports.Api.Models.Passport;

/// <summary>
/// Модель паспорта
/// </summary>
[Table("passports")]
internal class Passport
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("series")]
    public uint Series { get; set; }
    [Column("number")]
    public uint Number { get; set; }
}