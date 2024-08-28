using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using DigiWACS_Client.Models;
using Mapsui;
using Mapsui.Animations;
using Mapsui.Layers;
using Mapsui.Limiting;
using Mapsui.Styles;
using Mapsui.Styles.Thematics;
using Mapsui.Utilities;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;
using ReactiveUI;
using SkiaSharp;

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
		AreaMap.BackColor = Color.Black;
		HookLayer = new ObservableMemoryLayer<Coordinate>(
			(Coordinate c) => new PointFeature(c.X, c.Y)
		)
		{
			Style = SymbolStyles.CreatePinStyle(),
			ObservableCollection = (new TrackMock()).items
		};
		var a = this;
		AreaMap.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
		SKPicture s = SvgHelper.LoadSvgPicture((Stream)(new MilitarySymbolConverter()).Convert(null, null, null, null));
		var i = BitmapRegistry.Instance.Register((object)s, "symbology");
		AreaMap.Layers.Add((new Mapsui.Layers.AnimatedLayers.AnimatedPointLayer(new BusPointProvider())
		{
			IsMapInfoLayer = true,
			Easing = Easing.Linear,
			Style = new ThemeStyle(f =>
			{
				if (f["selected"]?.ToString() == "true")

					return new StyleCollection
					{
						Styles = {
							// With the StyleCollection you can use the same symbol as when not selected but 
							// put something in the background to indicate it is selected.
							new SymbolStyle { Fill = new Brush(Color.Cyan) { FillStyle = FillStyle.Hollow}, SymbolScale = 1.2 },
							new SymbolStyle() {
								SymbolType = SymbolType.Image,
								BitmapId = i
							}
						}
					};

				return new SymbolStyle() {
					SymbolType = SymbolType.Image,
					BitmapId = i
				};
				
			}) 
		}));
		;
		AreaMap.Navigator.Limiter = new ViewportLimiterKeepWithinExtent();
		AreaMap.Navigator.OverridePanBounds = AreaMap.Extent;
		AreaMap.Home = n => n.ZoomToBox(AreaMap.Extent);
		AreaMap.Layers.Add(HookLayer);
		AreaMap.Info += (s, e) =>
		{
			var feature = e.MapInfo?.Feature;
			if (feature is not null) {
				if (feature["selected"] is null) feature["selected"] = "true";
				else feature["selected"] = null;
			};
			
			HookControl.PlacePrimaryHook(s, e, ref a);
			//MapControl.Refresh();
		};
		
	}
}