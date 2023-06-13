using DigiWACS.PluginBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiWACS.Client.Properties;

//The over all structure
public struct AppSettingsStruct {
	public ConfigSectionPlugin ConfigSectionPlugin;
	public ConfigSectionServerConnections ConfigSectionServerConnections;
}

//Plugins Section
public struct ConfigSectionPlugin {
	public string[] pluginpaths;
	public KnownPlugin[] KnownPlugins;
}
public struct KnownPlugin {
	public string pluginEnabled;
	public string pluginName;
	public string pluginPath;
}

//Server Connections Section
public struct ConfigSectionServerConnections {
	public string connectionName;
	public string displayName;
	public string connectionURL;
	public string port;
	public string insecurePassword;
	public string coalition;
}
