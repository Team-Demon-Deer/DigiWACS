using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui;
using Mapsui.Layers;

namespace DigiWACS_Client.Models;

public class HookModel : ModelBase
{
    public enum HookTypes {
        Primary,
        Secondary
    }
    public HookTypes HookType { get; }

    public PointFeature HookedTarget { get; private set; }

    /// <summary>
    /// Not implemented
    /// </summary>
    /// TODO: Implement List of things under the hook location with an envelope.
    public List<PointFeature> HookedTargetsList;
    
    public HookModel(HookTypes hookType) {
        HookType = hookType;
        HookedTarget = new PointFeature(0, 0);
        OnPropertyChanged(nameof(HookedTarget));
    }
    
    public void Place(PointFeature point) {
        HookedTarget = point;
        OnPropertyChanged(nameof(HookedTarget));
    }

    public void Place(MPoint point) {
        HookedTarget = new PointFeature(point);
        OnPropertyChanged(nameof(HookedTarget));
    }
}