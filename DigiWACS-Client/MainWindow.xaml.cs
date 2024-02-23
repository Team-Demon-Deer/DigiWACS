using System;
using System.Diagnostics;
using System.Windows;

namespace DigiWACS.Client;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
	public delegate void TestedEventHandler( object source, EventArgs eventArgs );
	public event TestedEventHandler TestedEvent;
	private MapManagement MapManager = new MapManagement();

	public MainWindow() {
		Debug.Print( "MainWindow() Start" );
		InitializeComponent();
		MapControl.Map = MapManager.InitializeMap();
		var Event_Test = new Event_Test();
		TestedEvent += Event_Test.OnTestedEvent;
		//MapControl1.Map?.Layers.Add( TestEntity );
	}

	private void NewWindowButton_OnClick(object sender, RoutedEventArgs e) {
	}

	private void gRPCConnect_OnClick( object sender, RoutedEventArgs e) {
	}

	private void TestEvent_OnClick( object sender, RoutedEventArgs e ) {
		TestedEvent?.Invoke( sender, e );
	}

	private void Reconnect_Click( object sender, RoutedEventArgs e ) {

	}
	private void NewConnection_Click( object sender, RoutedEventArgs e ) {

	}
	private void ManageConnections_Click( object sender, RoutedEventArgs e ) {

	}
	private void SettingsMenuItem_Click( object sender, RoutedEventArgs e ) {
	}
}

