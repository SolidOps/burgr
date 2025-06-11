using System.Diagnostics;

namespace SolidOps.UM.Shared.Tests;

public static class WaitHelper
{
    public static async Task WaitUntilAsync(Func<bool> testFunction, int timeoutInSeconds = 30, int sleepInMilliseconds = 1000)
    {
        var start = Stopwatch.StartNew();
        while (!testFunction())
        {
            await Task.Delay(sleepInMilliseconds);
            if (start.ElapsedMilliseconds > timeoutInSeconds * 1000)
            {
                throw new TimeoutException($"Function lasted more than {timeoutInSeconds} seconds");
            }
        }
    }

    public static async Task WaitUntilAsync2(Func<Task<bool>> testFunction, int timeoutInSeconds = 30, int sleepInMilliseconds = 1000)
    {
        var start = Stopwatch.StartNew();
        while (!await testFunction())
        {
            await Task.Delay(sleepInMilliseconds);
            if (start.ElapsedMilliseconds > timeoutInSeconds * 1000)
            {
                throw new TimeoutException($"Function lasted more than {timeoutInSeconds} seconds");
            }
        }
    }

    public static void WaitUntilSync(Func<bool> testFunction, int timeoutInSeconds = 30, int sleepInMilliseconds = 1000)
    {
        var start = Stopwatch.StartNew();
        while (!testFunction())
        {
            Thread.Sleep(sleepInMilliseconds);
            if (start.ElapsedMilliseconds > timeoutInSeconds * 1000)
            {
                throw new TimeoutException($"Function lasted more than {timeoutInSeconds} seconds");
            }
        }
    }
}
