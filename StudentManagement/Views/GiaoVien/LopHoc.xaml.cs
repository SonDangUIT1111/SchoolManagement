using StudentManagement.ViewModel.GiaoVien;
using System.Windows.Controls;

namespace StudentManagement.Views.GiaoVien
{
    /// <summary>
    /// Interaction logic for LopHoc.xaml
    /// </summary>
    public partial class LopHoc : Page
    {
        public LopHoc()
        {
            InitializeComponent();
            DataContext = new LopHocViewModel();
        }
    }
}
