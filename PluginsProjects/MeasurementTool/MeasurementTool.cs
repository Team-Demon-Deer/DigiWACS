using System.ComponentModel.Composition;
using DigiWACS.PluginBase;
using System.Diagnostics;
using System.Reflection;
using System;

namespace MeasurementTool;

[Export( typeof( IDigiWACSPlugin ) ),
	ExportMetadata( "Name", "Measurement Tool" ),
	ExportMetadata( "Type", PluginType.Client ),
	ExportMetadata( "Version", "0.0.1" ),
	ExportMetadata( "Description", "A Basic measurement tool" ),
	ExportMetadata( "Author", "DigiWACS Team" ),
	ExportMetadata( "AuthorUrl", "https://github.com/Team-Demon-Deer/DigiWACS" )]
public class MeasurementTool : IDigiWACSPlugin {
	public event EventHandler OnPluginLoaded;

	public MeasurementTool() {
		Trace.WriteLine( $"{Assembly.GetExecutingAssembly().GetName()} : OnPluginLoad()" );
	}


}