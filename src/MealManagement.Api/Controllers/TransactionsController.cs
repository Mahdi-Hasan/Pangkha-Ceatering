using MealManagement.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MealManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<TransactionDto>> AddDeposit(AddDepositDto depositDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var command = new AddDepositCommand
            {
                UserId = userId,
                Amount = depositDto.Amount,
                PaymentMethod = depositDto.PaymentMethod,
                PaymentReference = depositDto.PaymentReference
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("adjustment")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TransactionDto>> AdjustBalance(BalanceAdjustmentDto adjustmentDto)
        {
            var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var command = new AdjustBalanceCommand
            {
                UserId = adjustmentDto.UserId,
                Amount = adjustmentDto.Amount,
                AdjustedById = adminId,
                Notes = adjustmentDto.Notes
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<TransactionDto>>> GetAllTransactions([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetAllTransactionsQuery
            {
                StartDate = startDate ?? DateTime.MinValue,
                EndDate = endDate ?? DateTime.MaxValue
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<TransactionDto>>> GetUserTransactions(Guid userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetUserTransactionsQuery
            {
                UserId = userId,
                StartDate = startDate ?? DateTime.MinValue,
                EndDate = endDate ?? DateTime.MaxValue
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("my-transactions")]
        public async Task<ActionResult<List<TransactionDto>>> GetMyTransactions([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var query = new GetUserTransactionsQuery
            {
                UserId = userId,
                StartDate = startDate ?? DateTime.MinValue,
                EndDate = endDate ?? DateTime.MaxValue
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}