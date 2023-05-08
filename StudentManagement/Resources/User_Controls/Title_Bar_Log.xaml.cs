using StudentManagement.ViewModel.Title_Bar_ViewModels;
using System.Windows.Controls;

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
