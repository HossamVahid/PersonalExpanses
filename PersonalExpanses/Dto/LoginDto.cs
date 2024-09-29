using System.ComponentModel.DataAnnotations;

namespace Manage_Personal_Expenses.Dto
{
    public class LoginDto
    {
       
           

            [EmailAddress]
            public string Email { get; set; }

            public string Password { get; set; }
        
    }
}
