using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using DigiWACS_Client.Controls;
using DigiWACS_Client.Services.Mapsui;
using DigiWACS_Client.ViewModels;
using Mapsui;
using Mapsui.Animations;
using Mapsui.Layers;
using Mapsui.Layers.AnimatedLayers;
using Mapsui.Limiting;
using Mapsui.Styles;
using Mapsui.Styles.Thematics;
using Mapsui.UI.Avalonia;
using Mapsui.Utilities;
using Mapsui;
using SkiaSharp;

namespace DigiWACS_Client.Models;

public class TouchEvent {
	public long Id { get; }
	public MPoint Location { get; }
	public long Tick { get; }

	public TouchEvent(long id, MPoint screenPosition, long tick)
	{
		Id = id;
		Location = screenPosition;
		Tick = tick;
	}
	
}
public class MapsuiWrapper : IMapInterface {
	private AssetManagerService _assetManagerService;
	
	public UserControl MapInterfaceControl { get; set; }
	public Map AreaMap { get; set; }
	public ShapefileProviderService ShapefileProviderService { get; set; }
	public HookProviderService HookProviderService { get; set; }
	private readonly ConcurrentDictionary<long, TouchEvent> _touches = new();
	
	public MapsuiWrapper(MainViewModel mainViewModel) {
		_assetManagerService = new AssetManagerService();
		
		AreaMap = new Map(){ CRS = "EPSG:4326"};
		AreaMap.BackColor = Color.Black;
		
		MapInterfaceControl = new BatshitCrazyCustomMapControl() {
			Name = "MapControl",
			Map = AreaMap,
			VerticalAlignment = VerticalAlignment.Stretch,
			HorizontalAlignment = HorizontalAlignment.Stretch,
		};
		
		var openStreeMapLayer = Mapsui.Tiling.OpenStreetMap.CreateTileLayer();
		//AreaMap.Layers.Add(openStreeMapLayer);
	
		ShapefileProviderService = new ShapefileProviderService(AreaMap);
		
		SKPicture s = SvgHelper.LoadSvgPicture((Stream)(MilitarySymbolConverter.Convert(10000100001101000408)));
		var i = BitmapRegistry.Instance.Register((object)s, "symbology");
		_assetManagerService.Assets.Add("symbology", i);

		AreaMap.Layers.Add((new AnimatedPointLayer(new BusPointProviderService())
		{
			IsMapInfoLayer = true,
			Easing = Easing.Linear,
			Style = new ThemeStyle(f =>
			{
				return new SymbolStyle() {
					SymbolType = SymbolType.Image,
					BitmapId = _assetManagerService.Assets["symbology"]
				};
				
			}) 
		}));
		

		HookProviderService = new HookProviderService(mainViewModel.PrimaryHook, mainViewModel.SecondaryHook, _assetManagerService);
		var animatedHookLayer = new AnimatedPointLayer(HookProviderService)
		{
			Easing = Easing.Linear,
			AnimationDuration = 1,
			Style = new ThemeStyle((f) => {
				switch (f["ID"]) {
					case HookModel.HookTypes.Primary:
						return new SymbolStyle() {
							SymbolScale = .25,
							BitmapId = _assetManagerService.Assets["PrimaryHook"]
						};
						break;
					case HookModel.HookTypes.Secondary:
						return new SymbolStyle() {
							SymbolScale = .25,
							BitmapId = _assetManagerService.Assets["SecondaryHook"]
						};
						break;
					default:
						return new SymbolStyle() {
							SymbolScale = .25,
							BitmapId = _assetManagerService.Assets["PrimaryHook"]
						};
						break;
				};
			})
		};
		AreaMap.Layers.Add(animatedHookLayer);
		
		// View Area Limitations
		AreaMap.Navigator.Limiter = new ViewportLimiterKeepWithinExtent();
		AreaMap.Navigator.OverridePanBounds = AreaMap.Extent;
		AreaMap.Home = n => n.ZoomToBox(AreaMap.Extent);
		
		
		AreaMap.Info += (object s, MapInfoEventArgs e) => MapInterfaceControl_PointerPressed(s, e, mainViewModel);
		//MapInterfaceControl.PointerPressed += (object s, PointerPressedEventArgs e) => MiddleClickTest(s, e, mainViewModel);
	}
	
	private void MiddleClickTest(object? sender, Avalonia.Input.PointerPressedEventArgs e, MainViewModel context) {
		PointerPoint point = e.GetCurrentPoint(sender as Control);
		MapInfo mapinfo = ((BatshitCrazyCustomMapControl)MapInterfaceControl).GetMapInfo(new MPoint(point.Position.X, point.Position.Y));
		HookModel activeHook;
		if (point.Properties.IsLeftButtonPressed) {
			activeHook = context.PrimaryHook;
		} else {
			activeHook = context.SecondaryHook;
		}
		if (mapinfo.Feature == null) {
			activeHook.Place(mapinfo.WorldPosition);		
		} else {
			activeHook.Place((PointFeature)mapinfo.Feature);
		}
	}

	private void MapInterfaceControl_PointerPressed(object sender, MapInfoEventArgs e, MainViewModel context) {
		if (e.MapInfo?.WorldPosition == null) return;
		
		if (e.MapInfo.Feature == null) {
			context.PrimaryHook.Place(e.MapInfo.WorldPosition);
		} else {
			context.PrimaryHook.Place((PointFeature)e.MapInfo.Feature);
		}
		Console.WriteLine(context.PrimaryHook.HookedTarget.Point.ToString());
	}
}