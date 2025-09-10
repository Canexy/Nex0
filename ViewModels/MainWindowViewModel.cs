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
using System.Threading;

namespace Nexo.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly DataService _dataService = new DataService();
        private CancellationTokenSource _messageCancellationTokenSource;
        
        // CAMBIO IMPORTANTE: Usar ObservableCollection en lugar de List
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
            Debug.WriteLine("MainWindowViewModel constructor started");
            LoadEmulators();
            _messageCancellationTokenSource = new CancellationTokenSource();
            Debug.WriteLine("MainWindowViewModel constructor completed");
        }

        private void LoadEmulators()
        {
            Debug.WriteLine("Loading emulators...");
            var emulators = _dataService.LoadEmulators();
            Debug.WriteLine($"Found {emulators.Count} emulators");
            
            Emulators.Clear();
            foreach (var emulator in emulators)
            {
                Emulators.Add(new EmulatorViewModel(emulator));
            }
            
            // Force UI update
            OnPropertyChanged(nameof(Emulators));
            Debug.WriteLine("Emulators loaded successfully");
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
            var vm = new AddEmulatorViewModel();
            dialog.DataContext = vm;
            
            var result = await dialog.ShowDialog<bool>(window);
            
            if (result)
            {
                var newEmulator = new Emulator
                {
                    Name = vm.Name,
                    ExecutablePath = vm.ExecutablePath,
                    AssociatedExtensions = vm.Extensions.Split(',', 
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()
                };
                
                // Add to the observable collection
                Emulators.Add(new EmulatorViewModel(newEmulator));
                
                SaveEmulators();
                ShowMessage("Emulador agregado correctamente", 3000);
                
                // Force UI update
                OnPropertyChanged(nameof(Emulators));
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
                // Forzar actualización de la UI
                OnPropertyChanged(nameof(Emulators));
                
                SaveEmulators();
                ShowMessage("Emulador actualizado correctamente", 3000);
            }
        }

        [RelayCommand]
        private void DeleteEmulator()
        {
            if (SelectedEmulator != null)
            {
                Emulators.Remove(SelectedEmulator);
                
                // Forzar actualización de la UI
                OnPropertyChanged(nameof(Emulators));
                
                SaveEmulators();
                ShowMessage("Emulador eliminado correctamente", 3000);
                
                SelectedEmulator = null;
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
                    await compatibleEmulators[0].LaunchGame(gamePath);
                    ShowMessage($"Ejecutando {System.IO.Path.GetFileName(gamePath)} con {compatibleEmulators[0].Name}", 3000);
                }
                else if (compatibleEmulators.Count > 1)
                {
                    ShowMessage("Múltiples emuladores compatibles. Implementar selección.", 3000);
                }
                else
                {
                    ShowMessage($"No hay emuladores configurados para la extensión {extension}", 3000);
                }
            }
        }

        [RelayCommand]
        private void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
        }

        public async void ShowMessage(string msg, int durationMs = 2500)
        {
            // Cancelar cualquier mensaje anterior
            _messageCancellationTokenSource.Cancel();
            _messageCancellationTokenSource = new CancellationTokenSource();
            
            Message = msg;
            
            try
            {
                // Esperar el tiempo especificado y luego limpiar el mensaje
                await Task.Delay(durationMs, _messageCancellationTokenSource.Token);
                Message = "";
            }
            catch (TaskCanceledException)
            {
                // El mensaje fue cancelado por uno nuevo, no hacer nada
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