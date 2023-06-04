using System.Diagnostics;
using DigiWACS.PluginBase;

namespace TestPlugin;

public class TestPlugin : ClientPlugin {
	public override void OnPluginLoad() {
		Trace.WriteLine("Plugin Loaded");
	}
}