using System.Collections.ObjectModel;
using DigiWACS_Client.Models;
using Microsoft.Extensions.Configuration;

namespace DigiWACS_Client.ViewModels;

public class SettingsViewModel : ViewModelBase {
	private SettingsModel _settings;
	public SettingsModel Settings {
		get => _settings;
		set => SetProperty(ref _settings, value);
	}

	public ServerConnectionsModel[] ServerConnections {
		get => _settings.ServerConnections;
	}
	
	
	public SettingsViewModel(SettingsModel settings) {
		Settings = settings;
	}

	public SettingsViewModel() {
		IConfigurationRoot configuration = new ConfigurationBuilder()
		.AddJsonFile("appsettings.json")
		.AddEnvironmentVariables()
		.Build();

		SettingsModel? localMadeSettings = configuration.GetRequiredSection("Settings").Get<SettingsModel>();

		Settings = localMadeSettings;
	}
}