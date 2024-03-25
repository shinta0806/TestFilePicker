using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using TestFilePicker.ViewModels;
using Windows.Storage;
using Windows.Storage.Pickers;

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
		try
		{
			FileSavePicker fileSavePicker = App.MainWindow.CreateSaveFilePicker();
			fileSavePicker.FileTypeChoices.Add("Text file", [".txt"]);
			StorageFile? file = await fileSavePicker.PickSaveFileAsync();
			if (file == null)
			{
				return;
			}
			ViewModel.Notice = file.Path;
		}
		catch (Exception ex)
		{
			// 管理者として実行すると例外が発生する（別の問題）
			// WindowsAppSDK チケット #2504 Trying to use a FileOpenPicker while running the app as Administrator will crash the app
			// https://github.com/microsoft/WindowsAppSDK/issues/2504
			ViewModel.Notice = ex.Message + "\n" + ex.StackTrace;

			// 【例外発生整理】
			// ・ログインユーザーにかかわらず、「管理者として実行」すると例外
			//   → 一般ユーザーでそのまま実行：OK
			//   → 一般ユーザーで管理者として実行：NG
			//   → 管理者ユーザーでそのまま実行：OK
			//   → 管理者ユーザーで管理者として実行：NG

			// 【ビルド整理】
			// ・異なるユーザーでビルド・実行しようとすると DEP0700 エラー「～競合するパッケージは XXXX です」となる
			// ・管理者権限で PowerShell を起動し、
			//   Get-AppxPackage -AllUsers | Select-String "XXXX"
			//   → PackageFullName "XXXX-yyyy" が表示される
			//   Remove-AppPackage -Package "XXXX-yyyy" -AllUsers
			//   → パッケージがアンインストールされるので、異なるユーザーでビルド・実行できるようになる
			//   → 一般ユーザーでログインして管理者として Visual Studio を実行すると別のエラー（パッケージの再インストールはブロックされました）になるので、管理者は管理者としてログインする
		}
	}
	#endregion
}
