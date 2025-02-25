using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entities
{
    public class User() 
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("number")]
        public string? Number { get; set; }

        [Column("password")]
        public string? Password { get; set; }

    }
}