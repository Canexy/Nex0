using Avalonia;
using System;
using System.IO;

namespace Nexo
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                // Registrar el error en un archivo
                File.WriteAllText("error_log.txt", $"Application failed: {ex}");
                Console.WriteLine($"Application failed: {ex}");
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}