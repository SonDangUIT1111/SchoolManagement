using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudentManagement.Views.Menu
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void TrangChu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ThongTinHocSinh_Click(object sender, RoutedEventArgs e)
        {
            Page.Content = new ThongTinHocSinh();
        }

        private void ThongTinGiaoVien_Click(object sender, RoutedEventArgs e)
        {
            Page.Content = new ThongTinGiaoVien();
        }

        private void LopHoc_Click(object sender, RoutedEventArgs e)
        {
            Page.Content = new LopHoc();
        }

        private void ThongTinTruong_Click(object sender, RoutedEventArgs e)
        {
            Page.Content = new ThongTinTruong();
        }

        private void BaoCao_Click(object sender, RoutedEventArgs e)
        {
            Page.Content = new BaoCao();
        }

        private void ThayDoiQuyDinh_Click(object sender, RoutedEventArgs e)
        {
            Page.Content = new ThayDoiQuyDinh();
        }
    }
}
