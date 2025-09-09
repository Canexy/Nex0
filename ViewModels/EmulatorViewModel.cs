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

        public string ExtensionsDisplay => string.Join(", ", _emulator.AssociatedExtensions);

        public EmulatorViewModel(Emulator emulator)
        {
            _emulator = emulator;
            Name = emulator.Name;
            ExecutablePath = emulator.ExecutablePath;
            Arguments = emulator.Arguments;
            Extensions = string.Join(", ", emulator.AssociatedExtensions);
            Configuration = emulator.Configuration;
            IconPath = emulator.IconPath;
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
            
            // Notificar que ExtensionsDisplay ha cambiado
            OnPropertyChanged(nameof(ExtensionsDisplay));
            
            return _emulator;
        }

        [RelayCommand]
        public async Task LaunchGame(string? gamePath = null)
        {
            if (string.IsNullOrEmpty(ExecutablePath))
            {
                return;
            }

            string path = gamePath ?? "";
            
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

        private Window? GetMainWindow()
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                return desktopLifetime.MainWindow;
            }
            return null;
        }
    }
}