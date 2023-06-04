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
			Debug.WriteLine("MainWindow()");
			InitializeComponent();
			MapControl1.Map?.Layers.Add(Mapsui.Utilities.OpenStreetMap.CreateTileLayer());
		}
		private void NewWindowButton_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("NewWindowButton_OnClick()");
			PluginWindow newWindow = new PluginWindow();
			newWindow.Show();
		}

		private void PluginTestButton_OnClick(object sender, RoutedEventArgs e)
		{	
			Debug.WriteLine("PluginTestButton_OnClick()");
		}
	}
}