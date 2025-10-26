using Autofac;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using SEP_Backend.ETask;
using Xunit;

namespace Tests.ETask;

public class TaskServiceTests
{
    private static readonly IContainer Container = TestContainerBuilder.Build(builder =>
    {
        builder.RegisterInstance(A.Fake<TaskBatchValidator>());
        builder.RegisterInstance(A.Fake<ITaskRepository>());
    });
    private readonly TaskBatchValidator _validator = Container.Resolve<TaskBatchValidator>();
    private readonly ITaskRepository _repository = Container.Resolve<ITaskRepository>();
    private readonly TaskService _service = Container.Resolve<TaskService>();

    [Fact]
    public async Task CreateBatchAsync_ValidatorReturnsError_Throws()
    {
        SEP_Backend.ETask.ETask[] tasks = [];
        const string error = "SomeError";
        A.CallTo(() => _validator.IsValid(tasks)).Returns(error);

        var act = () => _service.CreateBatchAsync(tasks);

        await act.Should().ThrowAsync<BadHttpRequestException>().WithMessage(error);
    }

    [Fact]
    public async Task CreateBatchAsync_ValidatorReturnsTrue_CallsRepository()
    {
        SEP_Backend.ETask.ETask[] tasks = [];
        A.CallTo(() => _validator.IsValid(tasks)).Returns(true);

        await _service.CreateBatchAsync(tasks);

        A.CallTo(() => _repository.CreateBatchAsync(tasks)).MustHaveHappenedOnceExactly();
    }
}