using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace DigiWACS.Client.Systems.Plugins;

/*

public class GenericPluginLoader<T> where T : class {
	private readonly List<GenericAssemblyLoadContext<T>> _loadContexts = new();

	public List<T> LoadAll( string pluginPath, string filter = "*.dll", params object[] constructorArgs ) {
		var plugins = new List<T>();
		try {
			foreach ( var filePath in Directory.EnumerateFiles( pluginPath, filter, SearchOption.AllDirectories ) ) {
				var plugin = Load( filePath, constructorArgs );

				if ( plugin != null ) plugins.Add( plugin );
			}
		}
		catch ( Exception e ) {
			Console.WriteLine( e );
			throw;
		}

		return plugins;
	}

	private T? Load( string pluginPath, params object[] constructorArgs ) {
		var loadContext = new GenericAssemblyLoadContext<T>( pluginPath );

		_loadContexts.Add( loadContext );

		var assembly = loadContext.LoadFromAssemblyPath( pluginPath );

		var type = assembly.GetTypes().FirstOrDefault( t => typeof( T ).IsAssignableFrom( t ) );
		if ( type == null ) return null;

		return (T)Activator.CreateInstance( type, constructorArgs )!;
	}

	public void UnloadAll() {
		foreach ( var loadContext in _loadContexts ) loadContext.Unload();
	}
}

public class GenericAssemblyLoadContext<T> : AssemblyLoadContext where T : class {
	private readonly HashSet<string>? _assembliesToNotLoadIntoContext;
	private readonly AssemblyDependencyResolver _resolver;

	public GenericAssemblyLoadContext( string pluginPath ) : base( true ) {
		var pluginInterfaceAssembly = typeof( T ).Assembly.FullName;
		if ( pluginInterfaceAssembly != null ) {
			_assembliesToNotLoadIntoContext = GetReferencedAssemblyFullNames( pluginInterfaceAssembly );
			_assembliesToNotLoadIntoContext.Add( pluginInterfaceAssembly );
		}

		_resolver = new AssemblyDependencyResolver( pluginPath );
	}

	private HashSet<string>? GetReferencedAssemblyFullNames( string referencedBy ) {
		return AppDomain.CurrentDomain
			.GetAssemblies().FirstOrDefault( t => t.FullName == referencedBy )!
			.GetReferencedAssemblies()
			.Select( t => t.FullName )
			.ToHashSet();
	}

	protected override Assembly Load( AssemblyName assemblyName ) {
		//Do not load the Plugin Interface DLL into the adapter's context
		//otherwise IsAssignableFrom is false. 
		if ( _assembliesToNotLoadIntoContext.Contains( assemblyName.FullName ) ) return null!;

		var assemblyPath = _resolver.ResolveAssemblyToPath( assemblyName );
		if ( assemblyPath != null ) return LoadFromAssemblyPath( assemblyPath );

		return null!;
	}

	protected override IntPtr LoadUnmanagedDll( string unmanagedDllName ) {
		var libraryPath = _resolver.ResolveUnmanagedDllToPath( unmanagedDllName );
		if ( libraryPath != null ) return LoadUnmanagedDllFromPath( libraryPath );

		return IntPtr.Zero;
	}
}

*/