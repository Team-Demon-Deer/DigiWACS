using System.Reflection;
using System.Runtime.Loader;

namespace DigiWACS.PluginBase;

// Classes taken from https://makolyte.com/csharp-generic-plugin-loader/
public class GenericAssemblyLoadContext<T> : AssemblyLoadContext where T : class {
	private readonly HashSet<string>? _assembliesToNotLoadIntoContext;
	private readonly AssemblyDependencyResolver _resolver;

	public GenericAssemblyLoadContext(string pluginPath) : base(true) {
		var pluginInterfaceAssembly = typeof(T).Assembly.FullName;
		if (pluginInterfaceAssembly != null) {
			_assembliesToNotLoadIntoContext = GetReferencedAssemblyFullNames(pluginInterfaceAssembly);
			_assembliesToNotLoadIntoContext.Add(pluginInterfaceAssembly);
		}

		_resolver = new AssemblyDependencyResolver(pluginPath);
	}

	private HashSet<string>? GetReferencedAssemblyFullNames(string referencedBy) {
		return AppDomain.CurrentDomain
			.GetAssemblies().FirstOrDefault(t => t.FullName == referencedBy)!
			.GetReferencedAssemblies()
			.Select(t => t.FullName)
			.ToHashSet();
	}

	protected override Assembly Load(AssemblyName assemblyName) {
		//Do not load the Plugin Interface DLL into the adapter's context
		//otherwise IsAssignableFrom is false. 
		if (_assembliesToNotLoadIntoContext.Contains(assemblyName.FullName)) return null!;

		var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
		if (assemblyPath != null) return LoadFromAssemblyPath(assemblyPath);

		return null!;
	}

	protected override IntPtr LoadUnmanagedDll(string unmanagedDllName) {
		var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
		if (libraryPath != null) return LoadUnmanagedDllFromPath(libraryPath);

		return IntPtr.Zero;
	}
}