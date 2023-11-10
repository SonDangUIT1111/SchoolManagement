using StudentManagement.ViewModel.GiamHieu;
using System.Windows.Controls;

namespace StudentManagement.Views.GiamHieu
{
    /// <summary>
    /// Interaction logic for QuanLiDiemSo.xaml
    /// </summary>
    public partial class QuanLiDiemSo : Page
    {
        public QuanLiDiemSo()
        {
            InitializeComponent();
            DataContext = new QuanLiDiemSoViewModel();
        }
    }
}
