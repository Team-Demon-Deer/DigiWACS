using Avalonia.Controls;
using Avalonia.Interactivity;
using DigiWACS_Client.ViewModels;

namespace DigiWACS_Client.Views;

public partial class SettingsWindow : Window {
	//todo: if something changed, set this.
	private bool settingsChanged = false;
	
	public SettingsWindow() {
		InitializeComponent();
		SizeToContent = SizeToContent.WidthAndHeight;
		
		ApplyButtonState(false);
	}

	private void ApplyButtonState(bool State) {
		ApplyButton.IsEnabled = State;
	}

	private void SaveSettings() {
		var dataContextSettings = ((MainViewModel)DataContext).Settings;
		dataContextSettings.SaveSettingsToJSON(dataContextSettings);
		
		settingsChanged = false;
	}
	
	private void OkButton_OnClick(object? sender, RoutedEventArgs e) {
		e.Handled = true;
		SaveSettings();
		Close();
	}
	
	private void CancelButton_OnClick(object? sender, RoutedEventArgs e) {
		e.Handled = true;
		Close();
	}
	
	private void ApplyButton_OnClick(object? sender, RoutedEventArgs e) {
		e.Handled = true;
		SaveSettings();
	}
}