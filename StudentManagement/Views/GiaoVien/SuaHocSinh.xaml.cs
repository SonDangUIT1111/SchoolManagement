using StudentManagement.ViewModel.GiaoVien;
using System.Windows;

namespace StudentManagement.Views.GiaoVien
{
    /// <summary>
    /// Interaction logic for SuaHocSinh.xaml
    /// </summary>
    public partial class SuaHocSinh : Window
    {
        public SuaHocSinh()
        {
            InitializeComponent();
            DataContext = new SuaHocSinhViewModel();
        }
    }
}
