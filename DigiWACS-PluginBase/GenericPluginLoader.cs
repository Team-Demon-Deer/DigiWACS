namespace DigiWACS.PluginBase;

// Classes taken from https://makolyte.com/csharp-generic-plugin-loader/
public class GenericPluginLoader<T> where T : class {
	private readonly List<GenericAssemblyLoadContext<T>> _loadContexts = new();

	public List<T> LoadAll(string pluginPath, string filter = "*.dll", params object[] constructorArgs) {
		var plugins = new List<T>();
		try {
			foreach (var filePath in Directory.EnumerateFiles(pluginPath, filter, SearchOption.AllDirectories)) {
				var plugin = Load(filePath, constructorArgs);

				if (plugin != null) plugins.Add(plugin);
			}
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
		return plugins;
	}

	private T? Load(string pluginPath, params object[] constructorArgs) {
		var loadContext = new GenericAssemblyLoadContext<T>(pluginPath);

		_loadContexts.Add(loadContext);

		var assembly = loadContext.LoadFromAssemblyPath(pluginPath);

		var type = assembly.GetTypes().FirstOrDefault(t => typeof(T).IsAssignableFrom(t));
		if (type == null) return null;

		return (T)Activator.CreateInstance(type, constructorArgs)!;
	}

	public void UnloadAll() {
		foreach (var loadContext in _loadContexts) loadContext.Unload();
	}
}