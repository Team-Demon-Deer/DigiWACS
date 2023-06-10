using System.Diagnostics;
using System.Reflection;
using DigiWACS.PluginBase;

namespace TestPlugin;

public class TestPlugin : IClientPlugin {
	string IClientPlugin.Name { get => "Test Plugin"; }
	string IClientPlugin.Description { get => "Test Plugin"; }

	public void OnPluginLoad() {
		Trace.WriteLine($"{Assembly.GetExecutingAssembly().GetName()} : OnPluginLoad()");
	}
}