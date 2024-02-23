using DigiWACS.PluginBase;
using System.Collections.Generic;
using System.Diagnostics;
using DigiWACS.Client.Properties;
using System.ComponentModel.Composition.Hosting;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Linq;
using System.IO;

namespace DigiWACS.Client.Systems;
internal class PluginManagement
{
	private Plugins _settings;
	internal CompositionContainer compositionContainer;

	// This uses the Managed Exstensibility Framework (MEF)
	// https://learn.microsoft.com/en-us/dotnet/framework/mef/
	// When tryin to find documentation, a good starting point is:
	// https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.composition
	// Enjoy some 2008 MEF goodness.
	// https://learn.microsoft.com/en-us/events/pdc-pdc08/tl33
	[ImportMany]
	public IEnumerable<Lazy<IDigiWACSPlugin, IDigiWACSPluginMetadata>> allPossiblePlugins;

	internal AggregateCatalog allPossiblePluginsCatalog = new AggregateCatalog();

	internal AggregateCatalog enabledPluginCatalog = new AggregateCatalog();
	internal CompositionBatch compositionBatch = new CompositionBatch();

	public PluginManagement(Plugins configurationSection)
	{
		Trace.WriteLine("PluginManagement()");
		_settings = configurationSection;

		SearchForAllPluginsAndAddToCatalog(_settings.pluginpaths);
		enabledPluginCatalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
		EnablePlugin();

		compositionContainer = new CompositionContainer(enabledPluginCatalog);
		compositionContainer.Compose(compositionBatch);
	}

	internal void EnablePlugin()
	{
		//future logic to only select which ones are enabled
		enabledPluginCatalog = allPossiblePluginsCatalog;
		//seudo code: return plugins.where( p => p.hash == RequestedHash).FirstOrDefault();
	}

	internal void SearchForAllPluginsAndAddToCatalog(List<string> PathsToSearch)
	{
		//Recersive Directory Search
		foreach (string pathToSearch in PathsToSearch)
		{
			string[] subpaths = Directory.GetDirectories(pathToSearch, "*", SearchOption.AllDirectories);
			foreach (string subpath in subpaths)
			{
				allPossiblePluginsCatalog.Catalogs.Add(new DirectoryCatalog(subpath));
			}
		}

		Debug.WriteLine($"Plugins added to Catalog: {allPossiblePluginsCatalog.Count()}");
	}
}