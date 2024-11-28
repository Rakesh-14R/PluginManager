#region Fire and forget
//using System;
//using System.Collections.Generic;

//class Program
//{
//    static void Main(string[] args)
//    {
//        // Initialize PluginManager with a limit of 4 concurrent executions
//        var pluginManager = new PluginManager(maxConcurrentExecutions: 4);

//        // List of exe paths (adjust these paths to your actual exe files)
//        var exePaths = new List<string>
//        {
//            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
//            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
//            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
//            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
//            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
//            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
//            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
//            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
//            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
//            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe"
//        };

//        Console.WriteLine("Starting PluginManager...");
//        //pluginManager.InvokePlugins(exePaths);



//        Console.WriteLine("PluginManager is processing plugins. Main thread is free.");
//        Console.ReadLine(); // Prevent the main program from exiting immediately
//    }
//}
#endregion


using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // List of paths to the executables you want to run
        var exePaths = new List<string>
        {
            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe",
            "C:\\Users\\user\\Desktop\\Projects\\ExeFile\\bin\\Release\\net6.0\\ExeFile.exe"
            // Add more paths as needed
        };

        // Create a PluginManager instance that allows up to 5 concurrent executions
        var pluginManager = new PluginManager(5);

        // Start invoking the plugins
        Console.WriteLine("Starting PluginManager...");
        await pluginManager.InvokePluginsAsync(exePaths);

        Console.WriteLine("All plugins have been processed.");
    }
}
