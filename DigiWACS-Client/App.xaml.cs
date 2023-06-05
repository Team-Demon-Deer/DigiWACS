using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using DigiWACS.PluginBase;
using Microsoft.Extensions.Configuration;

namespace DigiWACS.Client;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
	public static IConfigurationRoot Config = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddEnvironmentVariables()
		.AddJsonFile("appsettings.json")
		.AddUserSecrets(Assembly.GetExecutingAssembly(), true) //must be last in builder so it overrides appsettings.json
		.Build();

	public static readonly GenericPluginLoader<ClientPlugin> ClientPluginLoader = new();
	public static List<ClientPlugin> LoadedClientPlugins { get; private set; } =
		ClientPluginLoader.LoadAll(Config.GetConnectionString("PluginsPath"));
	
	public App() {
		Trace.WriteLine(Config.GetConnectionString("PluginsPath"));
	}
}