﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Fetcher;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;

namespace DigiWACS_Client.Models;

internal sealed class BusPointProviderService : MemoryProvider, IDynamic, IDisposable
{
    public event DataChangedEventHandler DataChanged;

    private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

    public BusPointProviderService()
    {
        Catch.TaskRun(RunTimerAsync);
    }

    private (double Lon, double Lat) _previousCoordinates = (24.945831, 60.192059);

    private async Task RunTimerAsync() {
        while (true) {
            await _timer.WaitForNextTickAsync();
            _previousCoordinates = (_previousCoordinates.Lon + 0.005, _previousCoordinates.Lat - 0.005);
            OnDataChanged();
        }
    }

    void IDynamic.DataHasChanged() {
        OnDataChanged();
    }

    private void OnDataChanged() {
        DataChanged?.Invoke(this, new DataChangedEventArgs());
    }

    public override Task<IEnumerable<IFeature>> GetFeaturesAsync(FetchInfo fetchInfo) {
        var busFeature = new PointFeature(SphericalMercator
            .FromLonLat(_previousCoordinates.Lon, _previousCoordinates.Lat).ToMPoint());
        busFeature["ID"] = "bus";
        return Task.FromResult((IEnumerable<IFeature>) [busFeature]);
    }

    public int i { get; set; }

    public void Dispose()
    {
        _timer.Dispose();
    }
}