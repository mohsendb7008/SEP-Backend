using Autofac;
using FluentAssertions;
using SEP_Backend.Event;
using Xunit;

namespace Tests.Event;

public class EventRequestValidatorTests
{
    private static readonly IContainer Container = TestContainerBuilder.Build();
    private readonly EventRequestValidator _requestValidator = Container.Resolve<EventRequestValidator>();

    [Fact]
    public void IsValid_WithValidRequest_ReturnsTrue()
    {
        var request = CreateValidRequest();

        var isValid = _requestValidator.IsValid(request, out var error);

        isValid.Should().BeTrue();
        error.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t ")]
    [InlineData("\n")]
    [InlineData("\t     \r")]
    [InlineData("\r\n\r\t")]
    public void IsValid_WithInvalidTitle_ExpectError(string title)
    {
        var request = CreateValidRequest();
        request.Title = title;

        var isValid = _requestValidator.IsValid(request, out var error);

        isValid.Should().BeFalse();
        error.Should().Contain("Title");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t ")]
    [InlineData("\n")]
    [InlineData("\t     \r")]
    [InlineData("\r\n\r\t")]
    public void IsValid_WithInvalidDescription_ExpectError(string description)
    {
        var request = CreateValidRequest();
        request.Description = description;

        var isValid = _requestValidator.IsValid(request, out var error);

        isValid.Should().BeFalse();
        error.Should().Contain("Description");
    }

    [Theory]
    [InlineData(null)]
    [InlineData(-100)]
    [InlineData(-1)]
    [InlineData(-0.5)]
    [InlineData(0)]
    [InlineData(0.25)]
    [InlineData(0.75)]
    [InlineData(0.999)]
    public void IsValid_WithInvalidBudget_ExpectError(decimal budget)
    {
        var request = CreateValidRequest();
        request.Budget = budget;

        var isValid = _requestValidator.IsValid(request, out var error);

        isValid.Should().BeFalse();
        error.Should().Contain("Budget");
    }

    private EventRequest CreateValidRequest() => new()
    {
        Title = "SomeTitle",
        Description = "SomeDescription",
        Budget = 100
    };
}