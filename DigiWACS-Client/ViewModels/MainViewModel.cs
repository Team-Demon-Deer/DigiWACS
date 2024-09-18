using System;
using DigiWACS_Client.Controls;
using DigiWACS_Client.Models;

namespace DigiWACS_Client.ViewModels;

public partial class MainViewModel : ViewModelBase {
	//public string Greeting => "Welcome to Avalonia!";

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
	public MainViewModel(Settings settings) {
		PrimaryHook = new HookModel(HookModel.HookTypes.Primary);
		SecondaryHook = new HookModel(HookModel.HookTypes.Secondary);

		MapInterface = new HomeBrewMapWrapper(this);
		//new MapsuiWrapper(this);
		
		Console.WriteLine($"KeyOne = {settings?.KeyOne}");
		Console.WriteLine($"KeyTwo = {settings?.KeyTwo}");
		Console.WriteLine($"KeyThree:Message = {settings?.KeyThree?.Message}");
	}
}