using MealManagement.Application.DTOs;
using MealManagement.Application.Features.Meals.Commands;
using MealManagement.Application.Features.Meals.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MealManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MealsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MealsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("toggle")]
        public async Task<ActionResult<bool>> ToggleMealStatus(DateTime date)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var command = new ToggleMealStatusCommand
            {
                UserId = userId,
                Date = date
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("today")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<MealDto>>> GetTodayMeals()
        {
            var query = new GetTodayMealsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("today/by-address")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Dictionary<string, List<MealDto>>>> GetTodayMealsByAddress()
        {
            var query = new GetTodayMealsByAddressQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<ActionResult<List<MealDto>>> GetUserMealHistory([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var query = new GetUserMealHistoryQuery
            {
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<MealDto>>> GetUserMealHistory(Guid userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var query = new GetUserMealHistoryQuery
            {
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}