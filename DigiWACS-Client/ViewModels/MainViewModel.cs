using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using DigiWACS_Client.Models;
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
	
	/*
	private PointFeature? _hookPrimary;
	public PointFeature HookPrimary
	{
		get => _hookPrimary;
		set => this.RaiseAndSetIfChanged(ref _hookPrimary, value);
	}
	
	private PointFeature? _hookSecondary;
	public PointFeature HookSecondary
	{
		get => _hookSecondary;
		set => this.RaiseAndSetIfChanged(ref _hookSecondary, value);
	}
	*/

	private HookProvider _hookProvider;
	public HookProvider HookProvider
	{
		get => _hookProvider;
		set => this.RaiseAndSetIfChanged(ref _hookProvider, value);
	}

	public HookFeature HookPrimary
	{
		get => _hookProvider.PrimaryHook;
	}
	public HookFeature HookSecondary
	{
		get => _hookProvider.SecondaryHook;
	}


	/// <summary>
	/// ViewModel Constructor
	/// </summary>
	public MainViewModel() {
		AreaMap = new Map();
		HookProvider = new HookProvider();
		var assetDictionary = AssetManager.Initialize(); //Load the DigiWACS exclusive assets
		SKPicture s = SvgHelper.LoadSvgPicture((Stream)(MilitarySymbolConverter.Convert(10000100001101000408)));
		var i = BitmapRegistry.Instance.Register((object)s, "symbology");
		assetDictionary.Add("symbology", i);
		InitializeMapsuiCustomization(AreaMap, assetDictionary);
		
		// Delegate Event
		AreaMap.Info += (s, e) =>
		{
			HookProvider.PlacePrimaryHook(s, e, this);
			
		};
	}

	private void InitializeMapsuiCustomization(Map aMap, Dictionary<string, int> assetDictionary)
	{
		AreaMap.BackColor = Color.Black;
		AreaMap.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
		var AnimatedHookLayer = new AnimatedPointLayer(HookProvider)
		{
			Easing = Easing.Linear,
			AnimationDuration = 1
		};
		AnimatedHookLayer.Style = new StyleCollection() { Styles = { new SymbolStyle() { BitmapId = assetDictionary["PrimaryHook"], SymbolScale = .25, Opacity = 0.5f }}};
			// new ThemeStyle(_feature =>
		// {
		// 	SymbolStyle style = new();
		// 	switch (((HookFeature)_feature).HookType)
		// 	{
		// 		case HookFeature.HookTypes.Primary:
		// 			style.BitmapId = assetDictionary["PrimaryHook"];
		// 			break;
		// 		case HookFeature.HookTypes.Secondary:
		// 			style.BitmapId = assetDictionary["SecondaryHook"];
		// 			break;
		//
		// 		default:
		// 			break;
		// 	}
		// 	return style;
		// });

		AreaMap.Layers.Add(AnimatedHookLayer);
		AreaMap.Layers.Add((new AnimatedPointLayer(new BusPointProvider())
		{
			IsMapInfoLayer = true,
			Easing = Easing.Linear,
			Style = new ThemeStyle(f =>
			{
				// if (f == HookPrimary)
				//
				// 	return new StyleCollection
				// 	{
				// 		Styles = {
				// 			// With the StyleCollection you can use the same symbol as when not selected but 
				// 			// put something in the background to indicate it is selected.
				// 			// new SymbolStyle { Fill = new Brush(Color.Cyan) { FillStyle = FillStyle.Hollow}, SymbolScale = 1.2 },
				// 			new SymbolStyle() {
				// 				SymbolType = SymbolType.Image,
				// 				BitmapId = i
				// 			}
				// 		}
				// 	};

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