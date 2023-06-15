using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Reflection;
using DigiWACS.PluginBase;

namespace TestPlugin;

[Export( typeof( IDigiWACSPlugin ) ), 
	ExportMetadata( "Name", "TestPlugin"),
	ExportMetadata( "Type", PluginType.Client ),
	ExportMetadata( "Version", "0.0.1" ),
	ExportMetadata( "Description", "Test Plugin Description" ), 
	ExportMetadata( "Author", "DigiWACS Team" ),
	ExportMetadata( "AuthorUrl", "https://github.com/Team-Demon-Deer/DigiWACS" )]
public class TestPlugin : IDigiWACSPlugin {
	public void OnPluginLoad() {
		Trace.WriteLine($"{Assembly.GetExecutingAssembly().GetName()} : OnPluginLoad()");
	}
}