using DigiWACS_Client.Services;
using Mapsui;

namespace DigiWACS_Client.Models;

public interface IMapInterface {
	
	public Map AreaMap { get; set; }
	public ShapefileProviderService ShapefileProviderService { get; set; }
	public HookProviderService HookProviderService { get; set; }
}