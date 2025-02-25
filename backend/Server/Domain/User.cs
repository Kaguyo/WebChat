using System.ComponentModel.DataAnnotations;

namespace Server.Domain
{
    public class User() 
    {
        [Key]
        public string Username { get; set; } = String.Empty;
        public string Number { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string? ProfileImage { get; set; } = String.Empty;

    }
}