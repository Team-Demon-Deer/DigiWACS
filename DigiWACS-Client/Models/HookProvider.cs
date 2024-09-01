using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DigiWACS_Client.ViewModels;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Fetcher;
using Mapsui.Layers;
using Mapsui.Providers;

namespace DigiWACS_Client.Models;

public class HookProvider : MemoryProvider, IDynamic, IDisposable
{
	public HookFeature PrimaryHook { get; set; }
	public HookFeature? SecondaryHook { get; set; }
	
	public HookProvider()
	{
		PrimaryHook = PrimaryHookDefinition(new HookFeature(0, 0));
		Catch.TaskRun(RunTimerAsync);
	}

	private async Task RunTimerAsync() {
		while (true) {
			await _timer.WaitForNextTickAsync();
			OnDataChanged();
		}
	}
	
	public void PlacePrimaryHook(object? s, MapInfoEventArgs e, MainViewModel dContext) {
		if (e.MapInfo?.WorldPosition == null) return;
		
		if (e.MapInfo.Feature == null) {
			PrimaryHook.HookTarget = new PointFeature(e.MapInfo.WorldPosition);
		} else {
			PrimaryHook.HookTarget = ((PointFeature)e.MapInfo.Feature);
		}
		
		Debug.WriteLine(PrimaryHook.Point.ToString());
		OnDataChanged();
	}

	public override Task<IEnumerable<IFeature>> GetFeaturesAsync(FetchInfo fetchInfo) {

		return Task.FromResult((IEnumerable<IFeature>) [PrimaryHookDefinition(new HookFeature(PrimaryHook.HookTarget))]);
	}

	private static HookFeature PrimaryHookDefinition(HookFeature hookFeature)
	{
		hookFeature.HookType = HookFeature.HookTypes.Primary;
		hookFeature["ID"] = "primaryHook"; 
		return hookFeature;
	}

	private static HookFeature SecondaryHookDefinition(HookFeature hookFeature)
	{
		hookFeature.HookType = HookFeature.HookTypes.Secondary;
		hookFeature["ID"] = "secondaryHook";
		return hookFeature;
	}
	
	private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromSeconds(0.01));

	// Implementing the Required things for MemoryProvider, IDynamic, & IDisposable
	public event DataChangedEventHandler? DataChanged;
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