using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace DigiWACS.Client;

public class AppSettings {
	public required string PluginsPath { get; set; }
}