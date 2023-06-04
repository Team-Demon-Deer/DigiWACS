namespace DigiWACS.PluginBase;

public class ClientPlugin {
	public virtual void OnPluginLoad() { }

	//Theorized Mouse Actions
	public virtual void OnNoEntitySelect() { }
	public virtual void OnSingleEntitySelect() { }
	public virtual void OnMultiEntitySelect() { }
}