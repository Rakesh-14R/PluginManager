#region Fire and forget
//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;

//public class PluginManager
//{
//    private readonly int _maxConcurrentExecutions;
//    private readonly SemaphoreSlim _semaphore;

//    public PluginManager(int maxConcurrentExecutions)
//    {
//        _maxConcurrentExecutions = maxConcurrentExecutions;
//        _semaphore = new SemaphoreSlim(_maxConcurrentExecutions);
//    }

//    public void InvokePlugins(IEnumerable<string> exePaths)
//    {
//        foreach (var exePath in exePaths)
//        {
//            Task.Run(async () =>
//            {
//                Console.WriteLine($"--->->--->------>-->>Created PluginManager: Task Created Thread ID :  {Thread.CurrentThread.ManagedThreadId}");
//                await _semaphore.WaitAsync(); // Wait for an available slot
//                try
//                {
//                    Console.WriteLine($"PluginManager: Invoking {exePath}");
//                    await PluginInvoker.InvokePluginAsync(exePath); // Delegate to PluginInvoker
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"PluginManager: Error invoking {exePath} - {ex.Message}");
//                }
//                finally
//                {
//                    _semaphore.Release(); // Release semaphore when done
//                    Console.WriteLine($"---<-<---<------<--<<Released PluginManager: Task Released Thread ID {Thread.CurrentThread.ManagedThreadId} completed");
//                }
//            });
//        }
//    }
//}

#endregion

#region when all
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class PluginManager
{
    private readonly int _maxConcurrentExecutions;
    private readonly SemaphoreSlim _semaphore;

    public PluginManager(int maxConcurrentExecutions)
    {
        _maxConcurrentExecutions = maxConcurrentExecutions;
        _semaphore = new SemaphoreSlim(_maxConcurrentExecutions);
    }

    public async Task InvokePluginsAsync(IEnumerable<string> exePaths)
    {
        if (exePaths == null || !exePaths.GetEnumerator().MoveNext())
        {
            Console.WriteLine($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): No valid executable paths provided.");
            return; // Exit if no valid paths are given
        }

        var tasks = new List<Task>();
        var taskDetails = new Dictionary<Task, string>(); // Track each task and its associated exe path

        foreach (var exePath in exePaths)
        {
            if (!IsValidExecutable(exePath))
            {
                Console.WriteLine($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): Invalid or inaccessible path - {exePath}");
                continue; // Skip invalid paths
            }

            var task = InvokePluginAsync(exePath);
            tasks.Add(task);
            taskDetails[task] = exePath;
        }

        try
        {
            // Wait for all tasks to complete
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            // Handle unexpected exceptions thrown by Task.WhenAll()
            Console.WriteLine($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): An error occurred while waiting for tasks - {ex.Message}");
        }
        finally
        {
            // Log a summary after all tasks complete
            Console.WriteLine($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): All plugin invocations have completed.");
        }

        // Log results for each task
        foreach (var task in tasks)
        {
            if (task.IsFaulted)
            {
                Console.WriteLine($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): Task for {taskDetails[task]} failed with error - {task.Exception?.InnerException?.Message}");
            }
            else if (task.IsCompleted)
            {
                Console.WriteLine($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): Task for {taskDetails[task]} completed successfully.");
            }
            else
            {
                Console.WriteLine($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): Task for {taskDetails[task]} is still running.");
            }
        }
    }

    private async Task InvokePluginAsync(string exePath)
    {
        await _semaphore.WaitAsync(); // Wait for an available slot
        try
        {
            Console.WriteLine($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): Invoking {exePath}");
            Log($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): Invoking {exePath}");

            await PluginInvoker.InvokePluginAsync(exePath); // Call PluginInvoker asynchronously
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): Error invoking {exePath} - {ex.Message}");
            Log($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): Error invoking {exePath} - {ex.Message}");
        }
        finally
        {
            _semaphore.Release(); // Release the semaphore when done
            Console.WriteLine($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): Task for {exePath} completed.");
            Log($"PluginManager (Thread ID {Thread.CurrentThread.ManagedThreadId}): Task for {exePath} completed.");
        }
    }

    private bool IsValidExecutable(string exePath)
    {
        // Check if the path exists and points to an executable file
        return File.Exists(exePath) && (Path.GetExtension(exePath).Equals(".exe", StringComparison.OrdinalIgnoreCase));
    }

    private void Log(string message)
    {
        // Log message to a file
        using (var writer = new StreamWriter("PluginManagerLog.txt", true))
        {
            writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
    }
}
#endregion