using Dormitory_System.Entities;
using Dormitory_System.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dormitory_System.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private User _currentUser;
        private ObservableCollection<User> _users;
        private User _selectedUser;
        private string _searchText;
        private int _currentPage;
        private int _totalPages;
        private string _customSqlQuery;
        private DataTable _queryResult;
        private bool _hasQueryResult;
        private ObservableCollection<object> _reportData;
        private string _statusMessage;

        // Свойства для привязки
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                SearchUsers(); // Поиск при изменении текста
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged();
            }
        }

        public string CustomSqlQuery
        {
            get => _customSqlQuery;
            set
            {
                _customSqlQuery = value;
                OnPropertyChanged();
            }
        }

        public DataTable QueryResult
        {
            get => _queryResult;
            set
            {
                _queryResult = value;
                OnPropertyChanged();
                HasQueryResult = _queryResult != null && _queryResult.Rows.Count > 0;
            }
        }

        public bool HasQueryResult
        {
            get => _hasQueryResult;
            set
            {
                _hasQueryResult = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<object> ReportData
        {
            get => _reportData;
            set
            {
                _reportData = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        // Команды
        public ICommand LogoutCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand BackupDatabaseCommand { get; }
        public ICommand RestoreDatabaseCommand { get; }
        public ICommand ShowLogCommand { get; }
        public ICommand ExecuteQueryCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand SaveToWordCommand { get; }

        // Конструктор
        public AdminViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            LogoutCommand = new RelayCommand(obj => Logout());
            AddUserCommand = new AuthorizedRelayCommand(
                execute: param => AddUser(),
                canExecute: param => AuthorizationService.CanAddUsers(CurrentUser),
                currentUser: CurrentUser
            );
            EditUserCommand = new AuthorizedRelayCommand(
                execute: param => EditUser(param),
                canExecute: param => AuthorizationService.CanEditUsers(CurrentUser),
                currentUser: CurrentUser
            );
            DeleteUserCommand = new AuthorizedRelayCommand(
                execute: param => DeleteUser(param),
                canExecute: param => AuthorizationService.CanDeleteUsers(CurrentUser),
                currentUser: CurrentUser
            );
            PreviousPageCommand = new RelayCommand(obj => PreviousPage());
            NextPageCommand = new RelayCommand(obj => NextPage());
            BackupDatabaseCommand = new AuthorizedRelayCommand(
                execute: param => BackupDatabase(),
                canExecute: param => AuthorizationService.CanBackupDatabase(CurrentUser),
                currentUser: CurrentUser
            );
            RestoreDatabaseCommand = new AuthorizedRelayCommand(
                execute: param => RestoreDatabase(),
                canExecute: param => AuthorizationService.CanRestoreDatabase(CurrentUser),
                currentUser: CurrentUser
            );
            ShowLogCommand = new AuthorizedRelayCommand(
                execute: param => ShowLog(),
                canExecute: param => AuthorizationService.CanViewLog(CurrentUser),
                currentUser: CurrentUser
            );
            ExecuteQueryCommand = new AuthorizedRelayCommand(
                execute: param => ExecuteQuery(),
                canExecute: param => AuthorizationService.CanExecuteQueries(CurrentUser),
                currentUser: CurrentUser
            );
            GenerateReportCommand = new RelayCommand(obj => GenerateReport());
            SaveToWordCommand = new RelayCommand(obj => SaveToWord());

            // Инициализация данных
            LoadUsers();
        }

        // Методы для команд
        private void Logout()
        {
            // Логика выхода
        }

        private void AddUser()
        {
            // Логика добавления пользователя
        }

        private void EditUser(object param)
        {
            // Логика редактирования пользователя
        }

        private void DeleteUser(object param)
        {
            // Логика удаления пользователя
        }

        private void PreviousPage()
        {
            if (CurrentPage > 1)
                CurrentPage--;
            LoadUsers();
        }

        private void NextPage()
        {
            if (CurrentPage < TotalPages)
                CurrentPage++;
            LoadUsers();
        }

        private void BackupDatabase()
        {
            // Логика резервного копирования
        }

        private void RestoreDatabase()
        {
            // Логика восстановления из резервной копии
        }

        private void ShowLog()
        {
            // Логика просмотра журнала
        }

        private void ExecuteQuery()
        {
            // Логика выполнения сложного запроса
        }

        private void GenerateReport()
        {
            // Логика формирования отчета
        }

        private void SaveToWord()
        {
            // Логика сохранения в Word
        }

        // Методы для загрузки данных
        private void LoadUsers()
        {
            // Логика загрузки пользователей с пагинацией
        }

        private void SearchUsers()
        {
            // Логика поиска пользователей
        }
    }
}
