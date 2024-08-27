using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using DigiWACS_Client.ViewModels;
using DigiWACS_Client.Models;
using Mapsui;
using Mapsui.UI.Avalonia;

namespace DigiWACS_Client.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}
	

	protected override void OnLoaded(RoutedEventArgs e)
	{
		base.OnLoaded(e);
		
		if (Design.IsDesignMode)
			return;
	}
}