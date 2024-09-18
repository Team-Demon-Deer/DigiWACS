using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DigiWACS_Client.Models;
using DigiWACS_Client.ViewModels;
using DigiWACS_Client.Views;
using Microsoft.Extensions.Configuration;

namespace DigiWACS_Client;

public partial class App : Application
{
	private static IConfigurationRoot _configuration = new ConfigurationBuilder()
		.AddJsonFile("appsettings.json")
		.AddEnvironmentVariables()
		.Build();

	private static SettingsModel? Settings = _configuration.GetRequiredSection("Settings").Get<SettingsModel>();
	
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			// Line below is needed to remove Avalonia data validation.
			// Without this line you will get duplicate validations from both Avalonia and CT
			BindingPlugins.DataValidators.RemoveAt(0);
			desktop.MainWindow = new MainWindow {
				DataContext = new MainViewModel(Settings)
			};
		}
		base.OnFrameworkInitializationCompleted();
	}
}