using System;
using System.Diagnostics;
using System.Windows;
using Grpc.Net.Client;
using Mapsui.Utilities;
using Microsoft.Extensions.Configuration;
using static DigiWACS.Client.App;

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

	private void gRPCConnect_OnClick(object sender, RoutedEventArgs e) {
		try {
			gRPC_Client.CreateChannel(Config.GetConnectionString("SeverConnection"));
		}
		catch (Exception exception) {
			Console.WriteLine(exception);
			throw;
		}
	}
}