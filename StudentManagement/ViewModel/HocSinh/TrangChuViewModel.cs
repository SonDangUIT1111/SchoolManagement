using StudentManagement.Model;
using StudentManagement.ViewModel.GiaoVien;
using StudentManagement.ViewModel.Login;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.HocSinh;
using StudentManagement.Views.Login;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StudentManagement.ViewModel.HocSinh
{
    internal class TrangChuViewModel : BaseViewModel
    {
        //declare variable
        private string _sayHello;
        public string SayHello { get { return _sayHello; } set { _sayHello = value;OnPropertyChanged(); } }
        public HocSinhWindow HocSinhWD { get; set; }
        private int _idHocSinh;
        public int IdHocSinh { get { return _idHocSinh; } set { _idHocSinh = value; } }

        private string _idHocSinhstring;
        public string IdHocSinhstring { get { return _idHocSinhstring; } set { _idHocSinhstring = value; } }


        private Model.HocSinh _hocSinhHienTai;
        public Model.HocSinh HocSinhHienTai { get { return _hocSinhHienTai;} set { _hocSinhHienTai = value;OnPropertyChanged(); } }
        //declare Pages
        public StudentManagement.Views.GiamHieu.BaoCaoMonHoc BaoCaoPage { get; set; }
        public StudentManagement.Views.GiamHieu.BaoCaoTongKetHocKy BaoCaoHocKyPage { get; set; }
        public StudentManagement.Views.HocSinh.ThongTinHocSinh ThongTinHocSinhPage { get; set; }
        public StudentManagement.Views.GiamHieu.ThongTinTruong ThongTinTruongPage { get; set; }
        public DiemSo XemDiemPage { get; set; }


        //declare ICommand
        public ICommand LoadWindow { get; set; }
        public ICommand SwitchThongTinTruong { get; set; }
        public ICommand SwitchXemDiem { get; set; }
        public ICommand CapNhatThongTin { get; set; }
        public ICommand DoiMatKhau { get; set; }
        public ICommand SwitchBaoCaoMonHoc { get; set; }
        public ICommand SwitchBaoCaoHocKy { get; set; }

        public TrangChuViewModel()
        {
            IdHocSinh = 100000;
            IdHocSinhstring = IdHocSinh.ToString();

            HocSinhHienTai = new Model.HocSinh();
            ThongTinTruongPage = new StudentManagement.Views.GiamHieu.ThongTinTruong();
            BaoCaoPage = new Views.GiamHieu.BaoCaoMonHoc();
            BaoCaoHocKyPage = new Views.GiamHieu.BaoCaoTongKetHocKy();
            XemDiemPage = new DiemSo();

            //define ICommand
            LoadWindow = new RelayCommand<HocSinhWindow>((parameter) => { return true; }, (parameter) =>
            {
                HocSinhWD = parameter;
                LoadThongTinCaNhan();
                LoadSayHello(HocSinhWD.imageAvatar);
                StudentManagement.ViewModel.HocSinh.DiemSoViewModel vm = XemDiemPage.DataContext as StudentManagement.ViewModel.HocSinh.DiemSoViewModel;
                vm.IdHocSinh = IdHocSinh;
                HocSinhWD.RPage.Content = ThongTinTruongPage;
                
            });
            SwitchThongTinTruong = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = ThongTinTruongPage;
            });
            SwitchXemDiem = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
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
            DoiMatKhau = new RelayCommand<string>((parameter) => { return true; }, (parameter) =>
            {
                string password;
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString = "select UserPassword from HocSinh where MaHocSinh = " + IdHocSinh.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows) reader.Read();
                    password = reader.GetString(0);
                    con.Close();
                 }
                ChangePasswordWindow window = new ChangePasswordWindow();
                ChangePasswordViewModel data = window.DataContext as ChangePasswordViewModel;
                data.Id = parameter.ToString();
                data.MatKhau = password;
                data.IsHS = true;
                //MessageBox.Show(parameter);
                window.ShowDialog();
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
        public void LoadSayHello(Border item)
        {
            int hour = DateTime.Now.Hour;
            if (hour >= 0 && hour < 6)
                SayHello = "Have a nice day";
            else if (hour >= 6 && hour < 12)
                SayHello = "Good morning";
            else if (hour >= 12 && hour < 18)
                SayHello = "Good afternoon";
            else if (hour >= 18 && hour < 24)
                SayHello = "Good evening";
            try
            {
                HocSinhWD.UserName.Text = HocSinhHienTai.TenHocSinh;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                MemoryStream stream = new MemoryStream(HocSinhHienTai.Avatar);
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                imageBrush.ImageSource = bitmap;
                imageBrush.Stretch = Stretch.UniformToFill;
                item.Background = imageBrush;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi, không cập nhật được hình ảnh.");
            }
        }
    }
}
