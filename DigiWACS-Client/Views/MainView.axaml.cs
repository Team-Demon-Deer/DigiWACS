using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using DigiWACS_Client.ViewModels;
using DigiWACS_Client.Controls;
using Mapsui.UI.Avalonia;

namespace DigiWACS_Client.Views;

public partial class MainView : UserControl {
    private const bool CustomDrawControl = false;
    
    public MainView() {
        InitializeComponent();
        this.Loaded += (s, e) => {
            if (CustomDrawControl) {
                MapContainer.Content = new CustomDrawingExampleControl() {
                    Name = "MapControl"
                };
            }
            else {
                MapContainer.Content = new MapControl() {
                    Name = "MapControl",
                    Map = ((MainViewModel)DataContext).MapInterface.AreaMap,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    
                };
                ((MapControl)MapContainer.Content).PointerPressed += (object s, PointerPressedEventArgs e) => asd(s, e);
                //PropertiesView.DataContext = (MainViewModel)DataContext; 
            }
        };
    }
    
    private void InitializeComponent() {
        InitializeComponent(true);
    }
    
    private void UnclosableTab_Closing(object? sender, global::System.ComponentModel.CancelEventArgs e) {
        e.Cancel = true;

        var popup = new global::DigiWACS_Client.Views.MessagePopup("You can't close this tab.\nMaybe there's unsaved work or something idk")  {
            WindowStartupLocation = global::Avalonia.Controls.WindowStartupLocation.CenterOwner
        };
        popup.ShowDialog(Avalonia.VisualTree.VisualExtensions.GetVisualParent<Window>(this));
    }

    private void asd(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        
        if (e.GetCurrentPoint(sender as Control).Properties.IsMiddleButtonPressed)
        {
            Debug.Print("wahoo!");
        }
        
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
