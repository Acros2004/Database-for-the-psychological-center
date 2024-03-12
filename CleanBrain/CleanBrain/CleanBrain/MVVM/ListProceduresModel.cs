using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.Pages;
using CleanBrain.UoF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrain.MVVM
{
    public class ListProceduresModel : INotifyPropertyChanged
    {
        private Procedure selectProc = new Procedure();
        private List<Procedure> _list;
        private UnitOfWork unit = new UnitOfWork();
        private bool isFrameVisible = false;
        private string[] spezializationMass = ManagerItem.isRussian ? new string[] { "Выберите", "Клиническая", "Когнитивная", "Развивающая", "Социальная" } : new string[] { "Choose", "Clinical", "Cognitive", "Developing", "Social" };
        private string spec = ManagerItem.isRussian ? "Выберите" : "Choose";
        private string search = ManagerItem.isRussian ? "Поиск" : "Search";
        private string searchCost = ManagerItem.isRussian ? "Цена" : "Cost";
        private List<Procedure> searchList;
        private List<Procedure> temp;
        private bool findPsy = false;
        private bool active = true;
        private bool activeCombo = true;

        public bool ActiveCombo
        {
            get { return activeCombo; }
            set { activeCombo = value; OnPropertyChanged("ActiveCombo"); }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; OnPropertyChanged("active"); }
        }

        public bool IsFrameVisible
        {
            get { return isFrameVisible; }
            set { isFrameVisible = value; OnPropertyChanged("IsFrameVisible"); }
        }
        public string SearchCost
        {
            get { return searchCost; }
            set { searchCost = value; OnPropertyChanged("SearchCost"); }
        }

        public Procedure SelectProc
        {
            get { return selectProc; }
            set { selectProc = value; OnPropertyChanged("SelectProc"); ManagerItem.Proc = value; }
        }
        public List<Procedure> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged("List"); }
        }

        public ListProceduresModel()
        {
            if (ManagerItem.ImClient)
                Active = false;
            ActiveCombo = true;
            ManagerItem.IsReadOnly = false;
            Manager.ReviewFrame.Content = null;
            ManagerItem.Proc = null;
            List = unit.Procedure.GetAll().ToList();
            searchList = List;
            ManagerItem.FindPsy = false;
            ManagerItem.Booking = false;
            
        }
        public ListProceduresModel(int id)
        {
            ManagerItem.IsReadOnly = true;
            ActiveCombo = false;
            Manager.ReviewFrame.Content = null;
            Psychologist psy = unit.Psychologist.Get(id);
            List = unit.Procedure.GetAll().Where(item => item.Spezialization_Procedure.Contains(psy.Spezialization_Psychologist)).ToList();
            Spec = GetCorrectSpecView(psy.Spezialization_Psychologist);
            searchList = List;
            findPsy = true;
            ManagerItem.FindPsy = true;
            Active = false;
            ManagerItem.Booking = true;
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
                        if (!ManagerItem.Booking)
                        {
                            unit = new UnitOfWork();
                            List = new List<Procedure>(unit.Procedure.GetAll());
                        }    
                    }
                    if(Manager.ReviewFrame.Content != null && ManagerItem.Proc != null)
                    {
                        IsFrameVisible = true;
                    }

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
                    if ((Spec == "Выберите" && Search == "Поиск" && SearchCost == "Цена") || (Spec == "Choose" && Search == "Search" && SearchCost == "Cost") || (Spec == "Выберите" && Search == "" && SearchCost == "Цена") || (Spec == "Choose" && Search == "" && SearchCost == "Cost") || (Spec == "Выберите" && Search == "" && SearchCost == "") || (Spec == "Choose" && Search == "" && SearchCost == ""))
                    {
                        List = searchList;
                        return;
                    }
                    if (Spec != "Выберите" && Spec != "Choose")
                    {
                        string tempValue = GetCorrectSpec(Spec);
                        temp = temp.Where(item => item.Spezialization_Procedure.Contains(tempValue)).ToList();
                    }
                    if (Search != "Поиск" && Search != "" && Search != "Search")
                    {
                        temp = temp.Where(person =>
                        person.Name_Procedure.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    }
                    if(SearchCost != "Цена" && SearchCost != "" && SearchCost != "Cost")
                    {
                        if (Int32.TryParse(SearchCost, out int numValue))
                        {
                            temp = temp.Where(item => item.Price >= numValue).ToList();
                        }
                        else
                        {
                            temp = null;
                        }
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
        private RelayCommand searchCostGot;
        public RelayCommand SearchCostGot
        {
            get
            {
                return searchCostGot ?? (searchCostGot = new RelayCommand(obj =>
                {
                    if (SearchCost == "Цена" || SearchCost == "Cost")
                    {
                        SearchCost = "";
                    }
                }));
            }
        }

        private RelayCommand searchCostLost;
        public RelayCommand SearchCostLost
        {
            get
            {
                return searchCostLost ?? (searchCostLost = new RelayCommand(obj =>
                {
                    if (string.IsNullOrWhiteSpace(SearchCost))
                    {
                        if (ManagerItem.isRussian)
                            SearchCost = "Цена";
                        else
                            SearchCost = "Cost";
                    }
                }));
            }
        }


        private RelayCommand openEdit;

        public RelayCommand OpenEdit
        {
            get
            {
                return openEdit ?? (openEdit = new RelayCommand(obj =>
                {

                    
                }));
            }
        }

        private RelayCommand addProc;

        public RelayCommand AddProc
        {
            get
            {
                return addProc ?? (addProc = new RelayCommand(obj =>
                {
                    Manager.ReviewFrame.Navigate(new ProcedurePage());
                    IsFrameVisible = true;
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
