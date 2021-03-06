﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using Core;
using Core.Events;
using Core.Model;
using Core.ViewModel;

namespace Agent.ViewModel
{
    class MainViewModel : VM
    {
        private GameViewModel GameView;
        private ServerModel ServerModel;
        private FiltersEvent FiltersEvent = new FiltersEvent();

        public GlobalEvents.ServerEvents ServerEvents = new GlobalEvents.ServerEvents();
        public GameModel GameModel = new GameModel();

        public NewsViewModel NewsViewModel { get; }
        public HomeViewModel HomeViewModel { get; }
        public ItemsViewModel ItemsViewModel { get; }
        public AlertsViewModel AlertsViewModel { get; }

        public ObservableCollection<UserNotification> UserNotifications => GameView.UserNotifications;

        Task finishInit;
        public MainViewModel()
        {
            BadFilterReportModel.Start();
            FactionsEngine.Start(FiltersEvent);
            finishInit = FiltersEvent.Start(); // добавляйте ещё через Task.WhenAll
            ServerModel = new ServerModel(ServerEvents);
            ServerModel.Start();
            GameView = new GameViewModel(GameModel, FiltersEvent);
            
            NewsViewModel = new NewsViewModel(GameView);
            HomeViewModel = new HomeViewModel(GameView);
            ItemsViewModel = new ItemsViewModel(GameView);
            AlertsViewModel = new AlertsViewModel(GameView);

            ActivateHomeCommand = new RelayCommand(() => CurrentContent = HomeViewModel);
            ActivateNewsCommand = new RelayCommand(() => CurrentContent = NewsViewModel);
            ActivateItemsCommand = new RelayCommand(() => CurrentContent = ItemsViewModel);
            ActivateAlertsCommand = new RelayCommand(() => CurrentContent = AlertsViewModel);
            CurrentContent = HomeViewModel;
        }

        public async Task FinishInit() => await finishInit;

        public void Run()
        {
            ServerEvents.Connected += GameDataEvent_Connected;
            ServerEvents.Disconnected += GameDataEvent_Disconnected;
            GameView.Run();
            GameModel.Start(ServerEvents, FiltersEvent,
                $"{Settings.Program.Directories.Temp}/GameData.json",
                $"{Settings.Program.Directories.Temp}/NewsData.json");
            BackgroundEvent.Start();
        }

        public async Task StopAsync()
        {
            await Task.WhenAll(
                BadFilterReportModel.StopAsync(),
                BackgroundEvent.StopAsync(),
                FiltersEvent.StopAsync());
        }

        private async void GameDataEvent_Disconnected()
        {
            await AsyncHelpers.RedirectToMainThread();
            IsConnectionLost = true;
        }

        private async void GameDataEvent_Connected()
        {
            await AsyncHelpers.RedirectToMainThread();
            IsConnectionLost = false;
        }

        bool isConnectionLost = false;
        public bool IsConnectionLost
        {
            get => isConnectionLost;
            set => Set(ref isConnectionLost, value);
        }

        VM currentContent;
        public VM CurrentContent
        {
            get => currentContent;
            set => Set(ref currentContent, value);
        }

        public ICommand ActivateHomeCommand { get; }
        public ICommand ActivateNewsCommand { get; }
        public ICommand ActivateItemsCommand { get; }
        public ICommand ActivateAlertsCommand { get; }
    }
}
