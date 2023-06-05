using System.Diagnostics;
using System.Reflection;
using DigiWACS.PluginBase;

namespace TestPlugin;

public class TestPlugin : ClientPlugin {
	public new string name = "Test Plugin";
	public override void OnPluginLoad() {
		Trace.WriteLine($"{Assembly.GetExecutingAssembly().GetName()} : OnPluginLoad()");
	}
}
