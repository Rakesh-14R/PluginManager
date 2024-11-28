#region fire and forget
//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Threading.Tasks;

//public static class PluginInvoker
//{
//    public static async Task InvokePluginAsync(string exePath)
//    {
//        if (!File.Exists(exePath))
//        {
//            Console.WriteLine($"PluginInvoker: File not found - {exePath}");
//            return;
//        }

//        try
//        {
//            var process = new Process
//            {
//                StartInfo = new ProcessStartInfo
//                {
//                    FileName = exePath,
//                    Arguments = "", // Add arguments if needed
//                    UseShellExecute = false,
//                    RedirectStandardOutput = true,
//                    RedirectStandardError = true,
//                    CreateNoWindow = true
//                }
//            };

//            process.OutputDataReceived += (sender, args) =>
//            {
//                if (!string.IsNullOrWhiteSpace(args.Data))
//                {
//                    Console.WriteLine($"[OUTPUT] {args.Data}");
//                }
//            };

//            process.ErrorDataReceived += (sender, args) =>
//            {
//                if (!string.IsNullOrWhiteSpace(args.Data))
//                {
//                    Console.WriteLine($"[ERROR] {args.Data}");
//                }
//            };

//            process.Start(); // Start the process
//            process.BeginOutputReadLine();
//            process.BeginErrorReadLine();

//            Console.WriteLine($"PluginInvoker: Running {exePath}");
//            await process.WaitForExitAsync(); // Wait asynchronously for the process to exit

//            Console.WriteLine($"PluginInvoker: {exePath} completed with exit code {process.ExitCode}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"PluginInvoker: Error executing {exePath} - {ex.Message}");
//        }
//    }
//}
#endregion


using System;
using System.Diagnostics;
using System.Threading.Tasks;

public static class PluginInvoker
{
    public static async Task InvokePluginAsync(string exePath)
    {
        try
        {
            // Create a process to run the executable
            var processStartInfo = new ProcessStartInfo
            {
                FileName = exePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.OutputDataReceived += (sender, e) => Console.WriteLine($"[Output]: {e.Data}");
                process.ErrorDataReceived += (sender, e) => Console.WriteLine($"[Error]: {e.Data}");

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Wait for the process to complete asynchronously
                await Task.Run(() => process.WaitForExit());

                Console.WriteLine($"PluginInvoker: {exePath} completed with exit code {process.ExitCode}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PluginInvoker: An error occurred while invoking {exePath} - {ex.Message}");
        }
    }
}
