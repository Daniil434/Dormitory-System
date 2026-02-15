using Dormitory_System.Entities;
using Dormitory_System.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dormitory_System.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        private User _currentUser;

        // Свойство для текущего пользователя
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
                UpdateViewBasedOnRole(); // Обновляем представление при смене пользователя
            }
        }

        // Свойство для текущего представления (UserControl)
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // Команда для выхода из системы
        public RelayCommand LogoutCommand { get; }

        // Конструктор, принимающий пользователя
        public MainViewModel(User user)
        {
            CurrentUser = user; // Устанавливаем пользователя и вызываем UpdateViewBasedOnRole
            LogoutCommand = new RelayCommand(obj => Logout());
        }

        // Метод для обновления представления в зависимости от роли пользователя
        private void UpdateViewBasedOnRole()
        {
            if (CurrentUser == null)
            {
                CurrentView = null;
                return;
            }

            switch (CurrentUser.RoleName)
            {
                case "Администратор":
                    CurrentView = new AdminView();
                    break;
                case "Комендант":
                    CurrentView = new KomendantView();
                    break;
                case "Кастелянша":
                    CurrentView = new KastylyankaView();
                    break;
                case "Студент":
                    CurrentView = new StudentView();
                    break;
                default:
                    CurrentView = new DefaultView();
                    break;
            }
        }

        // Метод для выхода из системы
        private void Logout()
        {
            // Очищаем текущего пользователя
            CurrentUser = null;
            CurrentView = null;

            // Показываем окно входа
            var loginView = new LoginView();
            loginView.Show();

            // Закрываем главное окно
            System.Windows.Application.Current.MainWindow.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
