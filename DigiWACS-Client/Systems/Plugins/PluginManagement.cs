using DigiWACS.PluginBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using DigiWACS.Client.Properties;

namespace DigiWACS.Client {
	internal class PluginManagement {
		private List<PluginInfo> _plugins;

		/*
		 * We want to try and load all the plugins that we remember in Appsettings.json,
		 * set their state to what we remember, and call the OnPluginLoad() in the enabled ones
		 * We also add any new ones and save those to our Appsettings.json if its new.
		*/
		public PluginManagement(ConfigSectionPlugin PluginSection) {
			Trace.WriteLine( "PluginManagement()" );
			foreach (KnownPlugin knownplugin in PluginSection.KnownPlugins)
			{
				PluginInfo info = new PluginInfo(
					info: knownplugin,
					reference: 
					);
				_plugins.Add();
			}
		}

		public void LoadPlugin(string pathToPlugin) {
		}
		
		internal struct PluginInfo {
			KnownPlugin plugininfo { get; set; }
			IDigiWACSPlugin pluinreference { get; set; }
			PluginInfo(KnownPlugin info, IDigiWACSPlugin reference) {
				plugininfo = info;
				pluinreference = reference;
			}
		}
	}
}
