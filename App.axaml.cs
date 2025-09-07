using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Nexo.ViewModels;
using Nexo.Views;
using System;

namespace Nexo
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            Console.WriteLine("Inicializando aplicación - Cargando XAML...");
            try
            {
                AvaloniaXamlLoader.Load(this);
                Console.WriteLine("XAML cargado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar XAML: {ex}");
                throw;
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            Console.WriteLine("Framework de Avalonia inicializado.");
            
            // Eliminar validación de datos para mejorar el rendimiento
            try
            {
                BindingPlugins.DataValidators.RemoveAt(0);
                Console.WriteLine("Validadores de datos removidos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al remover validadores: {ex}");
            }

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Console.WriteLine("Configurando ventana principal...");
                try
                {
                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = new MainWindowViewModel()
                    };
                    Console.WriteLine("Ventana principal configurada correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al configurar ventana principal: {ex}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine("No se detectó ApplicationLifetime de escritorio.");
            }

            base.OnFrameworkInitializationCompleted();
            Console.WriteLine("Inicialización de aplicación completada.");
        }
    }
}