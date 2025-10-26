using Autofac;
using FakeItEasy;
using FluentAssertions;
using SEP_Backend.ETask;
using SEP_Backend.Event;
using SEP_Backend.User;
using TimeProvider = SEP_Backend.Common.TimeProvider;
using Xunit;

namespace Tests.ETask;

public class TaskBatchValidatorTests
{
    private static readonly IContainer Container = TestContainerBuilder.Build(builder =>
    {
        builder.RegisterInstance(A.Fake<IUserRepository>());
        builder.RegisterInstance(A.Fake<IEventRepository>());
        builder.RegisterInstance(A.Fake<TimeProvider>());
    });
    private readonly IUserRepository _userRepository = Container.Resolve<IUserRepository>();
    private readonly IEventRepository _eventRepository = Container.Resolve<IEventRepository>();
    private readonly TimeProvider _timeProvider = Container.Resolve<TimeProvider>();
    private readonly TaskBatchValidator _validator = Container.Resolve<TaskBatchValidator>();

    [Fact]
    public async Task IsValid_EmptyTaskArray_ReturnsError()
    {
        SEP_Backend.ETask.ETask[] tasks = [];

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeFalse();
        result.Error.Should().Be("No tasks found");
    }

    [Fact]
    public async Task IsValid_TaskWithNonExistentUser_ReturnsError()
    {
        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var futureDate = DateTime.Now.AddDays(1);
        var tasks = new[]
        {
            new SEP_Backend.ETask.ETask
            {
                UserId = userId,
                EventId = eventId,
                Title = "Test Task",
                DueDate = futureDate
            }
        };
        A.CallTo(() => _userRepository.GetByIdAsync(userId)).Returns<User?>(null);
        A.CallTo(() => _timeProvider.Now()).Returns(DateTime.Now);

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeFalse();
        result.Error.Should().Be("User not found");
    }

    [Fact]
    public async Task IsValid_TaskWithEmptyUserId_SkipsUserValidation()
    {
        var eventId = Guid.NewGuid();
        var futureDate = DateTime.Now.AddDays(1);
        var tasks = new[]
        {
            new SEP_Backend.ETask.ETask
            {
                UserId = Guid.Empty,
                EventId = eventId,
                Title = "Test Task",
                DueDate = futureDate
            }
        };
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId))
            .Returns(new SEP_Backend.Event.Event { Id = eventId });
        A.CallTo(() => _timeProvider.Now()).Returns(DateTime.Now);

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task IsValid_TasksWithDifferentEventIds_ReturnsError()
    {
        var eventId1 = Guid.NewGuid();
        var eventId2 = Guid.NewGuid();
        var futureDate = DateTime.Now.AddDays(1);
        var tasks = new[]
        {
            new SEP_Backend.ETask.ETask
            {
                UserId = Guid.Empty,
                EventId = eventId1,
                Title = "Test Task 1",
                DueDate = futureDate
            },
            new SEP_Backend.ETask.ETask
            {
                UserId = Guid.Empty,
                EventId = eventId2,
                Title = "Test Task 2",
                DueDate = futureDate
            }
        };
        A.CallTo(() => _timeProvider.Now()).Returns(DateTime.Now);

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeFalse();
        result.Error.Should().Be("All tasks must have the same event id");
    }

    [Fact]
    public async Task IsValid_TaskWithNonExistentEvent_ReturnsError()
    {
        var eventId = Guid.NewGuid();
        var futureDate = DateTime.Now.AddDays(1);
        var tasks = new[]
        {
            new SEP_Backend.ETask.ETask
            {
                UserId = Guid.Empty,
                EventId = eventId,
                Title = "Test Task",
                DueDate = futureDate
            }
        };
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId)).Returns<SEP_Backend.Event.Event?>(null);
        A.CallTo(() => _timeProvider.Now()).Returns(DateTime.Now);

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeFalse();
        result.Error.Should().Be("Event not found");
    }

    [Fact]
    public async Task IsValid_TaskWithEmptyTitle_ReturnsError()
    {
        var eventId = Guid.NewGuid();
        var futureDate = DateTime.Now.AddDays(1);
        var tasks = new[]
        {
            new SEP_Backend.ETask.ETask
            {
                UserId = Guid.Empty,
                EventId = eventId,
                Title = "",
                DueDate = futureDate
            }
        };
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId))
            .Returns(new SEP_Backend.Event.Event { Id = eventId });
        A.CallTo(() => _timeProvider.Now()).Returns(DateTime.Now);

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeFalse();
        result.Error.Should().Be("Task title cannot be empty");
    }

    [Fact]
    public async Task IsValid_TaskWithNullTitle_ReturnsError()
    {
        var eventId = Guid.NewGuid();
        var futureDate = DateTime.Now.AddDays(1);

        var tasks = new[]
        {
            new SEP_Backend.ETask.ETask
            {
                UserId = Guid.Empty,
                EventId = eventId,
                Title = null!,
                DueDate = futureDate
            }
        };
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId))
            .Returns(new SEP_Backend.Event.Event { Id = eventId });
        A.CallTo(() => _timeProvider.Now()).Returns(DateTime.Now);

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeFalse();
        result.Error.Should().Be("Task title cannot be empty");
    }

    [Fact]
    public async Task IsValid_TaskWithPastDueDate_ReturnsError()
    {
        var eventId = Guid.NewGuid();
        var now = DateTime.Now;
        var pastDate = now.AddDays(-1);
        var tasks = new[]
        {
            new SEP_Backend.ETask.ETask
            {
                UserId = Guid.Empty,
                EventId = eventId,
                Title = "Test Task",
                DueDate = pastDate
            }
        };
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId))
            .Returns(new SEP_Backend.Event.Event { Id = eventId });
        A.CallTo(() => _timeProvider.Now()).Returns(now);

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeFalse();
        result.Error.Should().Be("Task due date cannot be in the past");
    }

    [Fact]
    public async Task IsValid_ValidTasksWithUsers_ReturnsTrue()
    {
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var futureDate = DateTime.Now.AddDays(1);
        var tasks = new[]
        {
            new SEP_Backend.ETask.ETask
            {
                UserId = userId1,
                EventId = eventId,
                Title = "Test Task 1",
                DueDate = futureDate
            },
            new SEP_Backend.ETask.ETask
            {
                UserId = userId2,
                EventId = eventId,
                Title = "Test Task 2",
                DueDate = futureDate
            }
        };
        A.CallTo(() => _userRepository.GetByIdAsync(userId1)).Returns(new User { Id = userId1 });
        A.CallTo(() => _userRepository.GetByIdAsync(userId2)).Returns(new User { Id = userId2 });
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId))
            .Returns(new SEP_Backend.Event.Event { Id = eventId });
        A.CallTo(() => _timeProvider.Now()).Returns(DateTime.Now);

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task IsValid_ValidTasksWithoutUsers_ReturnsTrue()
    {
        var eventId = Guid.NewGuid();
        var futureDate = DateTime.Now.AddDays(1);
        var tasks = new[]
        {
            new SEP_Backend.ETask.ETask
            {
                UserId = Guid.Empty,
                EventId = eventId,
                Title = "Test Task 1",
                DueDate = futureDate
            },
            new SEP_Backend.ETask.ETask
            {
                UserId = Guid.Empty,
                EventId = eventId,
                Title = "Test Task 2",
                DueDate = futureDate
            }
        };
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId))
            .Returns(new SEP_Backend.Event.Event { Id = eventId });
        A.CallTo(() => _timeProvider.Now()).Returns(DateTime.Now);

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task IsValid_MixedUserAssignment_ReturnsTrue()
    {
        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var futureDate = DateTime.Now.AddDays(1);
        var tasks = new[]
        {
            new SEP_Backend.ETask.ETask
            {
                UserId = userId,
                EventId = eventId,
                Title = "Assigned Task",
                DueDate = futureDate
            },
            new SEP_Backend.ETask.ETask
            {
                UserId = Guid.Empty,
                EventId = eventId,
                Title = "Unassigned Task",
                DueDate = futureDate
            }
        };
        A.CallTo(() => _userRepository.GetByIdAsync(userId)).Returns(new User { Id = userId });
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId))
            .Returns(new SEP_Backend.Event.Event { Id = eventId });
        A.CallTo(() => _timeProvider.Now()).Returns(DateTime.Now);

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task IsValid_SingleValidTask_ReturnsTrue()
    {
        var eventId = Guid.NewGuid();
        var futureDate = DateTime.Now.AddDays(1);
        var tasks = new[]
        {
            new SEP_Backend.ETask.ETask
            {
                UserId = Guid.Empty,
                EventId = eventId,
                Title = "Single Task",
                DueDate = futureDate
            }
        };
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId))
            .Returns(new SEP_Backend.Event.Event { Id = eventId });
        A.CallTo(() => _timeProvider.Now()).Returns(DateTime.Now);

        var result = await _validator.IsValid(tasks);

        result.IsValid.Should().BeTrue();
        result.Error.Should().BeNull();
    }
}