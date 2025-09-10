using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Nexo.ViewModels;
using Nexo.Views;
using System.Linq;

namespace Nexo
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // Eliminar validación de datos duplicada
            if (BindingPlugins.DataValidators.Count > 0)
            {
                var dataValidationPluginsToRemove = 
                    BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();
                
                foreach (var plugin in dataValidationPluginsToRemove)
                {
                    BindingPlugins.DataValidators.Remove(plugin);
                }
            }

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}