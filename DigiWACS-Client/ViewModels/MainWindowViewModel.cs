using Mapsui.UI.Avalonia;

namespace DigiWACS_Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
	public string Greeting => "Welcome to Avalonia!";
	
	public static MapControl MapControl { get; }= new Mapsui.UI.Avalonia.MapControl();
#pragma warning restore CA1822 // Mark members as static
}