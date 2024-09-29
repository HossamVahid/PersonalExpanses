using Manage_Personal_Expenses.Models;
using Microsoft.EntityFrameworkCore;

namespace Manage_Personal_Expenses.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<UserModel> UserModels { get; set; }
        public DbSet<ExpensesModel> ExpensesModels { get; set;}
    }
}
