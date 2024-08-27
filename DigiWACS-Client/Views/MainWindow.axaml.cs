using Avalonia.Controls;
using Avalonia.Interactivity;

namespace DigiWACS_Client.Views;

public partial class MainWindow : Window {
	public MainWindow() {
		InitializeComponent();
	}
	protected override void OnLoaded(RoutedEventArgs e) {
		base.OnLoaded(e);
		if (Design.IsDesignMode)
			return;
	}
}