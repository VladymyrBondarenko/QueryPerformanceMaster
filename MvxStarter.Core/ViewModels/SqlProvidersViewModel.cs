﻿using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using MvxStarter.Core.Messages;
using MvxStarter.Core.Models;
using MvxStarter.Core.Services;
using MvxStarter.Core.ViewModels.ConnectionParamsViewModels;
using QueryPerformanceMaster.Domain.SqlProviders;
using System.Collections.ObjectModel;

namespace MvxStarter.Core.ViewModels
{
    public class SqlProvidersViewModel : MvxViewModel
    {
        private readonly MvxSubscriptionToken _connectedToSqlProviderToken;
        private readonly ISqlProviderManager _sqlProviderManager;

        public SqlProvidersViewModel(ISqlProviderManager sqlProviderManager,
            IMvxMessenger mvxMessenger)
        {
            _sqlProviderManager = sqlProviderManager;
            _connectedToSqlProviderToken = mvxMessenger.Subscribe<ConnectedToSqlProviderMessage>(OnConnectedToSqlProvider);
        }

        private ObservableCollection<SqlProviderModel> _sqlProviderModels;
        public ObservableCollection<SqlProviderModel> SqlProviderModels
        {
            get { return _sqlProviderModels; }
            set { SetProperty(ref _sqlProviderModels, value); }
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            SqlProviderModels = new ObservableCollection<SqlProviderModel>(_sqlProviderManager.GetSqlProviders());
        }

        private void OnConnectedToSqlProvider(ConnectedToSqlProviderMessage message)
        {
            var sqlProvider = SqlProviderModels.FirstOrDefault(x => x.SqlProvider == message.SqlProvider);
            if (sqlProvider != null)
            {
                var databases = new List<SqlProviderDatabaseModel>(
                    message.Databases.Select(x => new SqlProviderDatabaseModel
                    {
                        Name = x,
                        SqlProvider = sqlProvider.SqlProvider,
                        ConnectionString = message.ConnectionString
                    }));

                SqlProviderModels[SqlProviderModels.IndexOf(sqlProvider)] = new SqlProviderModel
                {
                    Name = sqlProvider.Name,
                    SqlProvider = sqlProvider.SqlProvider,
                    IconPath = sqlProvider.IconPath,
                    Databases = databases,
                    IsExpanded = sqlProvider.IsExpanded,
                    ConnectionString = message.ConnectionString
                };
            }
        }
    }
}
