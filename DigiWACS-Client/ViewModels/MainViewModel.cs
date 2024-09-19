using System;
using System.Collections.ObjectModel;
using DigiWACS_Client.Controls;
using DigiWACS_Client.Models;
using Microsoft.Extensions.Configuration;

namespace DigiWACS_Client.ViewModels;

public partial class MainViewModel : ViewModelBase {
	//public string Greeting => "Welcome to Avalonia!";
	
	private SettingsModel _settings;
	public SettingsModel Settings {
		get => _settings;
		set => SetProperty(ref _settings, value); 
	}
	
	public IMapInterface MapInterface;
	
	private HookModel _primaryHook;
	public HookModel PrimaryHook {
		get => _primaryHook;
		set => SetProperty(ref _primaryHook, value);
	}
	
	public HookModel SecondaryHook { get; set; }

	/// <summary>
	/// ViewModel Constructor
	/// </summary>
	public MainViewModel(SettingsModel settings) {
		Settings = settings;
		PrimaryHook = new HookModel(HookModel.HookTypes.Primary);
		SecondaryHook = new HookModel(HookModel.HookTypes.Secondary);

		MapInterface = new MapsuiWrapper(this);
			//new HomeBrewMapWrapper(this);
	}
	public MainViewModel() { 
		IConfigurationRoot configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.AddEnvironmentVariables()
			.Build();

		SettingsModel? settings = configuration.GetRequiredSection("Settings").Get<SettingsModel>();
		
		Settings = settings;
		PrimaryHook = new HookModel(HookModel.HookTypes.Primary);
		SecondaryHook = new HookModel(HookModel.HookTypes.Secondary);

		MapInterface = new MapsuiWrapper(this);
	}

	public void DrawBraaline() {
		MapInterface.DrawBRAALine(SecondaryHook, PrimaryHook);
	}
	
	public void ClearBraaline() {
		MapInterface.ClearBRAALine();
	}
}