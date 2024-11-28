using System;
using System.IO;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Base file name
        string baseFileName = "Rakesh";
        string extension = ".txt";
        string filePath = GetUniqueFilePath(baseFileName, extension);

        int number = 1;

        // Set the end time to 3 minutes from now
        DateTime endTime = DateTime.Now.AddMinutes(2);

        Console.WriteLine($"Writing numbers to {filePath} for 3 minutes...");

        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {
            while (DateTime.Now < endTime)
            {
                // Write the number to the file
                writer.WriteLine(number);
                Console.WriteLine(number); // Optional: Print to console
                number++;

                // Delay for 1 second
                Thread.Sleep(1000);
            }
        }

        Console.WriteLine("Completed writing numbers to the file.");
    }

    // Method to generate a unique file path by incrementing the file name if it already exists
    static string GetUniqueFilePath(string baseFileName, string extension)
    {
        int counter = 1;
        string directory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(directory, baseFileName + extension);

        while (File.Exists(filePath))
        {
            // Increment the file name (e.g., Numbers_1.txt, Numbers_2.txt, etc.)
            filePath = Path.Combine(directory, $"{baseFileName}_{counter}{extension}");
            counter++;
        }

        return filePath;
    }
}
