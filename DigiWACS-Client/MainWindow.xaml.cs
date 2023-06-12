using System;
using System.Diagnostics;
using System.Windows;
using Mapsui.Utilities;

namespace DigiWACS.Client;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
	public delegate void TestedEventHandler( object source, EventArgs eventArgs );
	public event TestedEventHandler TestedEvent;
	private MapManagement MapManager = new MapManagement();

	public MainWindow() {
		Trace.WriteLine("MainWindow() Start");
		InitializeComponent();
		MapControl1.Map = MapManager.InitializeMap();
		var Event_Test = new Event_Test();
		TestedEvent += Event_Test.OnTestedEvent;
		//MapControl1.Map?.Layers.Add( TestEntity );
	}


	private void NewWindowButton_OnClick(object sender, RoutedEventArgs e) {
		Trace.WriteLine("NewWindowButton_OnClick()");
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
<<<<<<< Updated upstream
}
=======

	private void SettingsMenuItem_Click( object sender, RoutedEventArgs e ) {
		OpenSettingsMenu();
	}
	protected virtual void OpenSettingsMenu() { if ( OpenSettingsMenu != null ) OpenSettingsMenu(); }
}
>>>>>>> Stashed changes
