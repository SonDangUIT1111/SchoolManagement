using StudentManagement.ViewModel.HocSinh;
using System.Windows.Controls;

namespace StudentManagement.Views.HocSinh
{
    /// <summary>
    /// Interaction logic for DiemSo.xaml
    /// </summary>
    public partial class DiemSo : Page
    {
        public DiemSo()
        {
            InitializeComponent();
            DataContext = new DiemSoViewModel();
        }
    }
}
