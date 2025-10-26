namespace SEP_Backend.ETask;

public class TaskService(TaskBatchValidator validator, ITaskRepository repository)
{
    public async Task CreateBatchAsync(params ETask[] tasks)
    {
        var validation = await validator.IsValid(tasks);
        if (!validation.IsValid)
            throw new BadHttpRequestException(validation.Error ?? "");
        await repository.CreateBatchAsync(tasks);
    }
}