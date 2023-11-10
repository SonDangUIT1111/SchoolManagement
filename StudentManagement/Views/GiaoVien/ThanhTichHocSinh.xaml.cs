using StudentManagement.ViewModel.GiaoVien;
using System.Windows.Controls;

namespace StudentManagement.Views.GiaoVien
{
    /// <summary>
    /// Interaction logic for ThanhTichHocSinh.xaml
    /// </summary>
    public partial class ThanhTichHocSinh : Page
    {
        public ThanhTichHocSinh()
        {
            InitializeComponent();
            DataContext = new ThanhTichHocSinhViewModel();
        }
    }
}
