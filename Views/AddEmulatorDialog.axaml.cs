using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nexo.ViewModels;

namespace Nexo.Views
{
    public partial class AddEmulatorDialog : Window
    {
        public AddEmulatorDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void AddButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataContext is AddEmulatorViewModel vm)
            {
                if (string.IsNullOrWhiteSpace(vm.Name) || 
                    string.IsNullOrWhiteSpace(vm.ExecutablePath) || 
                    string.IsNullOrWhiteSpace(vm.Extensions))
                {
                    vm.ErrorMessage = "Todos los campos son obligatorios";
                    return;
                }
                
                Close(true);
            }
        }

        private void CancelButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close(false);
        }
    }
}