using Dormitory_System.Entities;
using Dormitory_System.Services;
using Dormitory_System.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace Dormitory_System.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private User _currentUser;

        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public AsyncRelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthService();
            LoginCommand = new AsyncRelayCommand(ExecuteLoginCommand);
        }

        private async Task ExecuteLoginCommand(object parameter)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Пожалуйста, введите имя пользователя и пароль.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                User authenticatedUser = await _authService.AuthenticateUserAsync(Username, Password);

                if (authenticatedUser != null)
                {
                    CurrentUser = authenticatedUser;

                    // Открываем главное окно
                    var mainWindow = new MainWindow(authenticatedUser);
                    mainWindow.Show();

                    // Закрываем окно входа
                    System.Windows.Application.Current.MainWindow.Close();
                }
                else
                {
                    MessageBox.Show("Неверное имя пользователя или пароль, либо учетная запись неактивна.",
                                    "Ошибка аутентификации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при попытке входа: {ex.Message}", "Системная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
