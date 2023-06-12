using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui.UI.Wpf;

namespace DigiWACS.Client {
	internal partial class MainViewModel : ObservableObject {

		[ObservableProperty]
		MapControl mapControl;
	}
}