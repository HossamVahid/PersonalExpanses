using System.ComponentModel.DataAnnotations.Schema;

namespace Manage_Personal_Expenses.Models
{
   public enum category { subscription, one_time_pay }
    [Table("expensess_table")]
    public class ExpensesModel
    {
        

        [Column("id")]
        public int Id { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Price")]
        public decimal Price { get; set; }

        [Column("Category")]
        public category Category { get; set; }

        [Column("userId")]
        public int UserId { get; set; }

    }
}
