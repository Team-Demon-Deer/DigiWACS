using DigiWACS.PluginBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiWACS.Client.Properties;

//The over all structure
public sealed record DigiWACSSettings {
	public Plugins Plugins { get; set; }
	public List<ServerConnections> ServerConnections { get; set; }
}

//Plugins Section
public sealed record Plugins {
	public List<string> pluginpaths { get; set; }
	public List<KnownPlugins> knownPlugins { get; set; }
}
public sealed record KnownPlugins {
	public bool pluginIsEnabled { get; set; }
	public string pluginName { get; set; }
	public int hash { get; set; }
}

//Server Connections Section
public sealed record ServerConnections {
	public string connectionName { get; set; }
	public string displayName { get; set; }
	public string connectionURL { get; set; }
	public string port { get; set; }
	public string insecurePassword { get; set; }
	public string coalition { get; set; }
}
