using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using BruTile;
using BruTile.Cache;
using DigiWACS_Client.ViewModels;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Fetcher;
using Mapsui.Layers;
using Mapsui.Nts.Providers.Shapefile;
using Mapsui.Providers;
using Mapsui.Rendering;
using Mapsui.Styles;
using Mapsui.Tiling.Layers;
using Mapsui.Tiling.Provider;

namespace DigiWACS_Client.Services;

public class ShapefileProviderService: MemoryProvider, IDynamic {
	private MainViewModel DataContext { get; } // Only gettable. if DataContext needs to be written, inject it in the method.
	const string _shapeFilePath = "Assets/gshhg-shp-2.3.7/GSHHS_shp/h/GSHHS_h_L1.shp";
	private IPersistentCache<byte[]> LayerCache;
	
	public ShapefileProviderService(MainViewModel injectedDataContext, string shapefilepath = _shapeFilePath) {
		DataContext = injectedDataContext;
		var _shapefile = new ShapeFile(shapefilepath, true, true) { CRS = "EPSG:4326"};
		var _rasterizingTileProvider = new RasterizingTileProvider(shapefileLayer(_shapefile), 
			persistentCache: LayerCache, 
			renderFormat: RenderFormat.Skp);
		var _shapeLayer = new TileLayer(_rasterizingTileProvider);

		injectedDataContext.AreaMap.Layers.Add(_shapeLayer);
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