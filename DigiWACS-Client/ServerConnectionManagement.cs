using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DigiWACS.Client;
/*
internal class ServerConnectionManagement {
	public Dictionary<string, ServerConnection> m_servers;
	IConfigurationRoot m_configurationRoot;

	public ServerConnectionManagement( IConfigurationRoot configurationRoot ) {
		m_configurationRoot = configurationRoot;
		foreach (IConfigurationSection connection in m_configurationRoot.GetSection( "configurationRoot" ).GetChildren() )
		{
			ServerConnection _serverConnection = new ServerConnection() {
				DisplayName = connection.GetValue<string>( "Display Name" ),
				ConnectionURL = connection.GetValue<string>( "ConnectionURL" ),
				Port = connection.GetValue<string>( "Port" ),
				InsecurePassword = connection.GetValue<string>( "InsecurePassword" )
			};
			Enum.TryParse<coalitionEnum>( connection.GetValue<string>( "Coalition" ), out _serverConnection.Coalition );

		m_servers.Add( connection.Key.ToString(), _serverConnection );
		}
	}
}

public record ServerConnection {
	public string DisplayName;
	public string ConnectionURL;
	public string Port;
	public string InsecurePassword;
	public coalitionEnum Coalition;

	public enum coalitionEnum {
		Neutral,
		Blue,
		Red
	}
}
*/
