using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Manage_Personal_Expenses.Models
{
    [Table("user_table")]
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        [EmailAddress]
        public string? Email { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("password")]
        public string? Password { get; set; }

       

        public UserModel() { }

        public UserModel(int id, string email, string name, string password) { 
        
            Id= id;
            Email= email;
            Name= name;
            Password= password;
        
        }

        



    }
}
