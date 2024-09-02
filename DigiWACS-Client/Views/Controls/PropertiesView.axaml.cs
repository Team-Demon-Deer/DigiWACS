using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.UpDock.Controls;
using DigiWACS_Client.Models;
using DigiWACS_Client.ViewModels;

namespace DigiWACS_Client.Views;

public partial class PropertiesView : UserControl {
	public PropertiesView() {
		InitializeComponent();
	}

	public void BindPrimaryHook(HookModel PrimaryHookModel) {
		var PrimaryHookLine1Binding = new Binding {
			Source = PrimaryHookModel,
			Path = nameof(PrimaryHookModel.HookedCoordinate)
		};
		
		//HookPrimaryLine1.Bind(HookPrimaryLine1.Text, PrimaryHookLine1Binding);
	}
}