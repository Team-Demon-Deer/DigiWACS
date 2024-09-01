using Mapsui;
using Mapsui.Layers;

namespace DigiWACS_Client.Models;

public class HookFeature : PointFeature {
	public enum HookTypes {
		Primary,
		Secondary
	}
	public HookTypes HookType { get; set; }
	public PointFeature HookTarget;
	public bool Visible;
	
	public HookFeature(PointFeature pointFeature) : base(pointFeature)
	{
		HookTarget = pointFeature;
	}

	public HookFeature(MPoint point) : base(point)
	{
		HookTarget = new PointFeature(point);
	}

	public HookFeature(double x, double y) : base(x, y)
	{
		HookTarget = new PointFeature(x, y);
	}
}