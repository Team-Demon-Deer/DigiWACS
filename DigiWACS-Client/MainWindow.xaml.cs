using System;
using System.Windows;
using DigiWACS_Client;
using Mapsui.UI.Wpf;

namespace DigiWACS_Client
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			MapControl1.Map?.Layers.Add(Mapsui.Utilities.OpenStreetMap.CreateTileLayer());
		}
		private void NewWindowButton_OnClick(object sender, RoutedEventArgs e)
		{
			Console.Write("New Window Button clicked.");
			PluginPage newWindow = new PluginPage();
			newWindow.InitializeComponent();
			newWindow.ShowsNavigationUI = false;
			Console.Write("New window creation attempted.");
		}
	}
}