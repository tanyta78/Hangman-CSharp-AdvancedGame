using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    public class Users
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }


        public double Score { get; set; }
    }
}
