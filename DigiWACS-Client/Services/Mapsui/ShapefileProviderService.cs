﻿using DigiWACS_Client.ViewModels;
using Mapsui;
using Mapsui.Fetcher;
using Mapsui.Layers;
using Mapsui.Nts.Providers.Shapefile;
using Mapsui.Providers;
using Mapsui.Rendering;
using Mapsui.Styles;
using Mapsui.Tiling.Layers;
using Mapsui.Tiling.Provider;

namespace DigiWACS_Client.Services.Mapsui;

public class ShapefileProviderService: MemoryProvider, IDynamic {
	private MainViewModel DataContext { get; } // Only gettable. if DataContext needs to be written, inject it in the method.
	const string _shapeFilePath = "Assets/gshhg-shp-2.3.7/GSHHS_shp/c/GSHHS_c_L1.shp";
	
	public ShapefileProviderService(Map areaMap, string shapefilepath = _shapeFilePath) {
		var _shapefile = new ShapeFile(shapefilepath, true, true) { CRS = "EPSG:4326"};
		var _rasterizingTileProvider = new RasterizingTileProvider(shapefileLayer(_shapefile),
			renderFormat: RenderFormat.Skp);
		var _shapeLayer = new TileLayer(_rasterizingTileProvider);

		areaMap.Layers.Add(_shapeLayer);
	}
	
	private static ILayer shapefileLayer(IProvider shapefileProvider) {
		return new Layer {
			Name = "Shapefile",
			DataSource = shapefileProvider,
			Style = new VectorStyle {
				Fill = new Brush(color: Color.Black),
				Line = new Pen(color: Color.White)
			}
		};
	}

	// Implementing the Boiler plate Required things for MemoryProvider, IDynamic, IDisposable, & INotifyPropertyChanged
	public event DataChangedEventHandler? DataChanged;
	void IDynamic.DataHasChanged() {
		OnDataChanged();
	}
	public void OnDataChanged() {
		DataChanged?.Invoke(this, new DataChangedEventArgs());
	}
}