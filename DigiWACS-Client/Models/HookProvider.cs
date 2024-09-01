using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DigiWACS_Client.ViewModels;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Fetcher;
using Mapsui.Layers;
using Mapsui.Providers;

namespace DigiWACS_Client.Models;

public class HookProvider : MemoryProvider, IDynamic, IDisposable, INotifyPropertyChanged
{
	private HookFeature primaryHook;

	public HookFeature PrimaryHook
	{
		get => primaryHook;
		set {
			if (primaryHook != value)
			{
				primaryHook = value;
				OnPropertyChanged();
			}
		}
	}
	
	public HookFeature? SecondaryHook { get; set; }
	
	public HookProvider(MainViewModel dContext)
	{

		
		
		PrimaryHook = new HookFeature(0, 0);
		
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
		

		Console.WriteLine(PrimaryHook.HookTarget.Point.ToString());
		OnDataChanged();
	}

	public override Task<IEnumerable<IFeature>> GetFeaturesAsync(FetchInfo fetchInfo) {

		return Task.FromResult((IEnumerable<IFeature>) [ new HookFeature(PrimaryHook.HookTarget)]);
	}

	public HookFeature GetHook(HookFeature.HookTypes HookType)
	{
		switch (HookType)
		{
			case HookFeature.HookTypes.Primary:
				return PrimaryHook;
			case HookFeature.HookTypes.Secondary:
				return SecondaryHook;
		}
		return null;
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
	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(field, value)) return false;
		field = value;
		OnPropertyChanged(propertyName);
		return true;
	}
	public void Dispose() {
		_timer.Dispose();
	}

}