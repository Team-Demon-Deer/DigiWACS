using System.Diagnostics;
using System.IO;
using System.Windows;
using DigiWACS.PluginBase;
using Microsoft.Extensions.Configuration;

namespace DigiWACS.Client;

public partial class PluginWindow : Window {
	static IConfiguration config = new ConfigurationBuilder()
		.AddJsonFile("appsettings.json")
		.AddEnvironmentVariables()
		.Build();

	public AppSettings DigiWACSSettings = config.GetRequiredSection("DigiWACS-Client").Get<AppSettings>();

	
	public PluginWindow() {
		InitializeComponent();
		Trace.WriteLine($"DigiWACSSettings.PluginsPath : {DigiWACSSettings.PluginsPath}");
		var pluginLoader = new GenericPluginLoader<ClientPlugin>();
		var loadedTestPlugin =
			pluginLoader.LoadAll(Path.GetFullPath(DigiWACSSettings.PluginsPath));
		Trace.WriteLine($"Loaded {loadedTestPlugin.Count} Plugin(s)");

		loadedTestPlugin[0].OnPluginLoad();

		pluginLoader.UnloadAll();
	}
}