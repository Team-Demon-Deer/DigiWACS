using DigiWACS.PluginBase;
using System;
using System.Diagnostics;
using System.Reflection;

namespace MeasurementTool {
	public class MeasurementTool : IDigiWACSPlugin {
		public DigiWACSPluginInfo Info => new DigiWACSPluginInfo(
		name: "Measurement Tool",
		description: "A Basic measurement tool",
		type: PluginType.Client,
		author: "DigiWACS Team",
		version: "0.0.1",
		authorURL: "https://github.com/Team-Demon-Deer/DigiWACS"
		);

		public void OnPluginLoad() {
			Trace.WriteLine( $"{Assembly.GetExecutingAssembly().GetName()} : OnPluginLoad()" );
		}
	}
}