using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CoordinateSharp;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts.Extensions;
using ReactiveUI;

namespace DigiWACS_Client.Models;

public class HookModel : ModelBase
{
    public enum HookTypes {
        Primary,
        Secondary
    }
    public HookTypes HookType { get; }

    private PointFeature _hookedTarget;
    public PointFeature HookedTarget {
        get => _hookedTarget;
        private set => SetProperty(ref _hookedTarget, value);
    }

    private Coordinate hookCoordinate;
    public Coordinate HookedCoordinate {
        get => hookCoordinate;
        private set => SetProperty(ref hookCoordinate, value);
    }

    /// <summary>
    /// Not implemented
    /// </summary>
    /// TODO: Implement List of things under the hook location with an envelope.
    public List<PointFeature> HookedTargetsList;
    
    public HookModel(HookTypes hookType) {
        HookType = hookType;
        HookedTarget = new PointFeature(0, 0);
    }
    
    public void Place(PointFeature point) {
        HookedTarget = point;
    }

    public void Place(MPoint point) {
        HookedTarget = new PointFeature(point);
    }

    public void updateTargetPoint(MPoint point) {
        hookCoordinate = WebMercator.ConvertWebMercatortoLatLong(new(point.X, point.Y));
        OnPropertyChanged(nameof(HookedCoordinate));
    }
}