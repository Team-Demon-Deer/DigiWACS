using Avalonia.Controls;
using DigiWACS_Client.Controls;
using DigiWACS_Client.ViewModels;

namespace DigiWACS_Client.Models;

public class HomeBrewMapWrapper : IMapInterface {
	public UserControl MapInterfaceControl { get; set; }
	
	public HomeBrewMapWrapper(MainViewModel mainViewModel) {
		MapInterfaceControl = new CustomDrawingExampleControl();
	}

}