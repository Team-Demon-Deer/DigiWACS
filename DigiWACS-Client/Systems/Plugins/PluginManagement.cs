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

namespace DigiWACS.Client {
	internal class PluginManagement {

		[ImportMany]
		public IEnumerable<Lazy<IDigiWACSPlugin, IDigiWACSPluginMetadata>> allPossiblePlugins;

		public IEnumerable<IDigiWACSPlugin> LoadedPlugins;

		private Plugins _settings;
		/*
		 * We want to try and load all the plugins that we remember in Appsettings.json,
		 * set their state to what we remember, and call the OnPluginLoad() in the enabled ones
		 * We also add any new ones and save those to our Appsettings.json if its new.
		*/
		public PluginManagement(Plugins configurationSection) {
			Trace.WriteLine( "PluginManagement()" );
			_settings = configurationSection;

			AggregateCatalog agregateCatalog = new AggregateCatalog();
			CompositionContainer compositionContainer = new CompositionContainer( agregateCatalog );
			CompositionBatch compositionBatch = new CompositionBatch();
			compositionBatch.AddPart( this );

			agregateCatalog.Catalogs.Add( new AssemblyCatalog( Assembly.GetExecutingAssembly() ) );

			foreach (string pathToSearch in _settings.pluginpaths)
			{
				string[] subpaths = Directory.GetDirectories( pathToSearch, "*", SearchOption.AllDirectories );
				foreach ( string subpath in subpaths ) { 
					agregateCatalog.Catalogs.Add( new DirectoryCatalog( subpath ) ); 
				}
			}

			compositionContainer.Compose( compositionBatch );

			Trace.WriteLine( $"Plugins in list: {allPossiblePlugins.Count()}" );

			foreach ( Lazy<IDigiWACSPlugin, IDigiWACSPluginMetadata> PossiblePlugin in allPossiblePlugins ) {
				string name = PossiblePlugin.Metadata.Name;
				int hash = PossiblePlugin.Metadata.GetHashCode();
				Trace.WriteLine( $"Plugin {name} : hash {hash}" );
			}
		}

		public void LoadPlugin(string pathToPlugin) {
		}
	}
}
