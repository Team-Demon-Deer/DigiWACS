using System;
using System.Collections.Generic;
using System.IO;
using DigiWACS_Client.Services;
using DigiWACS_Client.ViewModels;
using Mapsui;
using Mapsui.Animations;
using Mapsui.Layers;
using Mapsui.Layers.AnimatedLayers;
using Mapsui.Limiting;
using Mapsui.Styles;
using Mapsui.Styles.Thematics;
using Mapsui.Utilities;
using SkiaSharp;

namespace DigiWACS_Client.Models;

public class MapsuiWrapper : IMapInterface {
	
	public Map AreaMap { get; set; }
	public ShapefileProviderService ShapefileProviderService { get; set; }
	public HookProviderService HookProviderService { get; set; }
	
	public MapsuiWrapper(MainViewModel mainViewModel) {
		AreaMap = new Map(){ CRS = "EPSG:4326"};
		
		ShapefileProviderService = new ShapefileProviderService(AreaMap);
		HookProviderService = new HookProviderService(mainViewModel.PrimaryHook, mainViewModel.SecondaryHook);
		
		var assetDictionary = AssetManagerService.Initialize(); //Load the DigiWACS exclusive assets
		SKPicture s = SvgHelper.LoadSvgPicture((Stream)(MilitarySymbolConverter.Convert(10000100001101000408)));
		var i = BitmapRegistry.Instance.Register((object)s, "symbology");
		assetDictionary.Add("symbology", i);
		InitializeMapsuiCustomization(AreaMap, assetDictionary);
		
		// Delegate Event
		AreaMap.Info += (s, e) =>
		{
			if (e.MapInfo?.WorldPosition == null) return;
		
			if (e.MapInfo.Feature == null) {
				mainViewModel.PrimaryHook.Place(e.MapInfo.WorldPosition);
			} else {
				mainViewModel.PrimaryHook.Place((PointFeature)e.MapInfo.Feature);
			}
			Console.WriteLine(mainViewModel.PrimaryHook.HookedTarget.Point.ToString());
		};
	}

	
	
	private void InitializeMapsuiCustomization(Map aMap, Dictionary<string, int> assetDictionary) {
		AreaMap.BackColor = Color.Black;
		var openStreeMapLayer = Mapsui.Tiling.OpenStreetMap.CreateTileLayer();
		//AreaMap.Layers.Add(openStreeMapLayer);
		var AnimatedHookLayer = new AnimatedPointLayer(HookProviderService)
		{
			Easing = Easing.Linear,
			AnimationDuration = 1
		};
		AnimatedHookLayer.Style = new StyleCollection() { Styles = { new SymbolStyle() { BitmapId = assetDictionary["PrimaryHook"], SymbolScale = .25, Opacity = 0.5f }}};

		AreaMap.Layers.Add(AnimatedHookLayer);
		AreaMap.Layers.Add((new AnimatedPointLayer(new BusPointProviderService())
		{
			IsMapInfoLayer = true,
			Easing = Easing.Linear,
			Style = new ThemeStyle(f =>
			{
				return new SymbolStyle() {
					SymbolType = SymbolType.Image,
					BitmapId = assetDictionary["symbology"]
				};
				
			}) 
		}));
		
		// View Area Limitations
		AreaMap.Navigator.Limiter = new ViewportLimiterKeepWithinExtent();
		AreaMap.Navigator.OverridePanBounds = AreaMap.Extent;
		AreaMap.Home = n => n.ZoomToBox(AreaMap.Extent);
	}
}