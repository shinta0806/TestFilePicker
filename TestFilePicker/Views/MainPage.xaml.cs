using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

using TestFilePicker.ViewModels;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Diagnostics;

namespace TestFilePicker.Views;

public sealed partial class MainPage : Page
{
	public MainPage()
	{
		ViewModel = App.GetService<MainViewModel>();
		InitializeComponent();
		MenuFlyoutItemSaveClickedCommand = new RelayCommand(MenuFlyoutItemSaveClicked);
	}

	public MainViewModel ViewModel
	{
		get;
	}


	#region Save
	public RelayCommand MenuFlyoutItemSaveClickedCommand
	{
		get;
	}

	public async void MenuFlyoutItemSaveClicked()
	{
		FileSavePicker fileSavePicker = App.MainWindow.CreateSaveFilePicker();
		fileSavePicker.FileTypeChoices.Add("Text file", [".txt"]);
		StorageFile? file = await fileSavePicker.PickSaveFileAsync();
		if (file == null)
		{
			return;
		}
		Debug.WriteLine(file.Path);
	}
	#endregion

}
