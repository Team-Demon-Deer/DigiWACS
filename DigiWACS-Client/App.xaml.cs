﻿using System;
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
		.AddJsonFile( "Properties/appsettings.json" )
		.AddUserSecrets(Assembly.GetExecutingAssembly(), true) //must be last in builder so it overrides appsettings.json
		.Build();

	public static readonly GenericPluginLoader<IDigiWACSPlugin> ClientPluginLoader = new();
	public static List<IDigiWACSPlugin> LoadedClientPlugins { get; private set; } =
		ClientPluginLoader.LoadAll(Config.GetConnectionString("PluginsPath"));


	public App() {
		Console.WriteLine(Config.GetConnectionString("PluginsPath"));

		//new PluginManagement().PluginLoader( Config.GetConnectionString( "PluginsPath" ) );
	}
}