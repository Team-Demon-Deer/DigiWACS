using System;
using System.Collections.Generic;
using System.Linq;

namespace DigiWACSPluginBase;

// Classes taken from https://makolyte.com/csharp-generic-plugin-loader/
public class GenericPluginLoader<T> where T : class
{
	private readonly List<GenericAssemblyLoadContext<T>> loadContexts = new List<GenericAssemblyLoadContext<T>>();
	public List<T> LoadAll(string pluginPath, string filter="*.dll", params object[] constructorArgs)
	{
		List<T> plugins = new List<T>();
		
		foreach (var filePath in Directory.EnumerateFiles(pluginPath, filter, SearchOption.AllDirectories))
		{
			var plugin = Load(filePath, constructorArgs);

			if(plugin != null)
			{
				plugins.Add(plugin);
			}
		}

		return plugins;
	}
	private T Load(string pluginPath, params object[] constructorArgs)
	{
		var loadContext = new GenericAssemblyLoadContext<T>(pluginPath);

		loadContexts.Add(loadContext);

		var assembly = loadContext.LoadFromAssemblyPath(pluginPath);

		var type = assembly.GetTypes().FirstOrDefault(t => typeof(T).IsAssignableFrom(t));
		if (type == null)
		{
			return null;
		}

		return (T)Activator.CreateInstance(type, constructorArgs);
	}
	public void UnloadAll()
	{
		foreach(var loadContext in loadContexts)
		{
			loadContext.Unload();
		}
	}
}