namespace Service;

public class Delayer(TimeSpan loopInterval)
{
    private readonly Random _random = new();

    private async Task ThrottleAsync() => await Task.Delay(loopInterval);

    private async Task<T> CallAsync<T>(Func<Task<T>> func, Task throttle)
    {
        Console.WriteLine("Cycle:start() {0:T}", DateTime.Now);
        var seconds = (int)loopInterval.TotalSeconds;
        var randomInt = _random.Next(0, seconds - 1);
        var randomTimeSpan = TimeSpan.FromSeconds(randomInt);

        await Task.Delay(randomTimeSpan);

        var ret = await func();
        await throttle;
        Console.WriteLine("Cycle:end() {0:T}", DateTime.Now);
        return ret;
    }

    public async Task<T> RunCycle<T>(Func<Task<T>> func)
    {
        var throttle = Task.Run(ThrottleAsync);
        return await CallAsync(func, throttle);
    }
}
