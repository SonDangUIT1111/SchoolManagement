using System.Windows.Controls;
using StudentManagement.ViewModel.GiamHieu;

namespace StudentManagement.Views.GiamHieu
{
    /// <summary>
    /// Interaction logic for BaoCaoHocKy.xaml
    /// </summary>
    public partial class BaoCaoTongKetHocKy : Page
    {
        public BaoCaoTongKetHocKy()
        {
            InitializeComponent();
            DataContext = new BaoCaoHocKyViewModel();
        }
    }
}
