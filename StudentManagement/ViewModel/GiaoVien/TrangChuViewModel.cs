using StudentManagement.Model;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.HocSinh;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private byte[] _avatar;
        public byte[] Avatar { get { return _avatar; } set { _avatar = value; OnPropertyChanged(); } }
        private string _tenGiaoVien;
        public string TenGiaoVien { get { return _tenGiaoVien; } set { _tenGiaoVien = value; OnPropertyChanged(); } }
        public GiaoVienWindow GiaoVienWD { get; set; }
        //declare Pages
        public StudentManagement.Views.GiaoVien.BaoCao BaoCaoPage { get; set; }
        public StudentManagement.Views.GiaoVien.LopHoc LopHocPage { get; set; }
        public StudentManagement.Views.GiaoVien.ThongTinGiaoVien ThongTinGiaoVienPage { get; set; }
        public StudentManagement.Views.GiaoVien.ThongTinHocSinh ThongTinHocSinhPage { get; set; }
        public StudentManagement.Views.GiaoVien.ThongTinTruong ThongTinTruongPage { get; set; }

        //declare ICommand
        public ICommand LoadWindow { get; set; }
        public ICommand SwitchThongTinHocSinh { get; set; }
        public ICommand SwitchThongTinGiaoVien { get; set; }
        public ICommand SwitchLopHoc { get; set; }
        public ICommand SwitchThongTinTruong { get; set; }
        public ICommand SwitchBaoCao { get; set; }
        public ICommand SuaThongTinCaNhan { get; set; }
        public TrangChuViewModel()
        {
            //IdGiaoVien = 100000;
            BaoCaoPage = new StudentManagement.Views.GiaoVien.BaoCao();
            LopHocPage = new StudentManagement.Views.GiaoVien.LopHoc();
            ThongTinGiaoVienPage = new StudentManagement.Views.GiaoVien.ThongTinGiaoVien();
            ThongTinTruongPage = new StudentManagement.Views.GiaoVien.ThongTinTruong();

            //define ICommand
            LoadWindow = new RelayCommand<GiaoVienWindow>((parameter) => { return true; }, (parameter) =>
            {
                GiaoVienWD = parameter;
                LoadThongTinCaNhan();
            });

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
            SuaThongTinCaNhan = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {

                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString = "select NgaySinh,GioiTinh,DiaChi,Email from GiaoVien where MaGiaoVien = " + IdGiaoVien;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Model.GiaoVien teacher = new Model.GiaoVien() {
                            MaGiaoVien = IdGiaoVien,
                            TenGiaoVien = TenGiaoVien,
                            NgaySinh = reader.GetDateTime(0),
                            GioiTinh = reader.GetBoolean(1),
                            DiaChi = reader.GetString(2),
                            Email = reader.GetString(3),
                            Avatar = Avatar
                        };
                        SuaGiaoVien window = new SuaGiaoVien();
                        SuaGiaoVienViewModel data = window.DataContext as SuaGiaoVienViewModel;
                        data.GiaoVienHienTai = teacher;
                        window.ShowDialog();
                        LoadThongTinCaNhan();
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
            });

        }
        public void LoadThongTinCaNhan()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select TenGiaoVien,AnhThe from GiaoVien where MaGiaoVien = " + IdGiaoVien.ToString();
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows) reader.Read();
                TenGiaoVien= reader.GetString(0);
                Avatar = (byte[])reader.GetValue(1);
                con.Close();
            }
        }
    }
}
