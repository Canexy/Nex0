using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using System;

namespace Nexo.Views
{
    public partial class NotificationControl : UserControl
    {
        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<NotificationControl, string>(nameof(Title));
            
        public static readonly StyledProperty<string> MessageProperty =
            AvaloniaProperty.Register<NotificationControl, string>(nameof(Message));
            
        public static readonly StyledProperty<NotificationType> TypeProperty =
            AvaloniaProperty.Register<NotificationControl, NotificationType>(nameof(Type), 
                defaultValue: NotificationType.Info,
                coerce: OnTypeChanged);
            
        public static readonly StyledProperty<IRelayCommand> CloseCommandProperty =
            AvaloniaProperty.Register<NotificationControl, IRelayCommand>(nameof(CloseCommand));

        // Properties for the icon styling
        public static readonly StyledProperty<IBrush> IconBackgroundProperty =
            AvaloniaProperty.Register<NotificationControl, IBrush>(nameof(IconBackground));
            
        public static readonly StyledProperty<IBrush> IconForegroundProperty =
            AvaloniaProperty.Register<NotificationControl, IBrush>(nameof(IconForeground));
            
        public static readonly StyledProperty<Geometry> IconDataProperty =
            AvaloniaProperty.Register<NotificationControl, Geometry>(nameof(IconData));

        public string Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        
        public string Message
        {
            get => GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        
        public NotificationType Type
        {
            get => GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }
        
        public IRelayCommand CloseCommand
        {
            get => GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }
        
        public IBrush IconBackground
        {
            get => GetValue(IconBackgroundProperty);
            set => SetValue(IconBackgroundProperty, value);
        }
        
        public IBrush IconForeground
        {
            get => GetValue(IconForegroundProperty);
            set => SetValue(IconForegroundProperty, value);
        }
        
        public Geometry IconData
        {
            get => GetValue(IconDataProperty);
            set => SetValue(IconDataProperty, value);
        }

        public NotificationControl()
        {
            InitializeComponent();
            // Update the style when the control is initialized
            UpdateNotificationStyle();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private static NotificationType OnTypeChanged(AvaloniaObject sender, NotificationType value)
        {
            if (sender is NotificationControl control)
            {
                control.UpdateNotificationStyle();
            }
            return value;
        }
        
        private void UpdateNotificationStyle()
        {
            switch (Type)
            {
                case NotificationType.Success:
                    IconBackground = new SolidColorBrush(Color.Parse("#1038E07B")); // Green with 10% opacity
                    IconForeground = new SolidColorBrush(Color.Parse("#38E07B")); // Green
                    IconData = Geometry.Parse("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z");
                    break;
                case NotificationType.Warning:
                    IconBackground = new SolidColorBrush(Color.Parse("#10eab300")); // Yellow with 10% opacity
                    IconForeground = new SolidColorBrush(Color.Parse("#eab300")); // Yellow
                    IconData = Geometry.Parse("M12 5.99L19.53 19H4.47L12 5.99M12 2L1 21h22L12 2zm1 14h-2v2h2v-2zm0-6h-2v4h2v-4z");
                    break;
                case NotificationType.Error:
                    IconBackground = new SolidColorBrush(Color.Parse("#10ef4444")); // Red with 10% opacity
                    IconForeground = new SolidColorBrush(Color.Parse("#ef4444")); // Red
                    IconData = Geometry.Parse("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z");
                    break;
                case NotificationType.Info:
                default:
                    IconBackground = new SolidColorBrush(Color.Parse("#103b82f6")); // Blue with 10% opacity
                    IconForeground = new SolidColorBrush(Color.Parse("#3b82f6")); // Blue
                    IconData = Geometry.Parse("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z");
                    break;
            }
        }
    }
    
    public enum NotificationType
    {
        Info,
        Success,
        Warning,
        Error
    }
}