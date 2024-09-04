using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using DigiWACS_Client.Models;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Fetcher;
using Mapsui.Layers;
using Mapsui.Providers;

namespace DigiWACS_Client.Services.Mapsui;

public class HookProviderService : MemoryProvider, IDynamic, IDisposable, INotifyPropertyChanged {

    private HookModel Primary {get; set; }
    private HookModel Secondary { get; set; }
    HookModel[] HookArray { get; set; }
    
    public HookProviderService(HookModel Primary, HookModel Secondary) {
        HookArray = [ Primary, Secondary ];
        
        Catch.TaskRun(RunTimerAsync);
    }

    private async Task RunTimerAsync() {
        while (true) {
            await _timer.WaitForNextTickAsync();
            OnDataChanged();
        }
    }
    
    public override Task<IEnumerable<IFeature>> GetFeaturesAsync(FetchInfo fetchInfo)
    {
        Collection<IFeature> features = new();
        foreach (var hook in HookArray) {
            features.Add(new PointFeature(hook.HookedTarget) {["ID"] = hook.HookType});
            hook.updateTargetPoint(hook.HookedTarget.Point);
        }
        return Task.FromResult((IEnumerable<IFeature>) features);
    }

    // Sets the refresh rate of the HookProvider
    private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromSeconds(0.01));

    // Implementing the Boiler plate Required things for MemoryProvider, IDynamic, IDisposable, & INotifyPropertyChanged
    public event DataChangedEventHandler? DataChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    void IDynamic.DataHasChanged() {
        OnDataChanged();
    }
    public void OnDataChanged() {
        DataChanged?.Invoke(this, new DataChangedEventArgs());
    }
    public void Dispose() {
        _timer.Dispose();
    }
}