using Avalonia.Controls;
using DigiWACS_Client.Services;
using Mapsui;

namespace DigiWACS_Client.Models;

public interface IMapInterface {

	public UserControl MapInterfaceControl { get; set; }
	
	// Todo: Make this an agnostic type or something. Its required by MainView to be able to switch between the two.
	public Map AreaMap { get; set; }
	public ShapefileProviderService ShapefileProviderService { get; set; }
	public HookProviderService HookProviderService { get; set; }
}