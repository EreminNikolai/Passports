using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Passports.Identity.Models
{
    [Table("users")]
    public class AppUser : IdentityUser
    {
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
    }
}
