using System.Collections.Generic;
using System.Text.Json;

namespace DigiWACS_Client.Models;

public sealed class SettingsModel : ModelBase{
	public required int KeyOne { get; set; }
	public required bool KeyTwo { get; set; }
	public required NestedSettingsModel KeyThree { get; set; } = null!;
	public required List<ServerConnectionsModel> ServerConnections { get; set; }

	public void SaveSettingsToJSON(SettingsModel settings) {
		var json = JsonSerializer.Serialize(settings);
		System.IO.File.WriteAllText("appsettings.json", json);
		
		OnPropertyChanged(nameof(settings));
	}
}

public sealed class NestedSettingsModel {
	public required string Message { get; set; } = null!;
}

public sealed class ServerConnectionsModel {
	public required string ConnectionName { get; set; }
	public required string Host { get; set; }
	public required int Port { get; set; }
	public required string Username { get; set; }
	public required string Password { get; set; }

}