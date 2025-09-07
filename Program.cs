using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Nexo
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Forzar la apertura de una consola para ver los mensajes
            AllocConsole();
            
            Console.WriteLine("=== INICIANDO APLICACIÓN NEXO ===");
            Console.WriteLine($"Directorio: {Environment.CurrentDirectory}");
            
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Console.WriteLine($"!!! EXCEPCIÓN NO CONTROLADA: {e.ExceptionObject}");
                PausarParaVerMensaje();
            };
            
            try
            {
                Console.WriteLine("Construyendo aplicación...");
                var app = BuildAvaloniaApp();
                Console.WriteLine("Iniciando aplicación...");
                app.StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!! LA APLICACIÓN FALLÓ: {ex}");
                PausarParaVerMensaje();
            }
            finally
            {
                Console.WriteLine("=== APLICACIÓN FINALIZADA ===");
                PausarParaVerMensaje();
                FreeConsole();
            }
        }

        private static void PausarParaVerMensaje()
        {
            Console.WriteLine("Presiona Enter para continuar...");
            Console.ReadLine();
        }

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}