using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Styles;
using NetTopologySuite.Geometries;

namespace DigiWACS_Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static

	public string Greeting => "Welcome to Avalonia!";

	private Map _areaMap;
	
	[ObservableProperty] private Coordinate _hookPrimary;
	[ObservableProperty] private Coordinate _hookSecondary;
	
	private GenericCollectionLayer<List<IFeature>> _hookLayer;
	
	public Map AreaMap
	{
		get => _areaMap;
		set => SetProperty(ref _areaMap, value);
	}
	
	public GenericCollectionLayer<List<IFeature>> HookLayer
	{
		get => _hookLayer;
		set => _hookLayer = value;
	}
	
	public MainWindowViewModel()
	{
		AreaMap = new Map();
		HookLayer = new GenericCollectionLayer<List<IFeature>>();
		HookLayer.Style = SymbolStyles.CreatePinStyle();
	}
	

#pragma warning restore CA1822 // Mark members as static
}