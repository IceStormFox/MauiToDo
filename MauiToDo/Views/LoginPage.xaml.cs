using MauiToDo.ViewModels;

namespace MauiToDo.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}