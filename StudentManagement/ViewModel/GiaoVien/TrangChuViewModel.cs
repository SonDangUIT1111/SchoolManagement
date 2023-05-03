using StudentManagement.Views.GiaoVien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiaoVien
{
    internal class TrangChuViewModel:BaseViewModel
    {
        //declare variable
        private int _idGiaoVien;
        public int IdGiaoVien { get { return _idGiaoVien; } set { _idGiaoVien = value; } }

        //declare Pages
        public BaoCao BaoCaoPage { get; set; }
        public LopHoc LopHocPage { get; set; }
        public ThongTinGiaoVien ThongTinGiaoVienPage { get; set; }
        public ThongTinHocSinh ThongTinHocSinhPage { get; set; }
        public ThongTinTruong ThongTinTruongPage { get; set; }

        //declare ICommand
        public ICommand SwitchThongTinHocSinh { get; set; }
        public ICommand SwitchThongTinGiaoVien { get; set; }
        public ICommand SwitchLopHoc { get; set; }
        public ICommand SwitchThongTinTruong { get; set; }
        public ICommand SwitchBaoCao { get; set; }

        public TrangChuViewModel()
        {
            IdGiaoVien = 100000;
            BaoCaoPage = new BaoCao();
            LopHocPage = new LopHoc();
            ThongTinGiaoVienPage = new ThongTinGiaoVien();

            ThongTinTruongPage = new ThongTinTruong();

            //define ICommand

            SwitchThongTinGiaoVien = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = ThongTinGiaoVienPage;
            });
            SwitchLopHoc = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                StudentManagement.ViewModel.GiaoVien.LopHocViewModel vm = LopHocPage.DataContext as StudentManagement.ViewModel.GiaoVien.LopHocViewModel;
                vm.IdGiaoVien = IdGiaoVien;
                parameter.Content = LopHocPage;
            });
            SwitchThongTinTruong = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = ThongTinTruongPage;
            });
            SwitchBaoCao = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = BaoCaoPage;
            });

        }
    }
}
