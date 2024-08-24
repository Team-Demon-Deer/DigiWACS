using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui.UI.Avalonia;
using ReactiveUI;

namespace DigiWACS_Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
	public string Greeting => "Welcome to Avalonia!";

	private MapControl areaMap;

	public MapControl AreaMap
	{
		get => areaMap;
		set => areaMap = value;
	}

#pragma warning restore CA1822 // Mark members as static
}