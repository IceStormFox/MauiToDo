using MauiToDo.ViewModels;

namespace MauiToDo.Views;

public partial class Dashboard : ContentPage
{
	DashboardVM vm;
    public Dashboard()
	{
		InitializeComponent();
		vm = new DashboardVM();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		await vm.InitialiseRealm();
	}
}