using DigiWACS_Client.ViewModels;

namespace DigiWACS_Client.ViewModels.Controls;

public class PropertiesViewModel(MainViewModel dataContext) : ViewModelBase {
	public MainViewModel Instance { get; } = dataContext;

}