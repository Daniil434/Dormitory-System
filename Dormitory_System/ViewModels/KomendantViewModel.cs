using Dormitory_System.Entities;
using Dormitory_System.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dormitory_System.ViewModels
{
    public class KomendantViewModel : BaseViewModel
    {
        private User _currentUser;
        private ObservableCollection<Block> _blocks;
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<Settlement> _settlements;
        private ObservableCollection<Student> _students;
        private ObservableCollection<Violation> _violations;
        private Block _selectedBlock;
        private Student _selectedStudent;
        private string _newBlockName;
        private string _newRoomNumber;
        private string _newRoomType;
        private string _newViolationType;
        private string _statusMessage;

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public ObservableCollection<Block> Blocks
        {
            get => _blocks;
            set => SetProperty(ref _blocks, value);
        }

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set => SetProperty(ref _rooms, value);
        }

        public ObservableCollection<Settlement> Settlements
        {
            get => _settlements;
            set => SetProperty(ref _settlements, value);
        }

        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }

        public ObservableCollection<Violation> Violations
        {
            get => _violations;
            set => SetProperty(ref _violations, value);
        }

        public Block SelectedBlock
        {
            get => _selectedBlock;
            set => SetProperty(ref _selectedBlock, value);
        }

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set => SetProperty(ref _selectedStudent, value);
        }

        public string NewBlockName
        {
            get => _newBlockName;
            set => SetProperty(ref _newBlockName, value);
        }

        public string NewRoomNumber
        {
            get => _newRoomNumber;
            set => SetProperty(ref _newRoomNumber, value);
        }

        public string NewRoomType
        {
            get => _newRoomType;
            set => SetProperty(ref _newRoomType, value);
        }

        public string NewViolationType
        {
            get => _newViolationType;
            set => SetProperty(ref _newViolationType, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ICommand LogoutCommand { get; }
        public ICommand AddBlockCommand { get; }
        public ICommand DeleteBlockCommand { get; }
        public ICommand AddRoomCommand { get; }
        public ICommand DeleteRoomCommand { get; }
        public ICommand FilterSettlementCommand { get; }
        public ICommand CheckOutStudentCommand { get; }
        public ICommand AddViolationCommand { get; }

        public KomendantViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            LogoutCommand = new RelayCommand(obj => Logout());
            AddBlockCommand = new RelayCommand(obj => AddBlock());
            DeleteBlockCommand = new RelayCommand(obj => DeleteBlock(obj));
            AddRoomCommand = new RelayCommand(obj => AddRoom());
            DeleteRoomCommand = new RelayCommand(obj => DeleteRoom(obj));
            FilterSettlementCommand = new RelayCommand(obj => FilterSettlement());
            CheckOutStudentCommand = new RelayCommand(obj => CheckOutStudent(obj));
            AddViolationCommand = new RelayCommand(obj => AddViolation());

            LoadData();
        }

        private void Logout()
        {
            // Логика выхода
        }

        private void LoadData()
        {
            var blockRepository = new BlockRepository();
            var roomRepository = new RoomRepository();
            var studentRepository = new StudentRepository();
            var settlementRepository = new SettlementRepository();
            var violationRepository = new ViolationRepository();

            Blocks = blockRepository.GetAllBlocks();
            Rooms = roomRepository.GetAllRooms();
            Students = studentRepository.GetAllStudents();
            Settlements = settlementRepository.GetAllSettlements();
            Violations = violationRepository.GetAllViolations();
        }

        private void AddBlock()
        {
            if (!string.IsNullOrEmpty(NewBlockName))
            {
                var newBlock = new Block { BlockName = NewBlockName };
                var blockRepository = new BlockRepository();
                blockRepository.AddBlock(newBlock);
                Blocks.Add(newBlock);
                NewBlockName = string.Empty;
                StatusMessage = "Блок успешно добавлен!";
            }
            else
            {
                StatusMessage = "Введите название блока!";
            }
        }

        private void DeleteBlock(object block)
        {
            if (block is Block b)
            {
                var blockRepository = new BlockRepository();
                blockRepository.DeleteBlock(b.BlockId);
                Blocks.Remove(b);
                StatusMessage = "Блок успешно удален!";
            }
        }

        private void AddRoom()
        {
            if (!string.IsNullOrEmpty(NewRoomNumber) && SelectedBlock != null && !string.IsNullOrEmpty(NewRoomType))
            {
                var newRoom = new Room
                {
                    RoomNumber = NewRoomNumber,
                    BlockId = SelectedBlock.BlockId,
                    RoomType = NewRoomType,
                    Capacity = GetCapacityFromType(NewRoomType)
                };
                var roomRepository = new RoomRepository();
                roomRepository.AddRoom(newRoom);
                Rooms.Add(newRoom);
                NewRoomNumber = string.Empty;
                SelectedBlock = null;
                NewRoomType = string.Empty;
                StatusMessage = "Комната успешно добавлена!";
            }
            else
            {
                StatusMessage = "Заполните все поля!";
            }
        }

        private void DeleteRoom(object room)
        {
            if (room is Room r)
            {
                var roomRepository = new RoomRepository();
                roomRepository.DeleteRoom(r.RoomId);
                Rooms.Remove(r);
                StatusMessage = "Комната успешно удалена!";
            }
        }

        private void FilterSettlement()
        {
            if (SelectedStudent != null)
            {
                var settlementRepository = new SettlementRepository();
                Settlements = settlementRepository.GetSettlementsByStudentId(SelectedStudent.StudentId);
            }
            else
            {
                LoadData();
            }
        }

        private void CheckOutStudent(object settlement)
        {
            if (settlement is Settlement s)
            {
                var settlementRepository = new SettlementRepository();
                settlementRepository.CheckOutStudent(s.SettlementId);
                Settlements.Remove(s);
                StatusMessage = "Студент успешно выселен!";
            }
        }

        private void AddViolation()
        {
            if (SelectedStudent != null && !string.IsNullOrEmpty(NewViolationType))
            {
                var newViolation = new Violation
                {
                    StudentId = SelectedStudent.StudentId,
                    ViolationType = NewViolationType,
                    ViolationDate = DateTime.Now
                };
                var violationRepository = new ViolationRepository();
                violationRepository.AddViolation(newViolation);
                Violations.Add(newViolation);
                SelectedStudent = null;
                NewViolationType = string.Empty;
                StatusMessage = "Нарушение успешно добавлено!";
            }
            else
            {
                StatusMessage = "Заполните все поля!";
            }
        }

        private int GetCapacityFromType(string roomType)
        {
            if (roomType == "Single")
                return 1;
            if (roomType == "Double")
                return 2;
            if (roomType == "Triple")
                return 3;
            if (roomType == "Quad")
                return 4;
            return 0;
        }
    }
}
