using DigiWACS.PluginBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiWACS.Client.Properties;

//The over all structure
public sealed class DigiWACSSettings {
	public Plugins Plugins { get; set; }
	public List<ServerConnections> ServerConnections { get; set; }
}

//Plugins Section
public sealed class Plugins {
	public List<string> pluginpaths { get; set; }
	public List<KnownPlugins> knownPlugins { get; set; }
}
public sealed class KnownPlugins {
	public string pluginIsEnabled { get; set; }
	public string pluginName { get; set; }
	public int hash { get; set; }
}

//Server Connections Section
public sealed class ServerConnections {
	public string connectionName { get; set; }
	public string displayName { get; set; }
	public string connectionURL { get; set; }
	public string port { get; set; }
	public string insecurePassword { get; set; }
	public string coalition { get; set; }
}
