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

        [Column("firstname")]
        public string? FirstName { get; set; }

        [Column("lastname")]
        public string? LastName { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("number")]
        public string? Number { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("profileimg")]
        public string? ProfileImg { get; set; }
    }
}