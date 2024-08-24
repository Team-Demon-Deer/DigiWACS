using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using DigiWACS_Client.ViewModels;
using Mapsui.UI.Avalonia;

namespace DigiWACS_Client.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		MainWindowViewModel.MapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
	}
	
	private void UnclosableTab_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
	{
		e.Cancel = true;

		var popup = new MessagePopup("You can't close this tab.\nMaybe there's unsaved work or something idk")
		{
			WindowStartupLocation = WindowStartupLocation.CenterOwner
		};
		popup.ShowDialog(this);
	}

	protected override void OnLoaded(RoutedEventArgs e)
	{
		base.OnLoaded(e);
		if (Design.IsDesignMode)
			return;
	}
}

public class MessagePopup : Window
{
	public MessagePopup(string message)
	{
		SizeToContent = SizeToContent.WidthAndHeight;

		var panel = new StackPanel() { Spacing = 10, Margin = new Thickness(10) };

		var okButton = new Button() { Content = "OK", HorizontalAlignment = HorizontalAlignment.Right };

		okButton.Click += (s, e) => Close();

		panel.Children.Add(new TextBlock() { Text = message});
		panel.Children.Add(okButton);

		Content = panel;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		base.OnKeyDown(e);
		if (e.Key == Key.Enter)
			Close();
	}
}