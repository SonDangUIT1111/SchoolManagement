using Microsoft.Win32;
using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class SuaThongTinHocSinhViewModel : BaseViewModel
    {
        const string femalestring = "0, ";
        const string malestring = "1, ";
        public SuaThongTinHocSinh SuaThongTinHocSinhWD { get; set; }
        public string ImagePath { get; set; }
        private StudentManagement.Model.HocSinh _hocSinhHienTai;
        public StudentManagement.Model.HocSinh HocSinhHienTai { get => _hocSinhHienTai; set { _hocSinhHienTai = value;  } }

        public ICommand LoadData { get; set; }
        public ICommand ConfirmChange { get; set; }
        public ICommand ChangeImage { get; set; }
        public ICommand CancelChange { get; set; }

        public int[] TienHanhSuaThongTinHocSinh(DatePicker ngaysinh, string hoten,bool? isMale,string diachi,string email)
        {
            int[] result = new int[2];
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {

                    sqlConnectionWrap.Open();
                    List<int> quiDinh = new List<int>();
                    string cmdTest = "select GiaTri from QuiDinh";
                    SqlCommand cmd1 = new SqlCommand(cmdTest, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader readerTest = cmd1.ExecuteReader();
                    while (readerTest.HasRows)
                    {
                        while (readerTest.Read())
                        {
                            quiDinh.Add(readerTest.GetInt32(0));
                        }
                        readerTest.NextResult();
                    }
                    readerTest.Close();
                    if (DateTime.Now.Year - ngaysinh.SelectedDate.Value.Year > quiDinh[2] || DateTime.Now.Year - ngaysinh.SelectedDate.Value.Year < quiDinh[1])
                    {
                        //MessageBoxOK messageBoxOK = new MessageBoxOK();
                        //MessageBoxOKViewModel datamb = messageBoxOK.DataContext as MessageBoxOKViewModel;
                        //datamb.Content = "Tuổi của học sinh phải từ " + quiDinh[1].ToString() + " đến " + quiDinh[2].ToString();
                        //messageBoxOK.ShowDialog();
                        // vi pham quy dinh tuoi
                        return result;
                    }



                    string CmdString = @"update HocSinh set TenHocSinh = N'" + hoten + "', NgaySinh = CAST(N'"
                    + ToShortDateTime(ngaysinh) + "' AS DATE), GioiTinh = ";

                CmdString += isMale==true ? malestring: femalestring;
                    CmdString = CmdString + "DiaChi = N'" + diachi + "', Email = '" + email;
                    // Định nghĩa @imagebinary
                    if (ImagePath != null)
                    {
                        CmdString = CmdString + "', AnhThe = @imagebinary where MaHocSinh = " + HocSinhHienTai.MaHocSinh;
                        ByteArrayToBitmapImageConverter converter = new ByteArrayToBitmapImageConverter();
                        byte[] buffer = converter.ImageToBinary(ImagePath);
                        SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                        cmd.Parameters.AddWithValue("@imagebinary", buffer);
                        result[0] = cmd.ExecuteNonQuery();

                        //MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                        //messageBoxSuccessful.ShowDialog();
                    }
                    else
                    {
                        CmdString = CmdString + "' where MaHocSinh = " + HocSinhHienTai.MaHocSinh;
                        SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                        result[1] = cmd.ExecuteNonQuery();
                        //MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                        //messageBoxSuccessful.ShowDialog();
                    }
                    ImagePath = null;
                    return result;
            }
        }
        public string ToShortDateTime(DatePicker st)
        {
            string date = st.SelectedDate.Value.Year.ToString() + "-" + st.SelectedDate.Value.Month.ToString() + "-" + st.SelectedDate.Value.Day.ToString();
            return date;
        }

        public SuaThongTinHocSinhViewModel()
        {
            // Stryker disable all
            HocSinhHienTai = new StudentManagement.Model.HocSinh();
            LoadData = new RelayCommand<SuaThongTinHocSinh>((parameter) => { return true; }, (parameter) =>
            {
                SuaThongTinHocSinhWD = parameter;
                if (HocSinhHienTai.GioiTinh == true)
                {
                    SuaThongTinHocSinhWD.Male.IsChecked = true;
                }
                else
                {
                    SuaThongTinHocSinhWD.Female.IsChecked = true;
                }
            });
            CancelChange = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                SuaThongTinHocSinhWD.Close();

            });
            ChangeImage = new RelayCommand<Grid>((parameter) => { return true; }, (parameter) =>
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Insert Image";
                op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (.jpg;.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (.png)|.png";
                if (op.ShowDialog() == true)
                {
                    ImagePath = op.FileName;
                    try
                    {
                        ImageBrush imageBrush = new ImageBrush();
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(ImagePath);
                        bitmap.EndInit();
                        imageBrush.ImageSource = bitmap;
                        parameter.Background = imageBrush;
                        if (parameter.Children.Count > 1)
                        {
                            parameter.Children.Remove(parameter.Children[0]);
                            parameter.Children.Remove(parameter.Children[1]);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
                    }
                }
            });
            ConfirmChange = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (String.IsNullOrEmpty(SuaThongTinHocSinhWD.HoTen.Text) || String.IsNullOrEmpty(SuaThongTinHocSinhWD.NgaySinh.SelectedDate.Value.ToString()) ||
                    String.IsNullOrEmpty(SuaThongTinHocSinhWD.DiaChi.Text) || String.IsNullOrEmpty(SuaThongTinHocSinhWD.Email.Text))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng nhập đầy đủ thông tin";
                    MB.ShowDialog();
                }
                else if (!Regex.IsMatch(SuaThongTinHocSinhWD.Email.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Email không hợp lệ, vui lòng nhập lại!";
                    MB.ShowDialog();
                }
                else
                {
                    MessageBoxYesNo wd = new MessageBoxYesNo();

                    var data = wd.DataContext as MessageBoxYesNoViewModel;
                    data.Title = "Xác nhận!";
                    data.Question = "Bạn có muốn sửa thông tin học sinh này không?";
                    wd.ShowDialog();

                    var result = wd.DataContext as MessageBoxYesNoViewModel;
                    if (result.IsYes == true)
                    {
                        TienHanhSuaThongTinHocSinh(SuaThongTinHocSinhWD.NgaySinh, SuaThongTinHocSinhWD.HoTen.Text, SuaThongTinHocSinhWD.Male.IsChecked,
                            SuaThongTinHocSinhWD.DiaChi.Text, SuaThongTinHocSinhWD.Email.Text);
                    }

                }

            });
        }
    }
}
