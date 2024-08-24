using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui.UI.Avalonia;
using NetTopologySuite.Geometries;
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

	private Coordinate hookPrimary;
	private Coordinate hookSecondary;

	public Coordinate HookPrimary
	{
		get => hookPrimary;
		set => hookPrimary = value;
	}
	
	public Coordinate HookSecondary
	{
		get => hookSecondary;
		set => hookSecondary = value;
	}
	
#pragma warning restore CA1822 // Mark members as static
}