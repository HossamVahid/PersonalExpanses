using Manage_Personal_Expenses.Dto;
using Manage_Personal_Expenses.Models;

namespace Manage_Personal_Expenses.Mapper
{
    public class RegisterMapper
    {

        public UserModel MapToUser(RegisterDto registerDto)
        {
            return new UserModel
            {

                Email = registerDto.Email,
                Name = registerDto.Name,
                Password = registerDto.Password

            };

        }
    }
}
