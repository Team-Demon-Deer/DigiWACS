using System.Collections.Generic;

namespace DigiWACS_Client.Models;

public sealed class SettingsModel {
	public required int KeyOne { get; set; }
	public required bool KeyTwo { get; set; }
	public required NestedSettingsModel KeyThree { get; set; } = null!;
	public required List<ServerConnectionsModel> ServerConnections { get; set; }
}

public sealed class NestedSettingsModel {
	public required string Message { get; set; } = null!;
}

public sealed class ServerConnectionsModel(string connectionName, string host, int port, string username, string password) {
	public required string ConnectionName { get; set; } = connectionName;
	public required string Host { get; set; } = host; 
	public required int Port { get; set; } = port;
	public required string Username { get; set; } = username;
	public required string Password { get; set; } = password;

}