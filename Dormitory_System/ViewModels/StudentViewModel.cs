using Dormitory_System.DAL;
using Dormitory_System.Entities;
using Dormitory_System.Repositories;
using Dormitory_System.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Dormitory_System.ViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        private User _currentUser;
        private Student _currentStudent;
        private ObservableCollection<Payment> _payments;
        private ObservableCollection<Application> _applications;
        private ObservableCollection<Visit> _visits;
        private string _newApplicationDescription;
        private string _guestName;
        private int _currentPaymentPage = 1;
        private int _totalPaymentPages = 1;
        private int _currentApplicationPage = 1;
        private int _totalApplicationPages = 1;
        private int _currentVisitPage = 1;
        private int _totalVisitPages = 1;
        private string _statusMessage;

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public Student CurrentStudent
        {
            get => _currentStudent;
            set => SetProperty(ref _currentStudent, value);
        }

        public ObservableCollection<Payment> Payments
        {
            get => _payments;
            set => SetProperty(ref _payments, value);
        }

        public ObservableCollection<Application> Applications
        {
            get => _applications;
            set => SetProperty(ref _applications, value);
        }

        public ObservableCollection<Visit> Visits
        {
            get => _visits;
            set => SetProperty(ref _visits, value);
        }

        public string NewApplicationDescription
        {
            get => _newApplicationDescription;
            set => SetProperty(ref _newApplicationDescription, value);
        }

        public string GuestName
        {
            get => _guestName;
            set => SetProperty(ref _guestName, value);
        }

        public int CurrentPaymentPage
        {
            get => _currentPaymentPage;
            set => SetProperty(ref _currentPaymentPage, value);
        }

        public int TotalPaymentPages
        {
            get => _totalPaymentPages;
            set => SetProperty(ref _totalPaymentPages, value);
        }

        public int CurrentApplicationPage
        {
            get => _currentApplicationPage;
            set => SetProperty(ref _currentApplicationPage, value);
        }

        public int TotalApplicationPages
        {
            get => _totalApplicationPages;
            set => SetProperty(ref _totalApplicationPages, value);
        }

        public int CurrentVisitPage
        {
            get => _currentVisitPage;
            set => SetProperty(ref _currentVisitPage, value);
        }

        public int TotalVisitPages
        {
            get => _totalVisitPages;
            set => SetProperty(ref _totalVisitPages, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ICommand LogoutCommand { get; }
        public ICommand CreateApplicationCommand { get; }
        public ICommand RegisterVisitCommand { get; }
        public ICommand NextPaymentPageCommand { get; }
        public ICommand PreviousPaymentPageCommand { get; }
        public ICommand NextApplicationPageCommand { get; }
        public ICommand PreviousApplicationPageCommand { get; }
        public ICommand NextVisitPageCommand { get; }
        public ICommand PreviousVisitPageCommand { get; }

        public StudentViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            LogoutCommand = new RelayCommand(obj => Logout());
            CreateApplicationCommand = new RelayCommand(obj => CreateApplication());
            RegisterVisitCommand = new RelayCommand(obj => RegisterVisit());
            NextPaymentPageCommand = new RelayCommand(obj => NextPaymentPage());
            PreviousPaymentPageCommand = new RelayCommand(obj => PreviousPaymentPage());
            NextApplicationPageCommand = new RelayCommand(obj => NextApplicationPage());
            PreviousApplicationPageCommand = new RelayCommand(obj => PreviousApplicationPage());
            NextVisitPageCommand = new RelayCommand(obj => NextVisitPage());
            PreviousVisitPageCommand = new RelayCommand(obj => PreviousVisitPage());

            LoadStudentData();
            LoadPayments();
            LoadApplications();
            LoadVisits();
        }

        private void Logout()
        {
            // Логика выхода
        }

        private void LoadStudentData()
        {
            var studentRepository = new StudentRepository();
            CurrentStudent = studentRepository.GetStudentByUserId(CurrentUser.UserId);
        }

        private void LoadPayments()
        {
            var paymentRepository = new PaymentRepository();
            Payments = paymentRepository.GetPaymentsByStudentId(CurrentStudent.StudentId);
            TotalPaymentPages = (int)Math.Ceiling(Payments.Count / 10.0);
        }

        private void LoadApplications()
        {
            var applicationRepository = new ApplicationRepository();
            Applications = applicationRepository.GetApplicationsByStudentId(CurrentStudent.StudentId);
            TotalApplicationPages = (int)Math.Ceiling(Applications.Count / 10.0);
        }

        private void LoadVisits()
        {
            var visitRepository = new VisitRepository();
            Visits = visitRepository.GetVisitsByHostStudentId(CurrentStudent.StudentId);
            TotalVisitPages = (int)Math.Ceiling(Visits.Count / 10.0);
        }

        private void CreateApplication()
        {
            if (!string.IsNullOrEmpty(NewApplicationDescription))
            {
                // Получаем RoomId студента из базы данных
                int roomId = GetStudentRoomId(CurrentStudent.StudentId);
                if (roomId == 0)
                {
                    StatusMessage = "У студента не назначена комната!";
                    return;
                }

                var applicationRepository = new ApplicationRepository();
                var newApplication = new Application
                {
                    RoomId = roomId,
                    Description = NewApplicationDescription,
                    ReportDate = DateTime.Now,
                    Status = "Pending"
                };
                applicationRepository.AddApplication(newApplication, CurrentStudent.StudentId);
                Applications.Add(newApplication);
                NewApplicationDescription = string.Empty;
                StatusMessage = "Заявка успешно создана!";
            }
            else
            {
                StatusMessage = "Введите описание проблемы!";
            }
        }

        private int GetStudentRoomId(int studentId)
        {
            string query = "SELECT room_id FROM Students WHERE student_id = @StudentId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    var result = command.ExecuteScalar();
                    DbConnectionManager.CloseConnection(connection);
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        private void RegisterVisit()
        {
            if (!string.IsNullOrEmpty(GuestName))
            {
                var visitRepository = new VisitRepository();
                var newVisit = new Visit
                {
                    GuestName = GuestName,
                    HostStudentId = CurrentStudent.StudentId,
                    TimeIn = DateTime.Now
                };
                visitRepository.AddVisit(newVisit);
                Visits.Add(newVisit);
                GuestName = string.Empty;
                StatusMessage = "Посещение успешно зарегистрировано!";
            }
            else
            {
                StatusMessage = "Введите имя гостя!";
            }
        }

        private void NextPaymentPage()
        {
            if (CurrentPaymentPage < TotalPaymentPages)
            {
                CurrentPaymentPage++;
            }
        }

        private void PreviousPaymentPage()
        {
            if (CurrentPaymentPage > 1)
            {
                CurrentPaymentPage--;
            }
        }

        private void NextApplicationPage()
        {
            if (CurrentApplicationPage < TotalApplicationPages)
            {
                CurrentApplicationPage++;
            }
        }

        private void PreviousApplicationPage()
        {
            if (CurrentApplicationPage > 1)
            {
                CurrentApplicationPage--;
            }
        }

        private void NextVisitPage()
        {
            if (CurrentVisitPage < TotalVisitPages)
            {
                CurrentVisitPage++;
            }
        }

        private void PreviousVisitPage()
        {
            if (CurrentVisitPage > 1)
            {
                CurrentVisitPage--;
            }
        }
    }
}