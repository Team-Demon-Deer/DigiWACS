using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DigiWACS_Client.Models;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Styles;
using Mapsui.UI.Avalonia;
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
	public ICommand OnLoadedCommand { get; private set; }
	
	public MainViewModel()
	{
		AreaMap = new Map();
		var T = typeof(Coordinate); 
		HookLayer = new ObservableMemoryLayer<Coordinate>((Coordinate c) => new PointFeature(c.X, c.Y));
		HookLayer.Style = SymbolStyles.CreatePinStyle();
		HookLayer.ObservableCollection = new ObservableCollection<Coordinate>();
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