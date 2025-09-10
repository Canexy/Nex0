using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Nexo.ViewModels;
using Nexo.Views;
using System;
using System.Diagnostics;

namespace Nexo
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            try
            {
                AvaloniaXamlLoader.Load(this);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"XAML Loading Error: {ex}");
                throw;
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // Eliminar validación de datos para mejorar el rendimiento
            if (BindingPlugins.DataValidators.Count > 0)
            {
                BindingPlugins.DataValidators.RemoveAt(0);
            }

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                try
                {
                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = new MainWindowViewModel()
                    };
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"MainWindow creation failed: {ex}");
                    throw;
                }
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}