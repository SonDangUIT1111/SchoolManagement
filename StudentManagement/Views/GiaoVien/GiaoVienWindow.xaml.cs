using System.Windows;

namespace StudentManagement.Views.GiaoVien
{
    /// <summary>
    /// Interaction logic for GiaoVienWindow.xaml
    /// </summary>
    public partial class GiaoVienWindow : Window
    {
        public GiaoVienWindow()
        {
            InitializeComponent();
            DataContext = new StudentManagement.ViewModel.GiaoVien.TrangChuViewModel();
        }
    }
}
