namespace SplitCents.Core.Tests;

using FluentAssertions;
using Moq;
using SplitCents.Core.Exceptions;
using SplitCents.Core.Interfaces.Repositories;
using SplitCents.Core.Models;
using SplitCents.Core.Services;
using Xunit;

public class TransactionServicesTests
{
    private readonly Mock<ITransactionRepository> _repository = new();
    private readonly TransactionService _sut;

    public TransactionServicesTests()
    {
        _sut = new TransactionService(_repository.Object);
    }

    [Fact]
    public async Task CreateRecurringAsync_CreatesExpenseOwnedByUser()
    {
        var userId = Guid.NewGuid();
        var dueDate = new DateTime(2026, 10, 1);

        var result = await _sut.CreateRecurringAsync(
            userId, "Rent", 1000m, TransactionFrequency.Monthly, dueDate);

        result.userId.Should().Be(userId);
        result.type.Should().Be(TransactionType.Expense);
        result.nextTransactionDate.Should().Be(dueDate);
        _repository.Verify(r => r.AddRecurringAsync(result), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CreateRecurringAsync_WithNonPositiveAmount_Throws(decimal amount)
    {
        var act = () => _sut.CreateRecurringAsync(
            Guid.NewGuid(), "Rent", amount, TransactionFrequency.Monthly, DateTime.UtcNow);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Transaction amount must be greater than zero.");
    }

    [Fact]
    public async Task GetRecurringByIdAsync_WhenTransactionBelongsToAnotherUser_ThrowsNotFound()
    {
        var transactionId = Guid.NewGuid();
        _repository
            .Setup(r => r.GetRecurringByIdAsync(It.IsAny<Guid>(), transactionId))
            .ReturnsAsync((RecurringTransaction?)null);

        var act = () => _sut.GetRecurringByIdAsync(Guid.NewGuid(), transactionId);

        await act.Should().ThrowAsync<NotFoundException>();
        _repository.Verify(r => r.GetRecurringByIdAsync(It.IsAny<Guid>(), transactionId), Times.Once);
    }

    [Fact]
    public async Task MarkRecurringPaidAsync_UpdatesOnlyOwnedTransaction()
    {
        var userId = Guid.NewGuid();
        var transaction = MakeRecurring(userId);
        _repository
            .Setup(r => r.GetRecurringByIdAsync(userId, transaction.id))
            .ReturnsAsync(transaction);

        var paidOn = new DateTime(2026, 10, 2);
        await _sut.MarkRecurringPaidAsync(userId, transaction.id, paidOn);

        transaction.isPaid.Should().BeTrue();
        transaction.paidOn.Should().Be(paidOn);
        _repository.Verify(r => r.UpdateRecurringAsync(transaction), Times.Once);
    }

    [Fact]
    public async Task MarkRecurringUnpaidAsync_ClearsPaymentState()
    {
        var userId = Guid.NewGuid();
        var transaction = MakeRecurring(userId);
        transaction.isPaid = true;
        transaction.paidOn = DateTime.UtcNow;
        _repository
            .Setup(r => r.GetRecurringByIdAsync(userId, transaction.id))
            .ReturnsAsync(transaction);

        await _sut.MarkRecurringUnpaidAsync(userId, transaction.id);

        transaction.isPaid.Should().BeFalse();
        transaction.paidOn.Should().BeNull();
        _repository.Verify(r => r.UpdateRecurringAsync(transaction), Times.Once);
    }

    [Fact]
    public async Task FindUpcomingRecurringAsync_WithNegativeRange_ThrowsValidation()
    {
        var act = () => _sut.FindUpcomingRecurringAsync(Guid.NewGuid(), DateTime.UtcNow, -1);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Upcoming transaction days cannot be negative.");
        _repository.Verify(r => r.GetUpcomingRecurringAsync(
            It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
    }

    private static RecurringTransaction MakeRecurring(Guid userId) => new()
    {
        id = Guid.NewGuid(),
        userId = userId,
        description = "Rent",
        amount = 1000m,
        type = TransactionType.Expense,
        frequency = TransactionFrequency.Monthly,
        nextTransactionDate = new DateTime(2026, 10, 1),
        status = SubscriptionStatus.Active
    };
}