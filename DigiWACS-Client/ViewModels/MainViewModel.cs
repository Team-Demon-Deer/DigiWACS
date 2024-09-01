using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using DigiWACS_Client.Models;
using DigiWACS_Client.Services;
using DynamicData;
using Mapsui;
using Mapsui.Animations;
using Mapsui.Layers;
using Mapsui.Layers.AnimatedLayers;
using Mapsui.Limiting;
using Mapsui.Styles;
using Mapsui.Styles.Thematics;
using Mapsui.Utilities;
using ReactiveUI;
using SkiaSharp;
using Color = Mapsui.Styles.Color;

namespace DigiWACS_Client.ViewModels;

public partial class MainViewModel : ViewModelBase {
	//public string Greeting => "Welcome to Avalonia!";

	public Map AreaMap;

	private HookProviderService _hookProviderService;
	public HookProviderService HookProviderService
	{
		get => _hookProviderService;
		set => SetProperty(ref _hookProviderService, value);
	}
	
	private HookModel _primaryHook;
	public HookModel PrimaryHook {
		get => _primaryHook;
		set => SetProperty(ref _primaryHook, value);
	}
	
	public HookModel SecondaryHook { get; set; }
	public ObservableCollection<HookModel> HookModelObservableCollection { get; set; }

	/// <summary>
	/// ViewModel Constructor
	/// </summary>
	public MainViewModel() {
		AreaMap = new Map();
		HookModelObservableCollection = new();
		HookModelObservableCollection.AddRange([
			PrimaryHook =  new HookModel(HookModel.HookTypes.Primary), 
			//	SecondaryHook = new HookModel(HookModel.HookTypes.Secondary),
		]);
		
		HookProviderService = new HookProviderService(this);
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
				PrimaryHook.Place(e.MapInfo.WorldPosition);
			} else {
				PrimaryHook.Place((PointFeature)e.MapInfo.Feature);
			}
			Console.WriteLine(PrimaryHook.HookedTarget.Point.ToString());
		};
	}

	private void InitializeMapsuiCustomization(Map aMap, Dictionary<string, int> assetDictionary)
	{
		AreaMap.BackColor = Color.Black;
		//AreaMap.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
		var AnimatedHookLayer = new AnimatedPointLayer(HookProviderService)
		{
			Easing = Easing.Linear,
			AnimationDuration = 1
		};
		AnimatedHookLayer.Style = new StyleCollection() { Styles = { new SymbolStyle() { BitmapId = assetDictionary["DigiWACS_Client.Assets.PrimaryHook.svg"], SymbolScale = .25, Opacity = 0.5f }}};

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