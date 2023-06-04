using StudentManagement.Model;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.ViewModel.Login;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.Login;
using StudentManagement.Views.MessageBox;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StudentManagement.ViewModel.GiaoVien
{
    public class TrangChuViewModel : BaseViewModel
    {
        //declare variable
        private string _sayHello;
        public string SayHello { get { return _sayHello; } set { _sayHello = value;OnPropertyChanged(); } }
        private Model.GiaoVien _currentUser;
        public Model.GiaoVien CurrentUser { get { return _currentUser; } set { _currentUser = value;OnPropertyChanged(); } }
        public GiaoVienWindow GiaoVienWD { get; set; }
        //declare Pages
        public StudentManagement.Views.GiamHieu.BaoCaoMonHoc BaoCaoPage { get; set; }
        public StudentManagement.Views.GiamHieu.BaoCaoTongKetHocKy BaoCaoHocKyPage { get; set; }
        public StudentManagement.Views.GiaoVien.LopHoc LopHocPage { get; set; }
        public StudentManagement.Views.GiaoVien.ThanhTichHocSinh ThanhTichHocSinhPage { get; set; }
        public StudentManagement.Views.GiaoVien.HeThongBangDiem HeThongBangDiemPage { get; set; }
        public StudentManagement.Views.GiamHieu.ThongTinTruong ThongTinTruongPage { get; set; }
        public StudentManagement.Views.GiaoVien.SuaThongTinCaNhan ThongTinCaNhanPage { get; set; }

        //declare ICommand
        public ICommand LoadWindow { get; set; }
        public ICommand SwitchThongTinTruong { get; set; }
        public ICommand SwitchLopHoc { get; set; }
        public ICommand SwitchThanhTichHocSinh { get; set; }
        public ICommand SwitchQuanLiBangDiem { get; set; }
        public ICommand SwitchBaoCaoMonHoc { get; set; }
        public ICommand SwitchBaoCaoHocKy { get; set; }
        public ICommand DoiMatKhau { get; set; }
        public ICommand SuaThongTinCaNhan { get; set; }
        public TrangChuViewModel()
        {
            ThongTinTruongPage = new Views.GiamHieu.ThongTinTruong();
            LopHocPage = new Views.GiaoVien.LopHoc();
            ThanhTichHocSinhPage = new ThanhTichHocSinh();
            HeThongBangDiemPage = new HeThongBangDiem();
            BaoCaoPage = new Views.GiamHieu.BaoCaoMonHoc();
            BaoCaoHocKyPage = new Views.GiamHieu.BaoCaoTongKetHocKy();
            CurrentUser = new StudentManagement.Model.GiaoVien();
            //define ICommand
            LoadWindow = new RelayCommand<GiaoVienWindow>((parameter) => { return true; }, (parameter) =>
            {
                GiaoVienWD = parameter;
                LoadThongTinCaNhan();
                LoadSayHello(GiaoVienWD.imageAvatar);
                StudentManagement.ViewModel.GiaoVien.LopHocViewModel vm = LopHocPage.DataContext as StudentManagement.ViewModel.GiaoVien.LopHocViewModel;
                vm.IdGiaoVien = CurrentUser.MaGiaoVien;
                StudentManagement.ViewModel.GiaoVien.ThanhTichHocSinhViewModel vmThanhTich = ThanhTichHocSinhPage.DataContext as StudentManagement.ViewModel.GiaoVien.ThanhTichHocSinhViewModel;
                vmThanhTich.IdUser = CurrentUser.MaGiaoVien;
                StudentManagement.ViewModel.GiaoVien.HeThongBangDiemViewModel vmHeThongDiem = HeThongBangDiemPage.DataContext as StudentManagement.ViewModel.GiaoVien.HeThongBangDiemViewModel;
                vmHeThongDiem.IdUser = CurrentUser.MaGiaoVien;
                GiaoVienWD.RPage.Content = ThongTinTruongPage;
            });

            SwitchThongTinTruong = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = ThongTinTruongPage;
            });
            SwitchLopHoc = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = LopHocPage;
            });
            SwitchThanhTichHocSinh = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = ThanhTichHocSinhPage;

            });
            SwitchQuanLiBangDiem = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = HeThongBangDiemPage;
            });
            SwitchBaoCaoMonHoc = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = BaoCaoPage;
            });
            SwitchBaoCaoHocKy = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = BaoCaoHocKyPage;
            });

            DoiMatKhau = new RelayCommand<string>((parameter) => { return true; }, (parameter) =>
            {
                string password;
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString = "select UserPassword from GiaoVien where MaGiaoVien = " + CurrentUser.MaGiaoVien.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows) reader.Read();
                    password = reader.GetString(0);
                    con.Close();
                }
                ChangePasswordWindow window = new ChangePasswordWindow();
                ChangePasswordViewModel data = window.DataContext as ChangePasswordViewModel;
                data.Id = CurrentUser.MaGiaoVien.ToString();
                data.MatKhau = password;
                data.IsHS = false;
                //MessageBox.Show(parameter);
                window.ShowDialog();
            });

            SuaThongTinCaNhan = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {

                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try
                    {
                        SuaGiaoVien window = new SuaGiaoVien();
                        SuaGiaoVienViewModel data = window.DataContext as SuaGiaoVienViewModel;
                        data.GiaoVienHienTai = CurrentUser;
                        window.ShowDialog();
                        LoadThongTinCaNhan();
                    }
                    catch (Exception)
                    {
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
                    }
                }
            });
            DoiMatKhau = new RelayCommand<string>((parameter) => { return true; }, (parameter) =>
            {
                string password;
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString = "select UserPassword from GiaoVien where MaGiaoVien = " + CurrentUser.MaGiaoVien.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows) reader.Read();
                    password = reader.GetString(0);
                    con.Close();
                }
                ChangePasswordWindow window = new ChangePasswordWindow();
                ChangePasswordViewModel data = window.DataContext as ChangePasswordViewModel;
                data.Id = CurrentUser.MaGiaoVien.ToString();
                data.MatKhau = password;
                data.IsHS = false;
                //MessageBox.Show(parameter);
                window.ShowDialog();
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
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
                        return;
                    }
                    string CmdString = "select TenGiaoVien,NgaySinh,GioiTinh,DiaChi,Email,AnhThe from GiaoVien where MaGiaoVien = " + CurrentUser.MaGiaoVien.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows) reader.Read();
                    CurrentUser.TenGiaoVien = reader.GetString(0);
                    CurrentUser.NgaySinh = reader.GetDateTime(1);
                    CurrentUser.GioiTinh = reader.GetBoolean(2);
                    CurrentUser.DiaChi = reader.GetString(3);
                    CurrentUser.Email = reader.GetString(4);
                    CurrentUser.Avatar = (byte[])reader[5];
                    con.Close();
                } catch (Exception)
                {
                    MessageBoxFail msgBoxFail = new MessageBoxFail();   
                    msgBoxFail.ShowDialog();
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
                GiaoVienWD.UserName.Text = CurrentUser.TenGiaoVien;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                MemoryStream stream = new MemoryStream(CurrentUser.Avatar);
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                imageBrush.ImageSource = bitmap;
                imageBrush.Stretch = Stretch.UniformToFill;
                item.Background = imageBrush;
            }
            catch (Exception)
            {
                MessageBoxFail messageBoxFail = new MessageBoxFail();
                messageBoxFail.ShowDialog();    
            }
        }
    }
}
