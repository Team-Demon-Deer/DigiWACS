using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using DigiWACS.PluginBase;
using Microsoft.Extensions.Configuration;

namespace DigiWACS.Client;
/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
	public static IConfigurationRoot Config = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettings.json")
		.Build();

	public static readonly GenericPluginLoader<ClientPlugin> ClientPluginLoader = new GenericPluginLoader<ClientPlugin>();

	public static List<ClientPlugin> LoadedClientPlugins { get; private set; } =
		ClientPluginLoader.LoadAll(Config.GetConnectionString("PluginsPath"));

	public App() {
	}
	
}