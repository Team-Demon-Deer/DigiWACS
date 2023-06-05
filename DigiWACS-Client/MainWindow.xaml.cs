using System.Diagnostics;
using System.Linq;
using System.Windows;
using DigiWACS.PluginBase;
using Mapsui.Utilities;


namespace DigiWACS.Client;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
	public MainWindow() {
		Trace.WriteLine("MainWindow() Start");
		InitializeComponent();
		MapControl1.Map?.Layers.Add(OpenStreetMap.CreateTileLayer());
	}

	private void NewWindowButton_OnClick(object sender, RoutedEventArgs e) {
		Trace.WriteLine("NewWindowButton_OnClick()");
		var newWindow = new PluginWindow();
		newWindow.Show();
	}

	private void PluginTestButton_OnClick(object sender, RoutedEventArgs e) {
	}
}