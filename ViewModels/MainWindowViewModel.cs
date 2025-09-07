using System.Collections.ObjectModel;
using System.Windows.Input;
using Nexo.Models;

namespace Nexo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<Emulator> _emulators;
        private Emulator? _selectedEmulator;
        private bool _isDarkMode = true;
        
        public ObservableCollection<Emulator> Emulators
        {
            get => _emulators;
            set => SetProperty(ref _emulators, value);
        }
        
        public Emulator? SelectedEmulator
        {
            get => _selectedEmulator;
            set
            {
                if (SetProperty(ref _selectedEmulator, value))
                {
                    // Actualizar el estado de los comandos cuando cambia la selección
                    ((RelayCommand)EditEmulatorCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)DeleteEmulatorCommand).RaiseCanExecuteChanged();
                }
            }
        }
        
        public bool IsDarkMode
        {
            get => _isDarkMode;
            set => SetProperty(ref _isDarkMode, value);
        }
        
        public ICommand AddEmulatorCommand { get; }
        public ICommand EditEmulatorCommand { get; }
        public ICommand DeleteEmulatorCommand { get; }
        public ICommand ToggleThemeCommand { get; }
        
        public MainWindowViewModel()
        {
            _emulators = new ObservableCollection<Emulator>();
            
            // Comandos básicos
            AddEmulatorCommand = new RelayCommand(AddEmulator);
            EditEmulatorCommand = new RelayCommand(EditEmulator, () => SelectedEmulator != null);
            DeleteEmulatorCommand = new RelayCommand(DeleteEmulator, () => SelectedEmulator != null);
            ToggleThemeCommand = new RelayCommand(ToggleTheme);
            
            // Datos de ejemplo
            Emulators.Add(new Emulator {
                Name = "RetroArch",
                ExecutablePath = "/path/to/retroarch",
                Configuration = "Default"
            });
            
            Emulators.Add(new Emulator {
                Name = "PCSX2",
                ExecutablePath = "/path/to/pcsx2",
                Configuration = "High Performance"
            });
        }
        
        private void AddEmulator()
        {
            var newEmulator = new Emulator { Name = "Nuevo Emulador" };
            Emulators.Add(newEmulator);
            SelectedEmulator = newEmulator;
        }
        
        private void EditEmulator()
        {
            // Lógica para editar
        }
        
        private void DeleteEmulator()
        {
            if (SelectedEmulator != null)
            {
                Emulators.Remove(SelectedEmulator);
                SelectedEmulator = null;
            }
        }
        
        private void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
        }
    }
}