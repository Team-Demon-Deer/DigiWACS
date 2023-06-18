using System.ComponentModel;
using System.ComponentModel.Composition;

namespace DigiWACS.PluginBase;


public interface IDigiWACSPlugin {
	event EventHandler OnPluginLoaded;
}

public interface IDigiWACSPluginMetadata {

	//Required
	public string Name { get; }
	public PluginType Type { get; }

	//Optional
	[DefaultValue( "0.0.0" )]
	public string Version { get; }

	[DefaultValue("No Description")]
	public string Description { get; }

	[DefaultValue( "No Author" )]
	public string Author { get; }

	[DefaultValue( "No URL" )]
	public string AuthorUrl { get; }
}
public enum PluginType {
	Client,
	Server,
	Both
}