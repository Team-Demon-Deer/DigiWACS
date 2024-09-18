using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using DigiWACS_Client.ViewModels;

namespace DigiWACS_Client.Views;

public partial class MainView : UserControl {
    
    public MainView() {
        InitializeComponent();
        this.Loaded += (s, e) => {
            var dataContext = DataContext as MainViewModel;
            
            MapContainer.Content = (dataContext.MapInterface.MapInterfaceControl);
            SettingsMenuItem.DataContext = new SettingsViewModel(dataContext.Settings);
            };
            
            //PropertiesView.DataContext = (MainViewModel)DataContext; 
        }
    
    private void InitializeComponent() {
        InitializeComponent(true);
    }

    private void OnSettingsButtonClick(object sender, RoutedEventArgs e) {
        var dataContext = DataContext as MainViewModel;
        var settings = new SettingsView() {
            DataContext = new SettingsViewModel(dataContext.Settings),
            WindowStartupLocation = global::Avalonia.Controls.WindowStartupLocation.CenterOwner
        };
        settings.ShowDialog(Parent as Window);
    }
    
    private void UnclosableTab_Closing(object? sender, global::System.ComponentModel.CancelEventArgs e) {
        e.Cancel = true;

        var popup = new global::DigiWACS_Client.Views.MessagePopup("You can't close this tab.\nMaybe there's unsaved work or something idk")  {
            WindowStartupLocation = global::Avalonia.Controls.WindowStartupLocation.CenterOwner
        };
        popup.ShowDialog(Parent as Window);
        //Avalonia.VisualTree.VisualExtensions.GetVisualParent<Window>(this)
    }
}
public class MessagePopup : Window {
    public MessagePopup(string message) {
        SizeToContent = SizeToContent.WidthAndHeight;
        var panel = new StackPanel() { Spacing = 10, Margin = new Thickness(10) };
        var okButton = new Button() { Content = "OK", HorizontalAlignment = HorizontalAlignment.Right };
        okButton.Click += (s, e) => Close();
        panel.Children.Add(new TextBlock() { Text = message});
        panel.Children.Add(okButton);
        Content = panel;
    }

    protected override void OnKeyDown(KeyEventArgs e) {
        base.OnKeyDown(e);
        if (e.Key == Key.Enter)
            Close();
    }
}
