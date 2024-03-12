using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.Pages;
using CleanBrain.UoF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CleanBrain.MVVM
{
    public class ProcedureModel : INotifyPropertyChanged
    {
        private byte[] imageBytes;
        private BitmapImage image;
        private Procedure proc = new Procedure();
        private Brush correctName = new SolidColorBrush(Color.FromRgb(176, 42, 42));
        private Brush correctCost = new SolidColorBrush(Color.FromRgb(176, 42, 42));
        private Brush correctText = new SolidColorBrush(Color.FromRgb(176, 42, 42));
        private string[] spezializationMass = ManagerItem.isRussian ? new string[] { "Выберите", "Клиническая", "Когнитивная", "Развивающая", "Социальная" } : new string[] { "Choose", "Clinical", "Cognitive", "Developing", "Social" };
        private UnitOfWork unit = new UnitOfWork();
        private string name_Procedure;
        private string cost;
        private bool activeProc = false;
        private bool comboActive = true;
        private bool visMain = true;
        private bool visDelete = false;
        private bool editing = false;

        public bool ComboActive
        {
            get { return comboActive; }
            set { comboActive = value; OnPropertyChanged("ComboActive"); }
        }
        public bool ActiveProc
        {
            get { return activeProc; }
            set { activeProc = value; OnPropertyChanged("ActiveProc"); }
        }
        

        public ProcedureModel()
        {

            if (ManagerItem.isRussian)
            {
                NameButton = "Создать";
                Proc.Spezialization_Procedure = "Выберите";
            }
            else
            {
                NameButton = "Create";
                Proc.Spezialization_Procedure = "Choose";
            }
            byte[] imageBytesBuffer;
            using (FileStream fs = new FileStream(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\happyFamily2.jpg", FileMode.Open, FileAccess.Read))
            {

                imageBytesBuffer = new byte[fs.Length];
                fs.Read(imageBytesBuffer, 0, imageBytesBuffer.Length);
            }
            imageBytes = imageBytesBuffer;
            Image = GetImage(imageBytes);
        }
        private void getActive()
        {
            if (!ActiveProc)
            {
                VisDelete = true;
                ComboActive = true;
            }
            else
            {
                VisDelete = false;
                ComboActive = false;
            }
        }
        public ProcedureModel(int id)
        {
            ActiveProc = ManagerItem.IsReadOnly;
            if (ManagerItem.ImClient)
                ActiveProc = true;
            getActive();
            CorrectName = new SolidColorBrush(Color.FromRgb(0,0,0));
            CorrectCost = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            CorrectText = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            if (ManagerItem.isRussian)
                NameButton = "Изменить";
            else
                NameButton = "Editing";
            Proc = unit.Procedure.Get(id);
            Proc.Spezialization_Procedure = GetCorrectSpecView(Proc.Spezialization_Procedure);
            imageBytes = Proc.Photo_Procedure;
            Image = GetImage(imageBytes);
            NameProcedure = Proc.Name_Procedure;
            Cost = Proc.Price.ToString();
            editing = true;
            OnPropertyChanged("Depiction");
        }

        public bool VisDelete
        {
            get { return visDelete; }
            set { visDelete = value; }
        }


        public string NameButton
        {
            get;
            set;
        }

        public string[] SpezializationMass
        {
            get { return spezializationMass; }
        }
        public string Cost
        {
            get { return cost; }
            set 
            { 
                cost = value; 
                OnPropertyChanged("Cost");
            }
        }

        public string NameProcedure
        {
            get { return name_Procedure; }
            set 
            { 
                name_Procedure = value;
                OnPropertyChanged("NameProcedure");
            }
        }
        public Brush CorrectCost
        {
            get { return correctCost; }
            set
            {
                correctCost = value;
                OnPropertyChanged("CorrectCost");
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
        public Procedure Proc
        {
            get { return proc; }
            set 
            { 
                proc = value;
                OnPropertyChanged("Proc");
            }
        }
        public BitmapImage Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged("Image"); }
        }

        private RelayCommand createProc;

        public RelayCommand CreateProc
        {
            get
            {
                return createProc ?? (createProc = new RelayCommand(obj =>
                {
                    if (CorrectName is SolidColorBrush namecolor && CorrectText is SolidColorBrush textColor && CorrectCost is SolidColorBrush costColor)
                        if (namecolor.Color.Equals(Colors.Black) && textColor.Color.Equals(Colors.Black) && costColor.Color.Equals(Colors.Black) && Proc.Spezialization_Procedure != "Выберите" && Proc.Spezialization_Procedure != "Choose")
                        {
                            if (!editing)
                            {
                                Proc.Photo_Procedure = imageBytes;
                                List<Procedure> listProc = unit.Procedure.GetAll().ToList();

                                foreach (Procedure item in listProc)
                                {
                                    if (item.Name_Procedure == NameProcedure)
                                    {
                                        WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Процедура с таким именем существует" : "A procedure with this name exists");
                                        window.Show();
                                        return;
                                    }
                                }
                                Proc.Spezialization_Procedure = GetCorrectSpec(Proc.Spezialization_Procedure);
                                Proc.Name_Procedure = NameProcedure;
                                Proc.Price = Convert.ToInt32(Cost);
                                unit.Procedure.Create(Proc);
                                unit.Save();
                            }
                            else
                            {
                                Proc.Photo_Procedure = imageBytes;
                                Proc.Spezialization_Procedure = GetCorrectSpec(Proc.Spezialization_Procedure);
                                Proc.Name_Procedure = NameProcedure;
                                Proc.Price = Convert.ToInt32(Cost);
                                unit.Procedure.Update(Proc);
                                unit.Save();
                            }
                            Manager.ReviewFrame.Content = null;
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

        private RelayCommand delProc;

        public RelayCommand DelProc
        {
            get
            {
                return delProc ?? (delProc = new RelayCommand(obj =>
                {
                    List<Booking> prov = unit.Booking.GetAll().Where(item => item.Id_Procedure == ManagerItem.Proc.Id_Procedure).ToList();
                    if(prov != null)
                    {
                        foreach(Booking book in prov)
                        {
                            unit.Voucher.Delete(book.Id_Voucher);
                            unit.Booking.DeleteBooking(book);
                        }
                    }
                    unit.Procedure.Delete(Proc.Id_Procedure);
                    unit.Save();
                    Manager.ReviewFrame.Content = null;
                }));
            }
        }

        private RelayCommand changeText;
        public RelayCommand ChangeText
        {
            get
            {
                return changeText ?? (changeText = new RelayCommand(obj =>
                {

                    Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ0-9\s\-.,!?():;]{1,122}$");
                    Match match = regex.Match(Proc.Depiction);
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
        private RelayCommand changeCost;
        public RelayCommand ChangeCost
        {
            get
            {
                return changeCost ?? (changeCost = new RelayCommand(obj =>
                {

                    Regex regex = new Regex(@"^(?:[1-9]\d{0,3}|10000)$");
                    Match match = regex.Match(Cost);
                    if ((match.Success))
                    {
                        CorrectCost = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                    if ((match.Success == false))
                    {
                        CorrectCost = new SolidColorBrush(Color.FromRgb(176, 42, 42));
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

                    Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ0-9\s\-.,!?():;]{1,20}$");
                    Match match = regex.Match(NameProcedure);
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
