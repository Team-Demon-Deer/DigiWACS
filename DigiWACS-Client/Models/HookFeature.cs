using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	
	public HookFeature(PointFeature pointFeature, HookTypes hookType = HookTypes.Primary) : base(pointFeature)
	{
		HookTarget = pointFeature;
		HookType = hookType;
		this["ID"] = hookType;
	}

	public HookFeature(MPoint point, HookTypes hookType = HookTypes.Primary) : base(point)
	{
		HookTarget = new PointFeature(point);
		HookType = hookType;
		this["ID"] = hookType;
	}

	public HookFeature(double x, double y, HookTypes hookType = HookTypes.Primary) : base(x, y)
	{
		HookTarget = new PointFeature(x, y);
		HookType = hookType;
		this["ID"] = hookType;
	}
}