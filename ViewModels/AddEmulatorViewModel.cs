using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;
using System;

namespace Nexo.ViewModels
{
    public partial class AddEmulatorViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _name = "";
        
        [ObservableProperty]
        private string _executablePath = "";
        
        [ObservableProperty]
        private string _extensions = "";
        
        [ObservableProperty]
        private string _errorMessage = "";

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
                
                // Intentar inferir el nombre del emulador desde la ruta
                if (string.IsNullOrEmpty(Name))
                {
                    Name = System.IO.Path.GetFileNameWithoutExtension(ExecutablePath);
                }
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