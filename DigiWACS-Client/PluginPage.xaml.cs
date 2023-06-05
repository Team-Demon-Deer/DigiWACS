using System.Windows;
using static DigiWACS.Client.App;

namespace DigiWACS.Client;

public partial class PluginWindow : Window {
	public PluginWindow() {
		InitializeComponent();
		foreach (var plugin in LoadedClientPlugins) plugin.OnPluginLoad();
	}
}