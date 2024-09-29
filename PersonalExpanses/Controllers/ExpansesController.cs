using Manage_Personal_Expenses.Data;
using Manage_Personal_Expenses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Manage_Personal_Expenses.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
   
    public class ExpansesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public ExpansesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
    
        public async Task<IActionResult> Post_Expenses(ExpensesModel expenses)
        {
            var userIdClaim= this.User.Claims.FirstOrDefault(x=> x.Type == "UserId");
            if(userIdClaim == null) {  return Unauthorized("UserId claim not found"); }

            var userId = int.Parse(userIdClaim.Value);
            expenses.UserId = userId;

            await _appDbContext.ExpensesModels.AddAsync(expenses);
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]

        public async Task<IActionResult> Get_Expenses()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim == null) { return Unauthorized("UserId claim not found"); }
            var userId=int.Parse(userIdClaim.Value);

            var expensess = await _appDbContext.ExpensesModels.Where(x=> x.UserId== userId).ToListAsync();

            return Ok(expensess);
        }

        [HttpPut("id")]

        public async Task<IActionResult>Update_Expenses(int id, [FromBody]ExpensesModel expensesRequest)
        {
            if(id != expensesRequest.Id) { return BadRequest("Id-urile nu corespund"); }
            
            var expenses = await _appDbContext.ExpensesModels.FindAsync(id);

            if(expenses == null) { return NotFound("Nu a fost gasit"); }

            expenses.Description= expensesRequest.Description;
            expenses.Price = expensesRequest.Price;
            expenses.Category = expensesRequest.Category;

            await _appDbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("id")]
        public async Task<IActionResult>Delete_Expanses(int id)
        {
            var expanses = await _appDbContext.ExpensesModels.FindAsync(id);

            if (expanses == null) { return NotFound("Nu a fost gasit"); }

            _appDbContext.ExpensesModels.Remove(expanses);
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }

    }
}
