using System.Diagnostics;
using System.Windows;
using DigiWACS.PluginBase;
using Microsoft.Extensions.Configuration;
using static DigiWACS.Client.App;

namespace DigiWACS.Client;

public partial class PluginWindow : Window {

	public PluginWindow() {
		InitializeComponent();
		foreach (ClientPlugin plugin in LoadedClientPlugins) {
			plugin.OnPluginLoad();
		}
	}
}