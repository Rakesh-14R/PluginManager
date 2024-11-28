//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//class Program
//{
//    static async Task Main(string[] args)
//    {
//        // List of paths to the executables you want to run
//        var exePaths = new List<string>
//        {
//            @"C:\Path\To\Executable1.exe",
//            @"C:\Path\To\Executable2.exe",
//            @"C:\Path\To\Executable3.exe"
//            // Add more paths as needed
//        };

//        // Create a PluginManager instance that allows up to 1 concurrent execution
//        var pluginManager = new PluginManager(1);

//        // Start invoking the plugins sequentially
//        Console.WriteLine("Starting PluginManager...");
//        await pluginManager.InvokePluginsSequentiallyAsync(exePaths);

//        Console.WriteLine("All plugins have been processed.");
//    }
//}

//using System;
//using System.Diagnostics;
//using System.Threading.Tasks;

//public static class PluginInvoker
//{
//    public static async Task InvokePluginAsync(string exePath)
//    {
//        try
//        {
//            // Create a process to run the executable
//            var processStartInfo = new ProcessStartInfo
//            {
//                FileName = exePath,
//                RedirectStandardOutput = true,
//                RedirectStandardError = true,
//                UseShellExecute = false,
//                CreateNoWindow = true
//            };

//            using (var process = new Process { StartInfo = processStartInfo })
//            {
//                process.OutputDataReceived += (sender, e) => Console.WriteLine($"[Output]: {e.Data}");
//                process.ErrorDataReceived += (sender, e) => Console.WriteLine($"[Error]: {e.Data}");

//                process.Start();
//                process.BeginOutputReadLine();
//                process.BeginErrorReadLine();

//                // Wait for the process to complete and exit
//                await Task.Run(() => process.WaitForExit());

//                Console.WriteLine($"PluginInvoker: {exePath} completed with exit code {process.ExitCode}.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"PluginInvoker: An error occurred while invoking {exePath} - {ex.Message}");
//        }
//    }
//}


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

//    public async Task InvokePluginsSequentiallyAsync(IEnumerable<string> exePaths)
//    {
//        foreach (var exePath in exePaths)
//        {
//            // Wait asynchronously for an available slot before starting the next plugin
//            await _semaphore.WaitAsync();

//            try
//            {
//                Console.WriteLine($"PluginManager: Invoking {exePath} on Thread ID {Thread.CurrentThread.ManagedThreadId}");

//                // Invoke the plugin and wait for it to complete before moving to the next one
//                await PluginInvoker.InvokePluginAsync(exePath);

//                Console.WriteLine($"PluginManager: {exePath} completed successfully.");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"PluginManager: Error invoking {exePath} - {ex.Message}");
//            }
//            finally
//            {
//                // Release the semaphore so the next plugin can start
//                _semaphore.Release();
//                Console.WriteLine($"PluginManager: Released Thread ID {Thread.CurrentThread.ManagedThreadId} for {exePath}");
//            }
//        }
//    }
//}
