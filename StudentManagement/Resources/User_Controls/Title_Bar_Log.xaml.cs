using StudentManagement.ViewModel.Title_Bar_ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManagement.Resources.User_Controls
{
    /// <summary>
    /// Interaction logic for Title_Bar_Log.xaml
    /// </summary>
    public partial class Title_Bar_Log : UserControl
    {
        public Title_Bar_Log_ViewModel ViewModelLog { get; set; }
        public Title_Bar_Log()
        {
            InitializeComponent();
            this.DataContext = ViewModelLog = new Title_Bar_Log_ViewModel();
        }
    }
}
