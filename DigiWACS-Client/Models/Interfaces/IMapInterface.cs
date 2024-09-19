using Avalonia.Controls;

namespace DigiWACS_Client.Models;

public interface IMapInterface {

	public UserControl MapInterfaceControl { get; set; }
	
	public void DrawBRAALine(HookModel from, HookModel to);
	public void ClearBRAALine();
}