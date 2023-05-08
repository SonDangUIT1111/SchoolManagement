using StudentManagement.Model;
using StudentManagement.ViewModel.GiaoVien;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.HocSinh;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.HocSinh
{
    internal class TrangChuViewModel : BaseViewModel
    {
        //declare variable
        public HocSinhWindow HocSinhWD { get; set; }
        private int _idHocSinh;
        public int IdHocSinh { get { return _idHocSinh; } set { _idHocSinh = value; } }

        private byte[] _avatar;
        public byte[] Avatar { get { return _avatar; } set { _avatar = value; OnPropertyChanged(); } }
        private string _tenHocSinh;
        public string TenHocSinh { get { return _tenHocSinh; } set { _tenHocSinh = value; OnPropertyChanged(); } }
        //declare Pages
        public StudentManagement.Views.HocSinh.LopHoc LopHocPage { get; set; }
        public StudentManagement.Views.HocSinh.ThongTinHocSinh ThongTinHocSinhPage { get; set; }
        public StudentManagement.Views.HocSinh.ThongTinTruong ThongTinTruongPage { get; set; }
        public DiemSo XemDiemPage { get; set; }


        //declare ICommand
        public ICommand LoadWindow { get; set; }
        public ICommand SwitchThongTinHocSinh { get; set; }
        public ICommand SwitchLopHoc { get; set; }
        public ICommand SwitchThongTinTruong { get; set; }
        public ICommand SwitchXemDiem { get; set; }
        public ICommand CapNhatThongTin { get; set; }

        public TrangChuViewModel()
        {
            //IdHocSinh = 100000;


            LopHocPage = new StudentManagement.Views.HocSinh.LopHoc();
            ThongTinHocSinhPage = new StudentManagement.Views.HocSinh.ThongTinHocSinh();
            ThongTinTruongPage = new StudentManagement.Views.HocSinh.ThongTinTruong();

            //define ICommand
            LoadWindow = new RelayCommand<HocSinhWindow>((parameter) => { return true; }, (parameter) =>
            {
                HocSinhWD = parameter;
                LoadThongTinCaNhan();
            });
            SwitchThongTinHocSinh = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = ThongTinHocSinhPage;
            });
            SwitchLopHoc = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = LopHocPage;
            });
            SwitchThongTinTruong = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = ThongTinTruongPage;
            });
            SwitchXemDiem = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                XemDiemPage = new DiemSo();
                StudentManagement.ViewModel.HocSinh.DiemSoViewModel vm = XemDiemPage.DataContext as StudentManagement.ViewModel.HocSinh.DiemSoViewModel;
                vm.IdHocSinh = IdHocSinh;
                parameter.Content = XemDiemPage;
            });
            CapNhatThongTin = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                    string CmdString = "select NgaySinh,GioiTinh,DiaChi,Email,AnhThe from HocSinh where MaHocSinh = " + IdHocSinh;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh()
                            {
                                MaHocSinh = IdHocSinh,
                                TenHocSinh = TenHocSinh,
                                NgaySinh = reader.GetDateTime(0),
                                GioiTinh = reader.GetBoolean(1),
                                DiaChi = reader.GetString(2),
                                Email = reader.GetString(3),
                                Avatar = (byte[])reader.GetValue(4)
                            };
                            SuaHocSinh window = new SuaHocSinh();
                            SuaHocSinhViewModel data = window.DataContext as SuaHocSinhViewModel;
                            data.HocSinhHienTai = student;
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
                try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                string CmdString = "select TenHocSinh,AnhThe from HocSinh where MaHocSinh = " + IdHocSinh.ToString();
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows) reader.Read();
                TenHocSinh = reader.GetString(0);
                Avatar = (byte[])reader.GetValue(1);
                con.Close();
            }
        }
    }
}
