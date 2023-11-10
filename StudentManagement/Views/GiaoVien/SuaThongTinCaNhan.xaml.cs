using StudentManagement.ViewModel.GiaoVien;
using System.Windows;

namespace StudentManagement.Views.GiaoVien
{
    /// <summary>
    /// Interaction logic for SuaThongTinCaNhan.xaml
    /// </summary>
    public partial class SuaThongTinCaNhan : Window
    {
        public SuaThongTinCaNhan()
        {
            InitializeComponent();
            DataContext = new SuaHocSinhViewModel();
        }
    }
}
