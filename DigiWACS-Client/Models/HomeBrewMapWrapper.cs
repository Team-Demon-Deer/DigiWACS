using Avalonia.Controls;
using CoordinateSharp;
using DigiWACS_Client.Controls;
using DigiWACS_Client.ViewModels;

namespace DigiWACS_Client.Models;

public class HomeBrewMapWrapper : IMapInterface {
	public UserControl MapInterfaceControl { get; set; }


	public HomeBrewMapWrapper(MainViewModel mainViewModel) {
		MapInterfaceControl = new CustomDrawingExampleControl();
	}
	
	public void DrawBRAALine(HookModel from, HookModel to) {
		throw new System.NotImplementedException();
	}
	public void ClearBRAALine() {
		throw new System.NotImplementedException();
	}
}