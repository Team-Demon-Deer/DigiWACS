using System.Diagnostics;
using System.Windows;

namespace DigiWACS.Client
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
			Trace.WriteLine("New Window Button clicked.");
			PluginWindow newWindow = new PluginWindow();
			newWindow.Show();
			Trace.WriteLine("New window creation attempted.");
		}

		private void PluginTestButton_OnClick(object sender, RoutedEventArgs e)
		{

;		}
	}
}