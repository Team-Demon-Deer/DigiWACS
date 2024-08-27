using System.Diagnostics;
using DigiWACS_Client.ViewModels;
using Mapsui;
using Mapsui.Nts;
using NetTopologySuite.Geometries;

namespace DigiWACS_Client.Models;

public class HookControl
{
	public static void PlacePrimaryHook(object? s, MapInfoEventArgs e, ref MainWindowViewModel dContext)
	{
		if (e.MapInfo?.WorldPosition == null) return;

		//Debug.Print(new Point(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y).ToString());

		dContext.HookPrimary = new Coordinate(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y);

		Debug.Print(dContext.HookPrimary.ToString());
		
		// Add a point to the layer using the Info position
		dContext.HookLayer?.Features.Clear();

		dContext.HookLayer?.Features.Add(new GeometryFeature
		{
			Geometry = new Point(e.MapInfo.WorldPosition.X, e.MapInfo.WorldPosition.Y)
		}); 

		// To notify the map that a redraw is needed.
		dContext.HookLayer?.DataHasChanged();
	}
}