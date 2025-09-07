using System.Linq;
using System.Windows.Input;
using Nexo.Models;

namespace Nexo.ViewModels
{
    public class EmulatorViewModel : ViewModelBase
    {
        private readonly Emulator _emulator;
        private string _name;
        private string _executablePath;
        private string _arguments;
        private string _extensions;
        private string _configuration;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        public string ExecutablePath
        {
            get => _executablePath;
            set => SetProperty(ref _executablePath, value);
        }
        
        public string Arguments
        {
            get => _arguments;
            set => SetProperty(ref _arguments, value);
        }
        
        public string Extensions
        {
            get => _extensions;
            set => SetProperty(ref _extensions, value);
        }
        
        public string Configuration
        {
            get => _configuration;
            set => SetProperty(ref _configuration, value);
        }
        
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowseCommand { get; }
        
        public EmulatorViewModel(Emulator emulator)
        {
            _emulator = emulator;
            
            // Inicializar propiedades desde el modelo
            _name = emulator.Name;
            _executablePath = emulator.ExecutablePath;
            _arguments = emulator.Arguments;
            _extensions = string.Join(", ", emulator.AssociatedExtensions);
            _configuration = emulator.Configuration;
            
            // Configurar comandos básicos por ahora
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            BrowseCommand = new RelayCommand(Browse);
        }
        
        private void Save()
        {
            // Actualizar el modelo con los valores de la vista
            _emulator.Name = Name;
            _emulator.ExecutablePath = ExecutablePath;
            _emulator.Arguments = Arguments;
            _emulator.AssociatedExtensions = Extensions.Split(',')
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e))
                .ToList();
            _emulator.Configuration = Configuration;
        }
        
        private void Cancel()
        {
            // Lógica para cancelar
        }
        
        private void Browse()
        {
            // Lógica para examinar archivos
        }
    }
}