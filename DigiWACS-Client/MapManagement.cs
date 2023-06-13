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
using Mapsui.Layers.AnimatedLayers;
using System.Drawing;
using Mapsui.Styles.Thematics;
using System.Resources;

namespace DigiWACS.Client {
	public class MapManagement {

		public Map InitializeMap() {
        	        var image = File.OpenRead("C:\\ProgramData\\DigiWACS\\DigiWACS-Client\\Resources\\home.png");
	                int bitmapid = BitmapRegistry.Instance.Register(image);
			var map = new Map();
			map!.Layers.Add( OpenStreetMap.CreateTileLayer() );
			map.Layers.Add( CreateAnimatedPointLayer(new ThemeStyle ( f => { return CreateBitmapStyle(f, bitmapid); })));
			 map.Layers.Add(CreateAnimatedPointLayer(new LabelStyle {
                                ForeColor = Mapsui.Styles.Color.Black,
                                BackColor = new Mapsui.Styles.Brush(Mapsui.Styles.Color.Transparent),
                                LabelColumn = "name",
                        }));

			map.Home = n => n.CenterOnAndZoomTo(SphericalMercator.FromLonLat(37.359, 45.006).ToMPoint(), n.Resolutions[ 12 ] );
			return map;
		}

        private static ILayer CreateAnimatedPointLayer(Style s)
        {
            return new AnimatedPointLayer(new UnitTracksProvider())
            {
                Name = "Animated Points",
                Style = s
        };
        }
		private static SymbolStyle CreateBitmapStyle(IFeature feature, int bitmapid) {
			// For this sample we get the bitmap from an embedded resouce
			// but you could get the data stream from the web or anywhere
			// else.
			var bitmapHeight = 176; // To set the offset correct we need to know the bitmap height
			return new SymbolStyle { BitmapId = bitmapid, SymbolScale = 0.20, SymbolOffset = new Offset( 0, bitmapHeight * 0.5 ) ,
                SymbolRotation = (double)feature["rotation"]!
            };
		}
	}
}
