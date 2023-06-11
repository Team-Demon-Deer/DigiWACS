namespace DigiWACS.PluginBase;

public interface IDigiWACSPlugin {
	public DigiWACSPluginInfo Info { get; }
	void OnPluginLoad();
}

public struct DigiWACSPluginInfo {
	public string Name { get; }
	public string Description { get; }
	public PluginType Type { get; }
	public string Author { get; }
	public string Version { get; }
	public string AuthorUrl { get; }

	//Contstructor
	public DigiWACSPluginInfo ( string name,
							string description,
							PluginType type,
							string author,
							string version,
							string authorURL ) {
		Name = name; 
		Description = description; 
		Type = type; 
		Author = author; 
		Version = version; 
		AuthorUrl = authorURL;
	}

}
public enum PluginType {
	Client,
	Server,
	Both
}
