using DigiWACS.PluginBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace DigiWACS.Client {
	internal class PluginManagement {
		private static PluginManagement instance;

		public void PluginLoader( string path ) {
			string[] pluginPaths = new string[] { path };

			IEnumerable<IDigiWACSPlugin> commands = pluginPaths.SelectMany( pluginPath => {
				Assembly pluginAssembly = LoadPlugin( pluginPath );
				return CreateCommands( pluginAssembly );
			} ).ToList();
			foreach ( IDigiWACSPlugin command in commands ) {
				Console.WriteLine( $"{command.Info.Name}\t - {command.Info.Description}" );
			}
		}

		static Assembly LoadPlugin( string relativePath ) {
			// Navigate up to the solution root
			string root = Path.GetFullPath( Path.Combine(
				Path.GetDirectoryName(
					Path.GetDirectoryName(
						Path.GetDirectoryName(
							Path.GetDirectoryName(
								Path.GetDirectoryName( typeof( App ).Assembly.Location ) ) ) ) ) ) );

			string pluginLocation = Path.GetFullPath( Path.Combine( root, relativePath.Replace( '\\', Path.DirectorySeparatorChar ) ) );
			Console.WriteLine( $"Loading commands from: {pluginLocation}" );
			PluginLoadContext loadContext = new PluginLoadContext( pluginLocation );
			return loadContext.LoadFromAssemblyName( new AssemblyName( Path.GetFileNameWithoutExtension( pluginLocation ) ) );
		}

		static IEnumerable<IDigiWACSPlugin> CreateCommands( Assembly assembly ) {
			int count = 0;

			foreach ( Type type in assembly.GetTypes() ) {
				if ( typeof( IDigiWACSPlugin ).IsAssignableFrom( type ) ) {
					IDigiWACSPlugin result = Activator.CreateInstance( type ) as IDigiWACSPlugin;
					if ( result != null ) {
						count++;
						yield return result;
					}
				}
			}

			if ( count == 0 ) {
				string availableTypes = string.Join( ",", assembly.GetTypes().Select( t => t.FullName ) );
				throw new ApplicationException(
					$"Can't find any type which implements ICommand in {assembly} from {assembly.Location}.\n" +
					$"Available types: {availableTypes}" );
			}
		}
	}
}
