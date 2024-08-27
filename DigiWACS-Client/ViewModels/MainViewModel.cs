using System.Collections.ObjectModel;
using System.Windows.Input;
using DigiWACS_Client.Models;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Styles;
using NetTopologySuite.Geometries;
using ReactiveUI;

namespace DigiWACS_Client.ViewModels;

public partial class MainViewModel : ViewModelBase {
	public string Greeting => "Welcome to Avalonia!";

	public Map AreaMap;
	private Coordinate? _hookPrimary;
	public Coordinate HookPrimary
	{
		get => _hookPrimary;
		set => this.RaiseAndSetIfChanged(ref _hookPrimary, value);
	}
	
	private Coordinate? _hookSecondary;
	public Coordinate HookSecondary
	{
		get => _hookSecondary;
		set => this.RaiseAndSetIfChanged(ref _hookSecondary, value);
	}
	
	private ObservableMemoryLayer<Coordinate> _hookLayer;
	public ObservableMemoryLayer<Coordinate>? HookLayer
	{
		get => _hookLayer;
		set => this.RaiseAndSetIfChanged(ref _hookLayer, value);
	}
	
	public MainViewModel() {
		AreaMap = new Map();
		HookLayer = new ObservableMemoryLayer<Coordinate>(
			(Coordinate c) => new PointFeature(c.X, c.Y)
		)
		{
			Style = SymbolStyles.CreatePinStyle(),
			ObservableCollection = (new TrackMock()).items
		};
		var a = this;
		AreaMap.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
		AreaMap.Layers.Add(HookLayer);
		AreaMap.Info += (s, e) =>
		{
			HookControl.PlacePrimaryHook(s, e, ref a);
			//MapControl.Refresh();
		};
		
	}
}