using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Reflection;

namespace DigiWACS.Client {
	public class MapManagement {
<<<<<<< Updated upstream
		public Map InitializeMap() {
			var map = new Map();	
=======
		private IMapControl _mapControl;

		public void MapManagementInitialize( IMapControl mapControl ) {
			_mapControl = mapControl;
			mapControl.Map = CreateMap();
		}

		public static Map CreateMap() {
			var map = new Map();

>>>>>>> Stashed changes
			map.Layers.Add( OpenStreetMap.CreateTileLayer() );
			map.Layers.Add( CreatePointLayer() );
			map.Home = n => n.CenterOnAndZoomTo( map.Layers[ 1 ].Extent!.Centroid, n.Resolutions[ 5 ] );

			return map;
		}

		private MemoryLayer CreatePointLayer() {
			return new MemoryLayer {
				Name = "Points",
				IsMapInfoLayer = true,
				Features = GetCitiesFromEmbeddedResource(),
				Style = CreateBitmapStyle()
			};
		}

		private IEnumerable<IFeature> GetCitiesFromEmbeddedResource() {
			var path = "C:\\Users\\deser\\github\\DigiWACS\\DigiWACS-Client\\Resources\\Cities.json";
			var stream = File.OpenRead( path );
			var cities = DeserializeFromStream<City>( stream );

			return cities.Select( c => {
				var feature = new PointFeature(SphericalMercator.FromLonLat(c.Lng, c.Lat).ToMPoint());
				feature[ "name" ] = c.Name;
				feature[ "country" ] = c.Country;
				return feature;
			} );
		}

		private class City {
			public string Country { get; set; }
			public string Name { get; set; }
			public double Lat { get; set; }
			public double Lng { get; set; }
		}

		public static IEnumerable<T> DeserializeFromStream<T>( Stream stream ) {
			var serializer = new JsonSerializer();

			using ( var sr = new StreamReader( stream ) )
			using ( var jsonTextReader = new JsonTextReader( sr ) ) {
				return serializer.Deserialize<List<T>>( jsonTextReader );
			}
		}

		private static SymbolStyle CreateBitmapStyle() {
			// For this sample we get the bitmap from an embedded resouce
			// but you could get the data stream from the web or anywhere
			// else.
			var path = "C:\\Users\\deser\\github\\DigiWACS\\DigiWACS-Client\\Resources\\home.png"; // Designed by Freepik http://www.freepik.com
			var bitmapId = GetBitmapIdForEmbeddedResource( path );
			var bitmapHeight = 176; // To set the offset correct we need to know the bitmap height
			return new SymbolStyle { BitmapId = bitmapId, SymbolScale = 0.20, SymbolOffset = new Offset( 0, bitmapHeight * 0.5 ) };
		}

		private static int GetBitmapIdForEmbeddedResource( string imagePath ) {
			var image = File.OpenRead( imagePath );
			return BitmapRegistry.Instance.Register( image );
		}
	}
}
