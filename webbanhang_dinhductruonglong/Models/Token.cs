using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webbanhang_dinhductruonglong.Models
{
    public class Token
    {
        [Key]
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
