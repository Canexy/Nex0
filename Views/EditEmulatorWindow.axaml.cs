using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nexo.ViewModels;

namespace Nexo.Views
{
    public partial class EditEmulatorWindow : Window  // Debe ser EditEmulatorWindow
    {
        public EditEmulatorWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public EditEmulatorWindow(EmulatorViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SaveButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close(true);
        }

        private void CancelButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close(false);
        }
    }
}