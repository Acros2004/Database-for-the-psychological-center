using CleanBrain.Command;
using CleanBrain.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CleanBrain.ManagerFrame
{
    class ManagerItem
    {
        public static Psychologist Psy { get; set; }
        public static Procedure Proc { get; set; }
        public static Booking Book { get; set; }
        public static Review Rev { get; set; }

        public static bool ImClient { get; set; }

        public static bool IsReadOnly { get; set; }

        public static int MainId { get; set; }

        public static bool Booking { get; set; }

        public static bool FindProc { get; set; }
        public static bool FindPsy { get; set;}

        private static bool isrussian = true;
        public static bool isRussian 
        { 
            get { return isrussian; }
            set { isrussian = value; }
        }
        public static string PasswordCheck { get; set; }       
        public static bool ImGuest { get; set; }
    }
}
