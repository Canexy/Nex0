using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nexo.Views
{
    public partial class EditEmulatorWindow : Window
    {
        public EditEmulatorWindow()
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
    }
}