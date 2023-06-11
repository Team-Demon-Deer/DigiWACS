using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Grpc.Net.Client;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Utilities;
using Microsoft.Extensions.Configuration;
using static DigiWACS.Client.App;

namespace DigiWACS.Client;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
	public delegate void TestedEventHandler( object source, EventArgs eventArgs );
	public event TestedEventHandler TestedEvent;

	public MainWindow() {
		Trace.WriteLine("MainWindow() Start");
		InitializeComponent();
		MapControl1.Map?.Layers.Add(OpenStreetMap.CreateTileLayer());

		var Event_Test = new Event_Test();
		TestedEvent += Event_Test.OnTestedEvent;
		//MapControl1.Map?.Layers.Add( TestEntity );
	}


	private void NewWindowButton_OnClick(object sender, RoutedEventArgs e) {
		Trace.WriteLine("NewWindowButton_OnClick()");
		var newWindow = new PluginWindow();
		newWindow.Show();
	}

	private void gRPCConnect_OnClick( object sender, RoutedEventArgs e) {
		try {
			//string reply = gRPC_Client.GreeterMessage( "https://localhost:7001" ).ToString();
			//Trace.WriteLine( reply );
		}
		catch (Exception exception) {
			Console.WriteLine(exception);
			throw;
		}
	}

	private void TestEvent_OnClick( object sender, RoutedEventArgs e ) {
		OnTestedEvent();
	}
	protected virtual void OnTestedEvent() { if( TestedEvent != null) TestedEvent( this, EventArgs.Empty); }

	private void Reconnect_Click( object sender, RoutedEventArgs e ) {

	}
	private void NewConnection_Click( object sender, RoutedEventArgs e ) {

	}
	private void ManageConnections_Click( object sender, RoutedEventArgs e ) {

	}
}