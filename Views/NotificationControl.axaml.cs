using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nexo.Views
{
    public partial class NotificationControl : UserControl
    {
        public NotificationControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}