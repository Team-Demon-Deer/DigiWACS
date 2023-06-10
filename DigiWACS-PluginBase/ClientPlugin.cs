namespace DigiWACS.PluginBase;

public interface IClientPlugin {
	public string Name { get; }
	public string Description { get; }
	void OnPluginLoad();

	//Theorized Mouse Actions
	void OnNoEntitySelect() { }
	void OnSingleEntitySelect() { }
	void OnMultiEntitySelect() { }
}