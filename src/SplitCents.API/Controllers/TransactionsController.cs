namespace SplitCents.API.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitCents.API.DTOs;
using SplitCents.Core.Interfaces.Services;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("recurring")]
    public async Task<IActionResult> CreateRecurring([FromBody] RecurringTransactionRequest request)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var transaction = await _transactionService.CreateRecurringAsync(
            userId,
            request.Description,
            request.Amount,
            request.Frequency,
            request.NextTransactionDate,
            request.CategoryId,
            request.Notes,
            request.StartDate,
            request.EndDate,
            request.Provider);

        return CreatedAtAction(nameof(GetRecurringById), new { transactionId = transaction.id }, transaction);
    }

    [HttpGet("recurring")]
    public async Task<IActionResult> GetRecurring()
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        return Ok(await _transactionService.GetRecurringAsync(userId));
    }

    [HttpGet("recurring/{transactionId:guid}")]
    public async Task<IActionResult> GetRecurringById(Guid transactionId)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        return Ok(await _transactionService.GetRecurringByIdAsync(userId, transactionId));
    }

    [HttpPut("recurring/{transactionId:guid}")]
    public async Task<IActionResult> UpdateRecurring(
        Guid transactionId,
        [FromBody] RecurringTransactionRequest request)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        await _transactionService.UpdateRecurringAsync(
            userId,
            transactionId,
            request.Description,
            request.Amount,
            request.Frequency,
            request.NextTransactionDate,
            request.CategoryId,
            request.Notes,
            request.StartDate,
            request.EndDate,
            request.Provider);

        return NoContent();
    }

    [HttpDelete("recurring/{transactionId:guid}")]
    public async Task<IActionResult> DeleteRecurring(Guid transactionId)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        await _transactionService.DeleteRecurringAsync(userId, transactionId);
        return NoContent();
    }

    [HttpPost("recurring/{transactionId:guid}/paid")]
    public async Task<IActionResult> MarkRecurringPaid(
        Guid transactionId,
        [FromBody] MarkRecurringPaidRequest? request = null)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        await _transactionService.MarkRecurringPaidAsync(userId, transactionId, request?.PaidOn);
        return NoContent();
    }

    [HttpDelete("recurring/{transactionId:guid}/paid")]
    public async Task<IActionResult> MarkRecurringUnpaid(Guid transactionId)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        await _transactionService.MarkRecurringUnpaidAsync(userId, transactionId);
        return NoContent();
    }

    [HttpGet("recurring/upcoming")]
    public async Task<IActionResult> FindUpcomingRecurring(
        [FromQuery] DateTime? from = null,
        [FromQuery] int daysAhead = 30)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        return Ok(await _transactionService.FindUpcomingRecurringAsync(
            userId,
            from ?? DateTime.UtcNow,
            daysAhead));
    }

    private bool TryGetUserId(out Guid userId)
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(value, out userId);
    }
}