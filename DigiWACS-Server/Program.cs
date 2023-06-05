using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Channels;
using DigiWACS.PluginBase;
using DigiWACS.Server.Services;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace DigiWACS.Server;

static class Program {
	public static IConfigurationRoot Config = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddEnvironmentVariables()
		.AddJsonFile("appsettings.json")
		.AddUserSecrets(Assembly.GetExecutingAssembly(), true) //must be last in builder so it overrides appsettings.json
		.Build();

	public static readonly GenericPluginLoader<ServerPlugin> ServerPluginLoader = new();
	public static List<ServerPlugin> LoadedServerPlugins { get; private set; } =
		ServerPluginLoader.LoadAll(Config.GetConnectionString("PluginsPath"));

	static void Main(string[] args) {
		Console.ReadLine();
	}
}