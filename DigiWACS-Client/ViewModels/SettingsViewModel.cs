using System.Collections.Generic;
using System.Collections.ObjectModel;
using DigiWACS_Client.Models;
using Microsoft.Extensions.Configuration;

namespace DigiWACS_Client.ViewModels;

public class SettingsViewModel : ViewModelBase {
	public SettingsModel Settings { get; set; }

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