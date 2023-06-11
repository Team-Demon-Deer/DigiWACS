using System.Diagnostics;
using System.Reflection;
using DigiWACS.PluginBase;

namespace TestPlugin;

public class TestPlugin : IDigiWACSPlugin {
	DigiWACSPluginInfo IDigiWACSPlugin.Info => new DigiWACSPluginInfo(
		name: "TestPlugin",
		description: "Test Plugin Description",
		type: PluginType.Client,
		author: "DigiWACS Team",
		version: "0.0.1",
		authorURL: "https://github.com/Team-Demon-Deer/DigiWACS"
		);

	public void OnPluginLoad() {
		Trace.WriteLine($"{Assembly.GetExecutingAssembly().GetName()} : OnPluginLoad()");
	}
}