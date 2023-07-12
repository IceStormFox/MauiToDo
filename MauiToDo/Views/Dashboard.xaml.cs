using MauiToDo.ViewModels;

namespace MauiToDo.Views;

public partial class Dashboard : ContentPage
{
	public Dashboard(DashboardVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}