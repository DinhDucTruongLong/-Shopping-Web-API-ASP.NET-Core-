using System.ComponentModel.DataAnnotations;

namespace webbanhang_dinhductruonglong.Models
{
    public class Role
    {

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } // ví dụ: "Admin", "User"
    }
}
