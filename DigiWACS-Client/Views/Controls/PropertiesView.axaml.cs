using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using DigiWACS_Client.Models;

namespace DigiWACS_Client.Views;

public partial class PropertiesView : UserControl {
	public PropertiesView(HookModel PrimaryHookModel) {
		InitializeComponent();

		var PrimaryHookLine1Binding = new Binding {
			Source = PrimaryHookModel,
			Path = nameof(PrimaryHookModel.HookedCoordinate)
		};
		
		//HookPrimaryLine1.Bind(HookPrimaryLine1.Text, PrimaryHookLine1Binding);
	}
}