using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.Pages;
using CleanBrain.UoF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CleanBrain.MVVM
{
    public class PsyhologistModel : INotifyPropertyChanged
    {
        
        private bool editing = false;
        private string[] degreemas =  ManagerItem.isRussian ? new string[] { "Выберите","Балаклавр", "Магистр" } : new string[] { "Choose", "Bachelor", "Master" };
        private string[] spezializationMass = ManagerItem.isRussian ? new string[] { "Выберите", "Клиническая", "Когнитивная","Развивающая","Социальная" } : new string[] { "Choose", "Clinical", "Cognitive", "Developing", "Social" };
        private string[] timeStartMass = ManagerItem.isRussian ? new string[] { "Choose", "08:00","09:00","10:00","11:00","12:00" } : new string[] { "Choose", "08:00", "09:00", "10:00", "11:00", "12:00" };
        private string[] timeEndMass = ManagerItem.isRussian ?  new string[]{ "Выберите", "13:00","14:00","15:00","16:00","17:00","18:00","19:00","20:00"}: new string[] { "Choose", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00" };
        private byte[] imageBytes;
        private string exp;
        private Brush correctName = new SolidColorBrush(Color.FromRgb(176, 42, 42));
        private Brush correctSurname = new SolidColorBrush(Color.FromRgb(176, 42, 42));
        private Brush correctPatrynomic = new SolidColorBrush(Color.FromRgb(176, 42, 42));
        private Brush correctExp = new SolidColorBrush(Color.FromRgb(176, 42, 42));
        private Brush correctText = new SolidColorBrush(Color.FromRgb(176, 42, 42));
        private Timetable table = new Timetable();
        private Psychologist psychologist = new Psychologist();
        private Psychologist find;
        private UnitOfWork unit = new UnitOfWork();
        private BitmapImage image;
        private bool flagDesc = false;
        private bool activePsy = false;
        private bool comboActive = true;
        private string mondStart;
        private string mondEnd;
        private string tueStart;
        private string tueEnd;
        private string wenStart;
        private string wenEnd;
        private string thuStart;
        private string thuEnd;
        private string friStart;
        private string friEnd;
        private bool visDelete =false;

        public bool ComboActive
        {
            get { return comboActive; }
            set { comboActive = value; OnPropertyChanged("ComboActive"); }
        }

        public bool ActivePsy
        {
            get { return activePsy; }
            set { activePsy = value; OnPropertyChanged("ActivePsy"); }
        }

        public bool VisDelete
        {
            get { return visDelete; }
            set { visDelete = value; }
        }

        private void getActive()
        {
            if (ActivePsy)
            {
                VisDelete = false;
                ComboActive = false;
            }
            else
            {
                VisDelete = true;
                ComboActive = true;
            }
        }



        public PsyhologistModel(int id)
        {
            ActivePsy = ManagerItem.IsReadOnly;
            if (ManagerItem.ImClient)
                ActivePsy = true;
            if (ManagerItem.isRussian)
                NameButton = "Изменить";
            else
                NameButton = "Editing";
            VisDelete = true;
            getActive();
            Psychologist = unit.Psychologist.Get(id);
            imageBytes = Psychologist.Photo_Psychologist;
            Image = GetImage(imageBytes);
            editing = true;
            flagDesc = true;
            Table = unit.TimeTable.Get(Psychologist.Id_Psychologist);
            MondStart = Table.MondStart.ToString().Substring(0, 5); 
            MondEnd= Table.MondEnd.ToString().Substring(0, 5);
            TueStart = Table.TueStart.ToString().Substring(0, 5);
            TueEnd = Table.TueEnd.ToString().Substring(0, 5);
            WenStart= Table.WenStart.ToString().Substring(0, 5);
            WenEnd= Table.WenEnd.ToString().Substring(0, 5);
            ThuStart= Table.ThuStart.ToString().Substring(0, 5);
            ThuEnd= Table.ThuEnd.ToString().Substring(0, 5);
            FriStart= Table.FriStart.ToString().Substring(0, 5);
            FriEnd= Table.FriEnd.ToString().Substring(0, 5);
            Exp = Psychologist.Experience.ToString();
            Psychologist.Degree = GetCorrectDegreeView(Psychologist.Degree);
            Psychologist.Spezialization_Psychologist = GetCorrectSpecView(Psychologist.Spezialization_Psychologist);
            CorrectName = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            CorrectSurname = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            CorrectPatronymic = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            CorrectExp = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            CorrectText = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        public PsyhologistModel()
        {
            if (ManagerItem.isRussian)
                NameButton = "Создать";
            else
                NameButton = "Create";
            byte[] imageBytesBuffer;
            using (FileStream fs = new FileStream(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\bear.png", FileMode.Open, FileAccess.Read))
            {
                
                imageBytesBuffer = new byte[fs.Length];
                fs.Read(imageBytesBuffer, 0, imageBytesBuffer.Length);
            }
            imageBytes = imageBytesBuffer;
            Image = GetImage(imageBytes);
        }

        
        public string NameButton
        {
            get;
            set;
        }


        public string MondStart
        {
            get { return mondStart; }
            set
            {
                mondStart = value;
                if(value != null && value != "Выберите" && value != "Choose")
                    table.MondStart = TimeSpan.Parse(value);
            }
        }
        public string MondEnd
        {
            get { return mondEnd; }
            set
            {
                mondEnd = value;
                if (value != null && value != "Выберите" && value != "Choose")
                    table.MondEnd = TimeSpan.Parse(value);
            }
        }
        public string TueStart
        {
            get { return tueStart; }
            set
            {
                tueStart = value;
                if (value != null && value != "Выберите" && value != "Choose")
                    table.TueStart = TimeSpan.Parse(value);
            }
        }
        public string TueEnd
        {
            get { return tueEnd; }
            set
            {
                tueEnd = value;
                if (value != null && value != "Выберите" && value != "Choose")
                    table.TueEnd = TimeSpan.Parse(value);
            }
        }
        public string WenStart
        {
            get { return wenStart; }
            set
            {
                wenStart = value;
                if (value != null && value != "Выберите" && value != "Choose")
                    table.WenStart = TimeSpan.Parse(value);
            }
        }
        public string WenEnd
        {
            get { return wenEnd; }
            set
            {
                wenEnd = value;
                if (value != null && value != "Выберите" && value != "Choose")
                    table.WenEnd = TimeSpan.Parse(value);
            }
        }
        public string ThuStart
        {
            get { return thuStart; }
            set
            {
                thuStart = value;
                if (value != null && value != "Выберите" && value != "Choose")
                    table.ThuStart = TimeSpan.Parse(value);
            }
        }
        public string ThuEnd
        {
            get { return thuEnd; }
            set
            {
                thuEnd = value;
                if (value != null && value != "Выберите" && value != "Choose")
                    table.ThuEnd = TimeSpan.Parse(value);
            }
        }
        public string FriStart
        {
            get { return friStart; }
            set
            {
                friStart = value;
                if (value != null && value != "Выберите" && value != "Choose")
                    table.FriStart = TimeSpan.Parse(value);
            }
        }

        public string FriEnd
        {
            get { return friEnd; }
            set
            {
                friEnd = value;
                if (value != null && value != "Выберите" && value != "Choose")
                    table.FriEnd = TimeSpan.Parse(value);
            }
        }
        public string Exp
        {
            get { return exp; }
            set
            {
                exp = value;
                OnPropertyChanged(nameof(Exp));
            }
        }
        public Brush CorrectName
        {
            get { return correctName; }
            set
            {
                correctName = value;
                OnPropertyChanged("CorrectName");
            }
        }
        public Brush CorrectText
        {
            get { return correctText; }
            set
            {
                correctText = value;
                OnPropertyChanged("CorrectText");
            }
        }
        public Brush CorrectSurname
        {
            get { return correctSurname; }
            set
            {
                correctSurname = value;
                OnPropertyChanged("CorrectSurname");
            }
        }

        public Brush CorrectPatronymic
        {
            get { return correctPatrynomic; }
            set
            {
                correctPatrynomic = value;
                OnPropertyChanged("CorrectPatronymic");
            }
        }
        public Brush CorrectExp
        {
            get { return correctExp; }
            set
            {
                correctExp = value;
                OnPropertyChanged("CorrectExp");
            }
        }

        public BitmapImage Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged("Image"); }
        }


        public Psychologist Psychologist
        {
            get { return psychologist; }
            set 
            {
                psychologist = value;
                OnPropertyChanged("Psychologist");
            }
        }
        public Timetable Table
        {
            get { return table; }
            set 
            {
                table = value;
                OnPropertyChanged("Table");
            }
        }

        public string[] TimeStartMass
        {
            get { return timeStartMass; }
        }
        public string[] TimeEndMass
        {
            get { return timeEndMass; }
        }
        public string[] SpezializationMass
        {
            get { return spezializationMass; }
        }

        public string[] DegreeMas
        {
            get { return degreemas; }
        }

        private RelayCommand delPsy;

        public RelayCommand DelPsy
        {
            get
            {
                return delPsy ?? (delPsy = new RelayCommand(obj =>
                {
                    unit.Booking.DeleteBookingByIdPsy(Psychologist.Id_Psychologist);
                    unit.Voucher.DeleteAllById(Psychologist.Id_Psychologist);
                    unit.Psychologist.Delete(Psychologist.Id_Psychologist);
                    unit.Save();
                    Manager.PreviewFrame.Navigate(new PsychologistsPage());
                }));
            }
        }

        private RelayCommand exitFromPage;

        public RelayCommand ExitFromPage
        {
            get
            {
                return exitFromPage ?? (exitFromPage = new RelayCommand(obj =>
                {
                    if (ManagerItem.FindProc && ManagerItem.Booking)
                        Manager.PreviewFrame.Navigate(new PsychologistsPage(ManagerItem.Proc.Id_Procedure));
                    else
                        Manager.PreviewFrame.Navigate(new PsychologistsPage());
                }));
            }
            
        }

        private RelayCommand createPsy;

        public RelayCommand CreatePsy
        {
            get
            {
                return createPsy ?? (createPsy = new RelayCommand(obj =>
                {
                    if (CorrectName is SolidColorBrush namecolor && CorrectSurname is SolidColorBrush surnameColor && CorrectPatronymic is SolidColorBrush patronymicColor && CorrectExp is SolidColorBrush expColor && CorrectText is SolidColorBrush textColor)
                        if (namecolor.Color.Equals(Colors.Black) && surnameColor.Color.Equals(Colors.Black) && patronymicColor.Color.Equals(Colors.Black) && expColor.Color.Equals(Colors.Black)&& textColor.Color.Equals(Colors.Black) && MondStart != "Выберите" && MondStart != "Choose" && MondEnd != "Выберите" && MondEnd != "Choose" && TueStart != "Выберите" && TueStart != "Choose" && TueEnd != "Выберите" && TueEnd != "Choose" && WenStart != "Выберите" && WenStart != "Choose" && WenEnd != "Выберите" && WenEnd != "Choose" && ThuStart != "Выберите" && ThuStart != "Choose"  && ThuEnd != "Выберите" && ThuEnd != "Choose"  && FriStart != "Выберите" && FriStart != "Choose"  && FriEnd != "Выберите" && FriEnd != "Choose" && Psychologist.Spezialization_Psychologist != "Выберите" && Psychologist.Spezialization_Psychologist != "Choose" && Psychologist.Degree != "Выберите" && Psychologist.Degree != "Choose")
                        {
                            if (!editing)
                            {
                                Psychologist.Photo_Psychologist = imageBytes;
                                List<Psychologist> listPsy = unit.Psychologist.GetAll().ToList();

                                foreach (Psychologist item in listPsy)
                                {
                                    if (item.Name_Psychologist == Psychologist.Name_Psychologist && item.Surname_Psychologist == Psychologist.Surname_Psychologist && item.Patronymic_Psychologist == Psychologist.Patronymic_Psychologist)
                                    {
                                        WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Врач с таким ФИО существует" : "A doctor with such a full name exists");
                                        window.Show();
                                        return;
                                    }
                                }
                                Psychologist.Spezialization_Psychologist = GetCorrectSpec(Psychologist.Spezialization_Psychologist);
                                Psychologist.Degree = GetCorrectDegree(Psychologist.Degree);
                                unit.Psychologist.Create(Psychologist);
                                unit.Save();
                                unit = new UnitOfWork();
                                List<Psychologist> listforFind = unit.Psychologist.GetAll().ToList();
                                foreach (Psychologist item in listforFind)
                                {
                                    if (item.Name_Psychologist == Psychologist.Name_Psychologist && item.Surname_Psychologist == Psychologist.Surname_Psychologist && item.Patronymic_Psychologist == Psychologist.Patronymic_Psychologist)
                                    {
                                        find = item; break;
                                    }
                                }
                                table.Id_Psychologist = find.Id_Psychologist;
                                unit.TimeTable.Create(table);
                                CreateVouchers(find.Id_Psychologist);
                                unit.Save();
                                
                            }
                            else
                            {
                                Psychologist.Photo_Psychologist = imageBytes;
                                Psychologist.Spezialization_Psychologist = GetCorrectSpec(Psychologist.Spezialization_Psychologist);
                                Psychologist.Degree = GetCorrectDegree(Psychologist.Degree);
                                unit.Psychologist.Update(Psychologist);
                                unit.Save();
                                unit.TimeTable.Update(table);
                                unit.Save();
                                unit = new UnitOfWork();
                                CreateVouchers(Psychologist.Id_Psychologist);
                                unit.Save();

                            }
                           
                            Manager.PreviewFrame.Navigate(new PsychologistsPage());
                        }
                        else
                        {
                            WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Введённые данные не валидны" : "The entered data is not valid");
                            window.Show();
                            return;
                        }

                }));
            }
        }

        private RelayCommand changeName;
        public RelayCommand ChangeName
        {
            get
            {
                return changeName ?? (changeName = new RelayCommand(obj =>
                {
                    
                    Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ]{1,15}$");
                    Match match = regex.Match(psychologist.Name_Psychologist);
                    if ((match.Success))
                    {
                        CorrectName = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                    if ((match.Success == false))
                    {
                        CorrectName = new SolidColorBrush(Color.FromRgb(176, 42, 42));
                    }
                }));
            }
        }

        private void CreateVouchers(int id)
        {
            unit.Voucher.DeleteNotOrdered(id);
            DateTime timeNow = DateTime.Now;
            timeNow = timeNow.AddDays(2);
            List<Voucher> currentVouncher = unit.Voucher.GetAll().Where(item => item.Ordered.Contains("Да")).ToList();
            List<Voucher> monVoun = currentVouncher.Where(item => item.Date_Voucher.DayOfWeek == DayOfWeek.Monday).ToList();
            List<Voucher> tueVoun = currentVouncher.Where(item => item.Date_Voucher.DayOfWeek == DayOfWeek.Tuesday).ToList();
            List<Voucher> wenVoun = currentVouncher.Where(item => item.Date_Voucher.DayOfWeek == DayOfWeek.Wednesday).ToList();
            List<Voucher> thuVoun = currentVouncher.Where(item => item.Date_Voucher.DayOfWeek == DayOfWeek.Thursday).ToList();
            List<Voucher> friVoun = currentVouncher.Where(item => item.Date_Voucher.DayOfWeek == DayOfWeek.Friday).ToList();
            DateTime timeEnd = timeNow.AddDays(14);
            
            while (timeNow < timeEnd)
            {
                bool flagAdd = false;
                switch (timeNow.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        {
                            if(timeNow.Hour >= table.MondStart.Value.Hours && timeNow.Hour < table.MondEnd.Value.Hours)
                            {
                                bool flagMon = false;
                                foreach(Voucher item in monVoun)
                                {
                                    if(item.Time_Voucher_Start.Hours == timeNow.Hour)
                                    {
                                        flagMon = true;
                                        break;
                                    }
                                }
                                if (!flagMon)
                                {
                                    Voucher voucher = new Voucher();
                                    voucher.Date_Voucher = timeNow;
                                    voucher.Time_Voucher_Start = new TimeSpan(timeNow.Hour,0,0);
                                    timeNow = timeNow.AddHours(1);
                                    voucher.Time_Voucher_End = new TimeSpan(timeNow.Hour, 0, 0);
                                    voucher.Id_Psychologist = id;
                                    voucher.Ordered = "Нет";
                                    unit.Voucher.Create(voucher);
                                    flagAdd = true;
                                }
                            }
                            break;
                        }
                    case DayOfWeek.Tuesday:
                        {
                            if (timeNow.Hour >= table.TueStart.Value.Hours && timeNow.Hour < table.TueEnd.Value.Hours)
                            {
                                bool flagMon = false;
                                foreach (Voucher item in tueVoun)
                                {
                                    if (item.Time_Voucher_Start.Hours == timeNow.Hour)
                                    {
                                        flagMon = true;
                                        break;
                                    }
                                }
                                if (!flagMon)
                                {
                                    Voucher voucher = new Voucher();
                                    voucher.Date_Voucher = timeNow;
                                    voucher.Time_Voucher_Start = new TimeSpan(timeNow.Hour, 0, 0);
                                    timeNow = timeNow.AddHours(1);
                                    voucher.Time_Voucher_End = new TimeSpan(timeNow.Hour, 0, 0);
                                    voucher.Id_Psychologist = id;
                                    voucher.Ordered = "Нет";
                                    unit.Voucher.Create(voucher);
                                    flagAdd = true;
                                }
                            }
                            break;
                        }
                    case DayOfWeek.Wednesday:
                        {
                            if (timeNow.Hour >= table.WenStart.Value.Hours && timeNow.Hour < table.WenEnd.Value.Hours)
                            {
                                bool flagMon = false;
                                foreach (Voucher item in wenVoun)
                                {
                                    if (item.Time_Voucher_Start.Hours == timeNow.Hour)
                                    {
                                        flagMon = true;
                                        break;
                                    }
                                }
                                if (!flagMon)
                                {
                                    Voucher voucher = new Voucher();
                                    voucher.Date_Voucher = timeNow;
                                    voucher.Time_Voucher_Start = new TimeSpan(timeNow.Hour, 0, 0);
                                    timeNow = timeNow.AddHours(1);
                                    voucher.Time_Voucher_End = new TimeSpan(timeNow.Hour, 0, 0);
                                    voucher.Id_Psychologist = id;
                                    voucher.Ordered = "Нет";
                                    unit.Voucher.Create(voucher);
                                    flagAdd = true;
                                }
                            }
                            break;
                        }
                    case DayOfWeek.Thursday:
                        {
                            if (timeNow.Hour >= table.ThuStart.Value.Hours && timeNow.Hour < table.ThuEnd.Value.Hours)
                            {
                                bool flagMon = false;
                                foreach (Voucher item in thuVoun)
                                {
                                    if (item.Time_Voucher_Start.Hours == timeNow.Hour)
                                    {
                                        flagMon = true;
                                        break;
                                    }
                                }
                                if (!flagMon)
                                {
                                    Voucher voucher = new Voucher();
                                    voucher.Date_Voucher = timeNow;
                                    voucher.Time_Voucher_Start = new TimeSpan(timeNow.Hour, 0, 0);
                                    timeNow = timeNow.AddHours(1);
                                    voucher.Time_Voucher_End = new TimeSpan(timeNow.Hour, 0, 0);
                                    voucher.Id_Psychologist = id;
                                    voucher.Ordered = "Нет";
                                    unit.Voucher.Create(voucher);
                                    flagAdd = true;
                                }
                            }
                            break;
                        }
                    case DayOfWeek.Friday:
                        {
                            if (timeNow.Hour >= table.FriStart.Value.Hours && timeNow.Hour < table.FriEnd.Value.Hours)
                            {
                                bool flagMon = false;
                                foreach (Voucher item in friVoun)
                                {
                                    if (item.Time_Voucher_Start.Hours == timeNow.Hour)
                                    {
                                        flagMon = true;
                                        break;
                                    }
                                }
                                if (!flagMon)
                                {
                                    Voucher voucher = new Voucher();
                                    voucher.Date_Voucher = timeNow;
                                    voucher.Time_Voucher_Start = new TimeSpan(timeNow.Hour, 0, 0);
                                    timeNow = timeNow.AddHours(1);
                                    voucher.Time_Voucher_End = new TimeSpan(timeNow.Hour, 0, 0);
                                    voucher.Id_Psychologist = id;
                                    voucher.Ordered = "Нет";
                                    unit.Voucher.Create(voucher);
                                    flagAdd = true;
                                }
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                if (!flagAdd)
                    timeNow = timeNow.AddHours(1);
            }

        }

        private RelayCommand changeDescription;
        public RelayCommand ChangeDescription
        {
            get
            {
                return changeDescription ?? (changeDescription = new RelayCommand(obj =>
                {
                    Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ0-9\s\-.,!?():;]{1,275}$");
                    Match match = regex.Match(Psychologist.Description);
                    if ((match.Success))
                    {
                        CorrectText = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                    if ((match.Success == false))
                    {
                        CorrectText = new SolidColorBrush(Color.FromRgb(176, 42, 42));
                    }
                }));
            }
        }

        private RelayCommand changeSurname;

        public RelayCommand ChangeSurname
        {
            get
            {
                return changeSurname ?? (changeSurname = new RelayCommand(obj =>
                {
                    Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ]{1,15}$");
                    Match match = regex.Match(psychologist.Surname_Psychologist);
                    if ((match.Success))
                    {
                        CorrectSurname = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                    if ((match.Success == false))
                    {
                        CorrectSurname = new SolidColorBrush(Color.FromRgb(176, 42, 42));
                    }
                }));
            }
        }

        private RelayCommand changePatronymic;

        public RelayCommand ChangePatronymic
        {
            get
            {
                return changePatronymic ?? (changePatronymic = new RelayCommand(obj =>
                {
                    Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ]{1,15}$");
                    Match match = regex.Match(psychologist.Patronymic_Psychologist);
                    if ((match.Success))
                    {
                        CorrectPatronymic = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                    if ((match.Success == false))
                    {
                        CorrectPatronymic = new SolidColorBrush(Color.FromRgb(176, 42, 42));
                    }
                }));
            }
        }

        private RelayCommand changeExp;

        public RelayCommand ChangeExp
        {
            get
            {
                return changeExp ?? (changeExp = new RelayCommand(obj =>
                {
                    Regex regex = new Regex(@"^(?:[1-9]|[1-4][0-9]|50)$");
                    Match match = regex.Match(Exp);
                    if ((match.Success))
                    {
                        CorrectExp = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        Psychologist.Experience = Convert.ToInt16(Exp);
                    }
                    if ((match.Success == false))
                    {
                        CorrectExp = new SolidColorBrush(Color.FromRgb(176, 42, 42));
                    }
                }));
            }
        }

        private RelayCommand changeImage;

        public RelayCommand ChangeImage
        {
            get
            {
                return changeImage ?? (changeImage = new RelayCommand(obj =>
                {
                    string path;
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";

                    if (openFile.ShowDialog() == true)
                    {
                        path = openFile.FileName;
                        byte[] imageBytesBuffer;

                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            imageBytesBuffer = new byte[fs.Length];
                            fs.Read(imageBytesBuffer, 0, imageBytesBuffer.Length);
                        }
                        imageBytes = imageBytesBuffer;
                        Image = GetImage(imageBytesBuffer);
                    }
                }));
            }
        }
        private string GetCorrectSpecView(string value)
        {
            if (ManagerItem.isRussian)
            {
                switch (value)
                {
                    case "Клиническая психология": return "Клиническая";
                    case "Когнитивная психология": return "Когнитивная";
                    case "Развивающая психология": return "Развивающая";
                    case "Социальная психология": return "Социальная";
                    default: return value;
                }
            }
            else
            {
                switch (value)
                {
                    case "Клиническая психология": return "Clinical";
                    case "Когнитивная психология": return "Cognitive";
                    case "Развивающая психология": return "Developing";
                    case "Социальная психология": return "Social";
                    default: return value;
                }
            }

        }
    private string GetCorrectDegreeView(string value)
        {
            if (ManagerItem.isRussian)
            {
                switch (value)
                {
                    case "Бакалавр": return "Балаклавр";
                    case "Магистр": return "Магистр";
                    default: return value;
                }
            }
            else
            {
                switch (value)
                {
                    case "Бакалавр": return "Bachelor";
                    case "Магистр": return "Master";
                    default: return value;
                }
            }
            
        }
        private string GetCorrectSpec(string value)
        {
            if (ManagerItem.isRussian)
            {
                switch (value)
                {
                    case "Клиническая": return "Клиническая психология";
                    case "Когнитивная": return "Когнитивная психология";
                    case "Развивающая": return "Развивающая психология";
                    case "Социальная": return "Социальная психология";
                    default: return value;
                }
            }
            else
            {
                switch (value)
                {
                    case "Clinical": return "Клиническая психология";
                    case "Cognitive": return "Когнитивная психология";
                    case "Developing": return "Развивающая психология";
                    case "Social": return "Социальная психология";
                    default: return value;
                }
            }

        }
        private string GetCorrectDegree(string value)
        {
            if (ManagerItem.isRussian)
            {
                switch (value)
                {
                    case "Балаклавр": return "Бакалавр";
                    case "Магистр": return "Магистр";
                    default: return value;
                }
            }
            else
            {
                switch (value)
                {
                    case "Bachelor": return "Бакалавр";
                    case "Master": return "Магистр";
                    default: return value;
                }
            }
            
        }
        private BitmapImage GetImage(byte[] mass)
        {
            using (var ms = new System.IO.MemoryStream(mass))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
