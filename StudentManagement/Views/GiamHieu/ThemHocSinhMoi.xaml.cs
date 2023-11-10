using System.Windows;
using StudentManagement.ViewModel.GiamHieu;


namespace StudentManagement.Views.GiamHieu
{
    /// <summary>
    /// Interaction logic for ThemHocSinhMoi.xaml
    /// </summary>
    public partial class ThemHocSinhMoi : Window
    {
        public ThemHocSinhMoi()
        {
            InitializeComponent();
            DataContext = new ThemHocSinhMoiViewModel();
        }
    }
}
