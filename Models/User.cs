using System.ComponentModel.DataAnnotations;

namespace SOA.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        public string UserName { get; set; } = "";

        public string Password { get; set; } = "";

        public string Token { get; set; } = "";
    }
}