using Autofac;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using SEP_Backend.Budget;
using SEP_Backend.Event;
using Xunit;

namespace Tests.Budget;

public class BudgetServiceTests
{
    private static readonly IContainer Container = TestContainerBuilder.Build(builder =>
    {
        builder.RegisterInstance(A.Fake<IBudgetRepository>());
        builder.RegisterInstance(A.Fake<IEventRepository>());
    });
    private readonly IBudgetRepository _budgetRepository = Container.Resolve<IBudgetRepository>();
    private readonly IEventRepository _eventRepository = Container.Resolve<IEventRepository>();
    private readonly BudgetService _service = Container.Resolve<BudgetService>();

    [Fact]
    public async Task ProposeBudget_ValidEventAndAmount_CreatesBudget()
    {
        var eventId = Guid.NewGuid();
        const decimal amount = 1000m;
        var existingEvent = new SEP_Backend.Event.Event { Id = eventId };
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId)).Returns(existingEvent);

        await _service.ProposeBudget(eventId, amount);

        A.CallTo(() => _budgetRepository.CreateAsync(A<SEP_Backend.Budget.Budget>.That
            .Matches(b => b.EventId == eventId && b.ProposedAmount == amount))).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ProposeBudget_EventNotFound_ThrowsBadHttpRequestException()
    {
        var eventId = Guid.NewGuid();
        const decimal amount = 1000m;
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId)).Returns<SEP_Backend.Event.Event?>(null);

        var act = () => _service.ProposeBudget(eventId, amount);

        await act.Should().ThrowAsync<BadHttpRequestException>().WithMessage("Event not found");
    }

    [Fact]
    public async Task ProposeBudget_AmountTooSmall_ThrowsBadHttpRequestException()
    {
        var eventId = Guid.NewGuid();
        const decimal amount = 0.5m;
        var existingEvent = new SEP_Backend.Event.Event { Id = eventId };
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId)).Returns(existingEvent);

        var act = () => _service.ProposeBudget(eventId, amount);

        await act.Should().ThrowAsync<BadHttpRequestException>().WithMessage("Amount is too small");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    [InlineData(-0.01)]
    public async Task ProposeBudget_InvalidAmounts_ThrowsBadHttpRequestException(decimal invalidAmount)
    {
        var eventId = Guid.NewGuid();
        var existingEvent = new SEP_Backend.Event.Event { Id = eventId };
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId)).Returns(existingEvent);

        var act = () => _service.ProposeBudget(eventId, invalidAmount);

        await act.Should().ThrowAsync<BadHttpRequestException>().WithMessage("Amount is too small");
    }

    [Fact]
    public async Task UpdateBudget_ValidBudgetAndAmount_UpdatesBudget()
    {
        var budgetId = Guid.NewGuid();
        const decimal amount = 1500m;
        A.CallTo(() => _budgetRepository.UpdateAsync(A<SEP_Backend.Budget.Budget>._)).Returns(true);

        await _service.UpdateBudget(budgetId, amount);

        A.CallTo(() => _budgetRepository.UpdateAsync(A<SEP_Backend.Budget.Budget>.That
                .Matches(b => b.Id == budgetId && b.NegotiatedAmount == amount))).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateBudget_BudgetNotFound_ThrowsBadHttpRequestException()
    {
        var budgetId = Guid.NewGuid();
        const decimal amount = 1500m;
        A.CallTo(() => _budgetRepository.UpdateAsync(A<SEP_Backend.Budget.Budget>._)).Returns(false);

        var act = () => _service.UpdateBudget(budgetId, amount);

        await act.Should().ThrowAsync<BadHttpRequestException>().WithMessage("Budget not found");
    }

    [Fact]
    public async Task UpdateBudget_AmountTooSmall_ThrowsBadHttpRequestException()
    {
        var budgetId = Guid.NewGuid();
        const decimal amount = 0.5m;

        var act = () => _service.UpdateBudget(budgetId, amount);

        await act.Should().ThrowAsync<BadHttpRequestException>().WithMessage("Amount is too small");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    [InlineData(-0.99)]
    public async Task UpdateBudget_InvalidAmounts_ThrowsBadHttpRequestException(decimal invalidAmount)
    {
        var budgetId = Guid.NewGuid();

        var act = () => _service.UpdateBudget(budgetId, invalidAmount);

        await act.Should().ThrowAsync<BadHttpRequestException>().WithMessage("Amount is too small");
    }
}