using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
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

	/*
	public static readonly GenericPluginLoader<ClientPlugin> ClientPluginLoader = new();
	public static List<ClientPlugin> LoadedClientPlugins { get; private set; } =
		ClientPluginLoader.LoadAll(Config.GetConnectionString("PluginsPath")); 
	*/

	public App() {
		Console.WriteLine(Config.GetConnectionString("PluginsPath"));

		string[] pluginPaths = new string[]	{ Config.GetConnectionString("PluginsPath")	};

		IEnumerable<IClientPlugin> commands = pluginPaths.SelectMany( pluginPath =>
		{
			Assembly pluginAssembly = LoadPlugin( pluginPath );
			return CreateCommands( pluginAssembly );
		} ).ToList();
		foreach ( IClientPlugin command in commands ) {
			Console.WriteLine( $"{command.Name}\t - {command.Description}" );
		}
	}

	static Assembly LoadPlugin( string relativePath ) {
		// Navigate up to the solution root
		string root = Path.GetFullPath( Path.Combine(
			Path.GetDirectoryName(
				Path.GetDirectoryName(
					Path.GetDirectoryName(
						Path.GetDirectoryName(
							Path.GetDirectoryName( typeof( App ).Assembly.Location ) ) ) ) ) ) );

		string pluginLocation = Path.GetFullPath( Path.Combine( root, relativePath.Replace( '\\', Path.DirectorySeparatorChar ) ) );
		Console.WriteLine( $"Loading commands from: {pluginLocation}" );
		PluginLoadContext loadContext = new PluginLoadContext( pluginLocation );
		return loadContext.LoadFromAssemblyName( new AssemblyName( Path.GetFileNameWithoutExtension( pluginLocation ) ) );
	}

	static IEnumerable<IClientPlugin> CreateCommands( Assembly assembly ) {
		int count = 0;

		foreach ( Type type in assembly.GetTypes() ) {
			if ( typeof( IClientPlugin ).IsAssignableFrom( type ) ) {
				IClientPlugin result = Activator.CreateInstance( type ) as IClientPlugin;
				if ( result != null ) {
					count++;
					yield return result;
				}
			}
		}

		if ( count == 0 ) {
			string availableTypes = string.Join( ",", assembly.GetTypes().Select( t => t.FullName ) );
			throw new ApplicationException(
				$"Can't find any type which implements ICommand in {assembly} from {assembly.Location}.\n" +
				$"Available types: {availableTypes}" );
		}
	}
}