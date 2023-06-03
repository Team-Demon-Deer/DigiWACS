using DigiWACSPluginBase;

namespace TestPlugin;

public class TestPlugin : IClientPlugin
{
	public void Process(string message)
	{
		Console.WriteLine($"Message Logger: {DateTime.Now:O}\tReceived message {message}");
	}
}