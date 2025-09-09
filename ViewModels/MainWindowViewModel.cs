using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nexo.Models;
using Nexo.Services;
using Nexo.Views;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Nexo.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly DataService _dataService = new DataService();
        
        [ObservableProperty]
        private ObservableCollection<EmulatorViewModel> _emulators = new();
        
        [ObservableProperty]
        private EmulatorViewModel? _selectedEmulator;
        
        [ObservableProperty]
        private string _message = "";
        
        [ObservableProperty]
        private bool _isDarkMode = true;

        public MainWindowViewModel()
        {
            LoadEmulators();
        }

        private void LoadEmulators()
        {
            var emulators = _dataService.LoadEmulators();
            foreach (var emulator in emulators)
            {
                Emulators.Add(new EmulatorViewModel(emulator));
            }
        }

        public void SaveEmulators()
        {
            var emulatorModels = Emulators.Select(e => e.GetModel()).ToList();
            _dataService.SaveEmulators(emulatorModels);
        }

        [RelayCommand]
        private async Task AddEmulator()
        {
            var window = GetMainWindow();
            if (window == null) return;

            var dialog = new AddEmulatorDialog();
            var result = await dialog.ShowDialog<bool>(window);
            
            if (result && dialog.DataContext is AddEmulatorViewModel vm)
            {
                var newEmulator = new Emulator
                {
                    Name = vm.Name,
                    ExecutablePath = vm.ExecutablePath,
                    AssociatedExtensions = vm.Extensions.Split(',', 
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()
                };
                
                Emulators.Add(new EmulatorViewModel(newEmulator));
                SaveEmulators();
                ShowMessage("Emulador agregado correctamente");
            }
        }

        [RelayCommand]
        private async Task EditEmulator()
        {
            if (SelectedEmulator == null) return;
            
            var window = GetMainWindow();
            if (window == null) return;

            var dialog = new EditEmulatorWindow(SelectedEmulator);
            var result = await dialog.ShowDialog<bool>(window);
            
            if (result)
            {
                SaveEmulators();
                ShowMessage("Emulador actualizado correctamente");
            }
        }

        [RelayCommand]
        private void DeleteEmulator()
        {
            if (SelectedEmulator != null)
            {
                Emulators.Remove(SelectedEmulator);
                SaveEmulators();
                ShowMessage("Emulador eliminado correctamente");
            }
        }

        [RelayCommand]
        private async Task OpenGame()
        {
            var window = GetMainWindow();
            if (window == null) return;

            var options = new FilePickerOpenOptions
            {
                Title = "Selecciona un juego",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("Todos los archivos") { Patterns = new[] { "*.*" } } }
            };
            
            var result = await window.StorageProvider.OpenFilePickerAsync(options);
            if (result.Count > 0)
            {
                var gamePath = result[0].Path.LocalPath;
                var extension = System.IO.Path.GetExtension(gamePath).ToLower();
                
                // Buscar emuladores compatibles con esta extensión
                var compatibleEmulators = Emulators.Where(e => 
                    e.GetModel().AssociatedExtensions.Any(ext => 
                        ext.ToLower() == extension)).ToList();
                
                if (compatibleEmulators.Count == 1)
                {
                    // Si solo hay un emulador compatible, usarlo directamente
                    await compatibleEmulators[0].LaunchGame(gamePath);  // Ahora es accesible
                    ShowMessage($"Ejecutando {System.IO.Path.GetFileName(gamePath)} con {compatibleEmulators[0].Name}");
                }
                else if (compatibleEmulators.Count > 1)
                {
                    // Si hay múltiples emuladores compatibles, mostrar diálogo de selección
                    ShowMessage("Múltiples emuladores compatibles. Implementar selección.");
                    // TODO: Implementar diálogo de selección de emulador
                }
                else
                {
                    ShowMessage($"No hay emuladores configurados para la extensión {extension}");
                }
            }
        }

        [RelayCommand]
        private void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            // Aquí deberías implementar la lógica para cambiar el tema de la aplicación
        }

        public void ShowMessage(string msg, int durationMs = 2500)
        {
            Message = msg;
            // Implementar lógica para ocultar el mensaje después de un tiempo
            // Puedes usar un Timer o una función async
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