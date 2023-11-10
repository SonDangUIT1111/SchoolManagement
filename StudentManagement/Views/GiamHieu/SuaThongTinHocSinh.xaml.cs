using StudentManagement.ViewModel.GiamHieu;
using System.Windows;

namespace StudentManagement.Views.GiamHieu
{
    /// <summary>
    /// Interaction logic for SuaThongTinHocSinh.xaml
    /// </summary>
    public partial class SuaThongTinHocSinh : Window
    {
        public SuaThongTinHocSinh()
        {
            InitializeComponent();
            DataContext = new SuaThongTinHocSinhViewModel();
        }
    }
}
