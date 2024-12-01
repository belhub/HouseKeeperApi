using HouseKeeperApi.Models;
using HouseKeeperApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseKeeperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // walidacja
    [Authorize(Roles = "Tenant, Landlord")] //autoryzacja
    public class TransactionController : ControllerBase
    {
        private readonly ITrasnsactionService _transactionService;

        public TransactionController(ITrasnsactionService trasnsactionService)
        {
            _transactionService = trasnsactionService;
        }

        [HttpGet("byUserId/{userId}")]
        public async Task<ActionResult<List<TransactionDto>>> GetTransactionsForUser([FromRoute] int userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var transactions = await _transactionService.GetAllTtransactionsFormUserId(userId, startDate, endDate);
            return transactions == null ? NotFound() : Ok(transactions);
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> CreateTransaction([FromBody] TransactionDto transactionDto, [FromRoute] int userId)
        {
            var transaction = await _transactionService.CreateTransaction(transactionDto, userId);
            return Created($"Pomyslnie utworzono transakcje o id: {transaction}", null);
        }

        [HttpPut("{transactionId}")]
        public async Task<ActionResult> UpdateTransaction([FromRoute] int transactionId, [FromBody] TransactionDto transactionDto)
        {
            var transaction = await _transactionService.UpdateTransaction(transactionId, transactionDto);
            return transaction ? Ok(transaction) : NotFound();
        }

        [HttpDelete("{transactionId}")]
        public async Task<ActionResult> DeleteRoom([FromRoute] int transactionId)
        {
            var transaction = await _transactionService.DeleteTransaction(transactionId);
            return transaction ? NoContent() : NotFound();
        }
    }
}
