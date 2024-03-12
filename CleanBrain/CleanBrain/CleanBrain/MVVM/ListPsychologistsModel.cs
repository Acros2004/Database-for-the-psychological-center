using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.Pages;
using CleanBrain.UoF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CleanBrain.MVVM
{
    public class ListPsychologistsModel : INotifyPropertyChanged
    {
        private Psychologist selectPsy = new Psychologist();
        private List<Psychologist> _list;
        private UnitOfWork unit = new UnitOfWork();
        private string[] spezializationMass = ManagerItem.isRussian ? new string[] { "Выберите", "Клиническая", "Когнитивная", "Развивающая", "Социальная" } : new string[] { "Choose", "Clinical", "Cognitive", "Developing", "Social" };
        private string spec = ManagerItem.isRussian ? "Выберите" : "Choose";
        private string search = ManagerItem.isRussian ? "Поиск" : "Search";
        private List<Psychologist> searchList;
        private List<Psychologist> temp;
        private bool findProc = false;
        private bool active = true;
        private bool activeCombo = true;
        private string logoText = ManagerItem.isRussian ? "Наши психологи" : "Our psychologists";
        private bool isFrameVisible;

        public bool ActiveCombo
        {
            get { return activeCombo; }
            set { activeCombo = value; OnPropertyChanged("ActiveCombo"); }
        }
        public bool IsFrameVisible
        {
            get { return isFrameVisible; }
            set
            {
                isFrameVisible = value;
                OnPropertyChanged("IsFrameVisible");
            }
        }

        public string LogoText
        {
            get { return logoText; }
            set { logoText = value; OnPropertyChanged("LogoText"); }
        }
        public bool Active
        {
            get { return active; }
            set { active = value; OnPropertyChanged("Active"); }
        }
        public string Spec
        {
            get { return spec; }
            set { spec = value; OnPropertyChanged("Spec"); }
        }
        public string Search
        {
            get { return search; }
            set { search = value; OnPropertyChanged("Search"); }
        }
        public string[] SpezializationMass
        {
            get { return spezializationMass; }
        }
        public Psychologist SelectPsy
        {
            get { return selectPsy; }
            set { selectPsy = value; OnPropertyChanged("SelectPsy"); ManagerItem.Psy = value; }
        }
        public List<Psychologist> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged("List"); }
        }

        public ListPsychologistsModel()
        {
            ManagerItem.IsReadOnly = false;
            if (ManagerItem.ImClient)
                Active = false;
            ActiveCombo = true;
            Manager.ReviewFrame.Content = null;
            ManagerItem.Psy = null;
            List = new List<Psychologist>(unit.Psychologist.GetAll());
            searchList = List;
            ManagerItem.FindProc = false;
            ManagerItem.Booking = false;
        }
        public ListPsychologistsModel(int id)
        {
            ManagerItem.IsReadOnly = true;
            Manager.ReviewFrame.Content = null;
            if (ManagerItem.isRussian)
                LogoText = "Выберите психолога";
            else
                LogoText = "Choose a psychologist";
            Procedure psy = unit.Procedure.Get(id);
            List = unit.Psychologist.GetAll().Where(item => item.Spezialization_Psychologist.Contains(psy.Spezialization_Procedure)).ToList();
            Spec = GetCorrectSpecView(psy.Spezialization_Procedure);
            searchList = List;
            ManagerItem.FindProc = true;
            ManagerItem.Booking = true;
            Active = false;
            ActiveCombo = false;
        }

        

        private RelayCommand openEdit;

        public RelayCommand OpenEdit
        {
            get
            {
                return openEdit ?? (openEdit = new RelayCommand(obj =>
                {
                    
                    //MessageBox.Show($"{ManagerItem.Psy.Id_Psychologist}");
                   Manager.PreviewFrame.Navigate(new DoctorPage(ManagerItem.Psy.Id_Psychologist));
                }));
            }
        }

        private RelayCommand addPsychologist;

        public RelayCommand AddPsychologist
        {
            get
            {
                return addPsychologist ?? (addPsychologist = new RelayCommand(obj =>
                {
                    Manager.PreviewFrame.Navigate(new DoctorPage());
                }));
            }
        }

        private RelayCommand searchChange;
        public RelayCommand SearchChange
        {
            get
            {
                return searchChange ?? (searchChange = new RelayCommand(obj =>
                {
                    temp = searchList;
                    if((Spec == "Выберите" && Search == "Поиск") || (Spec == "Choose" && Search == "Search") || (Spec == "Выберите" && Search == "") || (Spec == "Choose" && Search ==""))
                    {
                        List = searchList;
                        return;
                    }
                    if(Spec != "Выберите" && Spec != "Choose")
                    {
                        string tempValue = GetCorrectSpec(Spec);
                        temp = temp.Where(item => item.Spezialization_Psychologist.Contains(tempValue)).ToList();
                    }
                    if(Search != "Поиск" && Search != "" && Search != "Search")
                    {
                        temp = temp.Where(person =>
                        person.Name_Psychologist.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        person.Surname_Psychologist.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        person.Patronymic_Psychologist.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    }
                    List = temp;
                }));
            }
        }
        private RelayCommand searchGot;
        public RelayCommand SearchGot
        {
            get
            {
                return searchGot ?? (searchGot = new RelayCommand(obj =>
                {
                    if (Search == "Поиск" || Search == "Search")
                    {
                        Search = "";
                    }
                }));
            }
        }

        private RelayCommand searchLost;
        public RelayCommand SearchLost
        {
            get
            {
                return searchLost ?? (searchLost = new RelayCommand(obj =>
                {
                    if (string.IsNullOrWhiteSpace(Search))
                    {
                        if (ManagerItem.isRussian)
                            Search = "Поиск";
                        else
                            Search = "Search";
                    }
                }));
            }
        }

        private RelayCommand closeBorderClick;

        public RelayCommand CloseBorderClick
        {
            get
            {
                return closeBorderClick ?? (closeBorderClick = new RelayCommand(obj =>
                {
                    Manager.ReviewFrame.Content = null;
                    IsFrameVisible = false;
                }));
            }
        }

        private RelayCommand closeBorder;

        public RelayCommand CloseBorder
        {
            get
            {
                return closeBorder ?? (closeBorder = new RelayCommand(obj =>
                {
                    if (Manager.ReviewFrame.Content == null)
                    {
                        IsFrameVisible = false;
                    }
                    if (Manager.ReviewFrame.Content != null && ManagerItem.Psy != null)
                    {
                        IsFrameVisible = true;
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
