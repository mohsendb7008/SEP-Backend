namespace SEP_Backend.ETask;

public class TaskService(TaskBatchValidator validator, ITaskRepository repository)
{
    public async Task CreateBatchAsync(params ETask[] tasks)
    {
        var validation = await validator.IsValid(tasks);
        if (!validation.IsValid)
            throw new BadHttpRequestException(validation.Error ?? string.Empty);
        await repository.CreateBatchAsync(tasks);
    }

    public async Task<bool> UpdateAsync(ETask task)
    {
        var validation = await validator.IsValid(task);
        if (!validation.IsValid)
            throw new BadHttpRequestException(validation.Error ?? string.Empty);
        return await repository.UpdateAsync(task);
    }
}