using CommunityToolkit.Mvvm.ComponentModel;

namespace TestFilePicker.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
	public MainViewModel()
	{
	}

	private String? _notice;
	public String? Notice
	{
		get => _notice;
		set => SetProperty(ref _notice, value);
	}
}
