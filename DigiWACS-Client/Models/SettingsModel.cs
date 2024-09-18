namespace DigiWACS_Client.Models;

public sealed class SettingsModel {
	public required int KeyOne { get; set; }
	public required bool KeyTwo { get; set; }
	public required NestedSettings KeyThree { get; set; } = null!;
	public required ServerConnections[] ServerConnections { get; set; }
}

public sealed class NestedSettings {
	public required string Message { get; set; } = null!;
}

public sealed class ServerConnections {
	public required string ConnectionName { get; set; }
	public required string Host { get; set; } = null!;
	public required int Port { get; set; }
	public required string Username { get; set; } = null!;
	public required string Password { get; set; } = null!;
}