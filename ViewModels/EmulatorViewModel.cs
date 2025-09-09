using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nexo.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using Avalonia.Controls.ApplicationLifetimes;

namespace Nexo.ViewModels
{
    public partial class EmulatorViewModel : ViewModelBase
    {
        private readonly Emulator _emulator;
        
        [ObservableProperty]
        private string _name;
        
        [ObservableProperty]
        private string _executablePath;
        
        [ObservableProperty]
        private string _arguments;
        
        [ObservableProperty]
        private string _extensions;
        
        [ObservableProperty]
        private string _configuration;
        
        [ObservableProperty]
        private string _iconPath;

        // Añade esta propiedad para mostrar las extensiones en el DataGrid
        public string ExtensionsDisplay => string.Join(", ", _emulator.AssociatedExtensions);

        public EmulatorViewModel(Emulator emulator)
        {
            _emulator = emulator;
            _name = emulator.Name;
            _executablePath = emulator.ExecutablePath;
            _arguments = emulator.Arguments;
            _extensions = string.Join(", ", emulator.AssociatedExtensions);
            _configuration = emulator.Configuration;
            _iconPath = emulator.IconPath;
        }

        public Emulator GetModel()
        {
            _emulator.Name = Name;
            _emulator.ExecutablePath = ExecutablePath;
            _emulator.Arguments = Arguments;
            _emulator.AssociatedExtensions = Extensions.Split(',')
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e))
                .ToList();
            _emulator.Configuration = Configuration;
            _emulator.IconPath = IconPath;
            
            return _emulator;
        }

        [RelayCommand]
        public async Task LaunchGame(string? gamePath = null)  // Cambiado de private a public
        {
            if (string.IsNullOrEmpty(ExecutablePath))
            {
                // Mostrar mensaje de error
                return;
            }

            string path = gamePath ?? "";
            
            // Si no se proporciona una ruta de juego, abrir diálogo de selección
            if (string.IsNullOrEmpty(path))
            {
                var window = GetMainWindow();
                if (window == null) return;

                var options = new FilePickerOpenOptions
                {
                    Title = $"Selecciona un juego para {Name}",
                    AllowMultiple = false,
                    FileTypeFilter = new[] {
                        new FilePickerFileType("Juegos") { 
                            Patterns = _emulator.AssociatedExtensions.Select(e => $"*{e}").ToArray() 
                        }
                    }
                };
                
                var result = await window.StorageProvider.OpenFilePickerAsync(options);
                if (result.Count > 0)
                {
                    path = result[0].Path.LocalPath;
                }
            }
            
            // Lanzar el emulador con el juego
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = ExecutablePath,
                        Arguments = $"{Arguments} \"{path}\"",
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error launching emulator: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        private async Task BrowseExecutable()
        {
            var window = GetMainWindow();
            if (window == null) return;

            var options = new FilePickerOpenOptions
            {
                Title = "Selecciona el ejecutable del emulador",
                AllowMultiple = false,
                FileTypeFilter = new[] { 
                    new FilePickerFileType("Ejecutables") { Patterns = new[] { "*.exe", "*.bat", "*.app" } } 
                }
            };
            
            var result = await window.StorageProvider.OpenFilePickerAsync(options);
            if (result.Count > 0)
            {
                ExecutablePath = result[0].Path.LocalPath;
            }
        }

        private Window? GetMainWindow()  // Añadido el nullable operator
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                return desktopLifetime.MainWindow;
            }
            return null;
        }
    }
}