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

        private Model.HocSinh _hocSinhHienTai;
        public Model.HocSinh HocSinhHienTai { get { return _hocSinhHienTai;} set { _hocSinhHienTai = value; } }
        //declare Pages
        public StudentManagement.Views.GiamHieu.BaoCaoMonHoc BaoCaoPage { get; set; }
        public StudentManagement.Views.GiamHieu.BaoCaoTongKetHocKy BaoCaoHocKyPage { get; set; }
        public StudentManagement.Views.HocSinh.ThongTinHocSinh ThongTinHocSinhPage { get; set; }
        public StudentManagement.Views.HocSinh.ThongTinTruong ThongTinTruongPage { get; set; }
        public DiemSo XemDiemPage { get; set; }


        //declare ICommand
        public ICommand LoadWindow { get; set; }
        public ICommand SwitchThongTinHocSinh { get; set; }
        public ICommand SwitchThongTinTruong { get; set; }
        public ICommand SwitchXemDiem { get; set; }
        public ICommand CapNhatThongTin { get; set; }
        public ICommand SwitchBaoCaoMonHoc { get; set; }
        public ICommand SwitchBaoCaoHocKy { get; set; }

        public TrangChuViewModel()
        {
            IdHocSinh = 100033;

            HocSinhHienTai = new Model.HocSinh();
            ThongTinHocSinhPage = new StudentManagement.Views.HocSinh.ThongTinHocSinh();
            ThongTinTruongPage = new StudentManagement.Views.HocSinh.ThongTinTruong();
            BaoCaoPage = new Views.GiamHieu.BaoCaoMonHoc();
            BaoCaoHocKyPage = new Views.GiamHieu.BaoCaoTongKetHocKy();

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
                SuaHocSinh window = new SuaHocSinh();
                SuaHocSinhViewModel data = window.DataContext as SuaHocSinhViewModel;
                data.HocSinhHienTai = HocSinhHienTai;
                window.ShowDialog();
                LoadThongTinCaNhan();
            });
            SwitchBaoCaoMonHoc = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = BaoCaoPage;
            });
            SwitchBaoCaoHocKy = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = BaoCaoHocKyPage;
            });
        }
        public void LoadThongTinCaNhan()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        con.Open();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return;
                    }
                    string CmdString = "select TenHocSinh,NgaySinh,GioiTinh,DiaChi,Email,AnhThe from HocSinh where MaHocSinh = " + IdHocSinh.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows) reader.Read();
                    HocSinhHienTai.MaHocSinh = IdHocSinh;
                    HocSinhHienTai.TenHocSinh = reader.GetString(0);
                    HocSinhHienTai.NgaySinh = reader.GetDateTime(1);
                    HocSinhHienTai.GioiTinh = reader.GetBoolean(2);
                    HocSinhHienTai.DiaChi = reader.GetString(3);
                    HocSinhHienTai.Email = reader.GetString(4);
                    HocSinhHienTai.Avatar = (byte[])reader[5];
                    con.Close();
                }catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
