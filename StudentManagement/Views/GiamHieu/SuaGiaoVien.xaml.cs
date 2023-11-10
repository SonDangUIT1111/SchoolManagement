using System.Windows;
using StudentManagement.ViewModel.GiamHieu;

namespace StudentManagement.Views.GiamHieu
{
    /// <summary>
    /// Interaction logic for SuaGiaoVien.xaml
    /// </summary>
    public partial class SuaGiaoVien : Window
    {
        public SuaGiaoVien()
        {
            InitializeComponent();
            DataContext = new SuaGiaoVienViewModel();
        }

        private void TenGV_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
