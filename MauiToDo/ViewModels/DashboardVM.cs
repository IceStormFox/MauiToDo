using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiToDo.Extension;
using MauiToDo.Models;
using Realms;
using Realms.Sync;
using System.Collections.ObjectModel;

namespace MauiToDo.ViewModels
{
    public partial class DashboardVM : BaseViewModel
    {
        private Realm realm;
        private PartitionSyncConfiguration config;
        public DashboardVM()
        {
            todoList = new ObservableCollection<Todo>();
            EmptyText = "No todos here. Add the first one :)";
        }
        [ObservableProperty]
        ObservableCollection<Todo> todoList;

        [ObservableProperty] 
        string emptyText;

        [ObservableProperty] 
        string todoEntryText;

        [ObservableProperty] 
        bool isRefreshing;

        public async Task InitialiseRealm()
        {
            config = new PartitionSyncConfiguration($"{App.RealmApp.CurrentUser.Id}", App.RealmApp.CurrentUser);
            realm = Realm.GetInstance(config);

            GetTodos();
            if (TodoList.Count == 0) 
            {
                EmptyText = "Loading projects...";
                await Task.Delay(2000);
                GetTodos();
                EmptyText = "No todos here. Add first one :)";
            }
        }
        [RelayCommand]
        public async void GetTodos()
        {
            IsRefreshing = true;
            IsBusy = true;

            try
            {
                var tlist = realm.All<Todo>().ToList().Reverse<Todo>().OrderBy(t => t.Completed);
                TodoList = new ObservableCollection<Todo>(tlist);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayPromptAsync("Error", ex.Message);
            }

            IsRefreshing = false;
            IsBusy = false;
        }
        [RelayCommand]
        async Task AddTodo()
        {
            if (string.IsNullOrWhiteSpace(TodoEntryText))
                return;
            IsBusy = true;
            try
            {
                var todo = new Todo
                {
                    Name = GeneralExtensions.UppercaseFirst(TodoEntryText),
                    Partition = App.RealmApp.CurrentUser.Id,
                    Owner = App.RealmApp.CurrentUser.Profile.Email
                };
                realm.Write(() =>
                {
                    realm.Add(todo);
                });
                TodoEntryText = "";
                GetTodos();
            }
            catch (Exception ex) 
            {
                await Application.Current.MainPage.DisplayPromptAsync ("Error", ex.Message);
            }
            IsBusy = false;
        }

        [RelayCommand]
        public async void EditTodo(Todo todo)
        {
            string newString = await App.Current.MainPage.DisplayPromptAsync("Edit", todo.Name);

            if (newString is null || string.IsNullOrWhiteSpace(newString.ToString())) 
                return;
            try
            {
                realm.Write(() =>
                {
                    var foundTodo = realm.Find<Todo>(todo.Id);
                    foundTodo.Name = GeneralExtensions.UppercaseFirst(newString.ToString());
                });
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayPromptAsync("Error", ex.Message);
            }
        }
        [RelayCommand]
        public async void CheckTodo(Todo todo)
        {
            try
            {
                realm.Write(() =>
                {
                    var foundTodo = realm.Find<Todo>(todo.Id);
                    foundTodo.Completed = !foundTodo.Completed;
                });
                await Task.Delay(2);
                var sortedList = TodoList.ToList().OrderBy(t => t.Completed);
                TodoList = new ObservableCollection<Todo>(sortedList);
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayPromptAsync("Error", ex.Message);
            }
        }
        [RelayCommand]
        async Task DeleteTask(Todo todo) 
        {
            IsBusy = true;
            try
            {
                realm.Write(() =>
                {
                    realm.Remove(todo);
                });
                todoList.Remove(todo);
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayPromptAsync("Error", ex.Message);
            }
            IsBusy = false;
        }

        [RelayCommand]
        async Task SignOut()
        {
            IsBusy = true;
            try
            {
                await App.RealmApp.RemoveUserAsync(App.RealmApp.CurrentUser);
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                App.Current.Quit();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayPromptAsync("Error", ex.Message);
            }
            IsBusy = false;
        }
    }
}
