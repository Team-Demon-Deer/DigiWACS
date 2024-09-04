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
	public MainViewModel() {
		PrimaryHook = new HookModel(HookModel.HookTypes.Primary);
		SecondaryHook = new HookModel(HookModel.HookTypes.Secondary);
		
		MapInterface =  new MapsuiWrapper(this);
	}
}