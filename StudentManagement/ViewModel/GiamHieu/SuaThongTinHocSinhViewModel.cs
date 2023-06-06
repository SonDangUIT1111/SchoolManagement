using Microsoft.Win32;
using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
using System;
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
        public SuaThongTinHocSinh SuaThongTinHocSinhWD { get; set; }
        public string ImagePath { get; set; }
        private StudentManagement.Model.HocSinh _hocSinhHienTai;
        public StudentManagement.Model.HocSinh HocSinhHienTai { get => _hocSinhHienTai; set { _hocSinhHienTai = value; OnPropertyChanged(); } }
        public ICommand LoadData { get; set; }
        public ICommand ConfirmChange { get; set; }
        public ICommand ChangeImage { get; set; }
        public ICommand CancelChange { get; set; }
        public SuaThongTinHocSinhViewModel()
        {
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
                        using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                        {
                            try
                            {
                                try 
                                { 
                                    con.Open(); 
                                } catch (Exception) 
                                { 
                                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                                    messageBoxFail.ShowDialog();
                                    return; 
                                }
                                string CmdString = @"update HocSinh set TenHocSinh = N'" + SuaThongTinHocSinhWD.HoTen.Text + "', NgaySinh = CAST(N'" 
                                + ToShortDateTime(SuaThongTinHocSinhWD.NgaySinh) + "' AS DATE), GioiTinh = ";
                                if (SuaThongTinHocSinhWD.Male.IsChecked == true)
                                {
                                    CmdString += "1, ";
                                }
                                else
                                {
                                    CmdString += "0, ";
                                }
                                CmdString = CmdString + "DiaChi = N'" + SuaThongTinHocSinhWD.DiaChi.Text + "', Email = '" + SuaThongTinHocSinhWD.Email.Text;
                                // Định nghĩa @imagebinary
                                if (ImagePath != null)
                                {
                                    CmdString = CmdString + "', AnhThe = @imagebinary where MaHocSinh = " + HocSinhHienTai.MaHocSinh;
                                    ByteArrayToBitmapImageConverter converter = new ByteArrayToBitmapImageConverter();
                                    byte[] buffer = converter.ImageToBinary(ImagePath);
                                    SqlCommand cmd = new SqlCommand(CmdString, con);
                                    cmd.Parameters.AddWithValue("@imagebinary", buffer);
                                    cmd.ExecuteScalar();
                                    MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                                    messageBoxSuccessful.ShowDialog();
                                    con.Close();
                                } else
                                {
                                    CmdString = CmdString + "' where MaHocSinh = " + HocSinhHienTai.MaHocSinh;
                                    SqlCommand cmd = new SqlCommand(CmdString, con);
                                    cmd.ExecuteScalar();
                                    MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                                    messageBoxSuccessful.ShowDialog();
                                    con.Close();
                                }
                                ImagePath = null;
                                SuaThongTinHocSinhWD.Close();
                            }
                            catch (Exception)
                            {
                                MessageBoxFail messageBoxFail = new MessageBoxFail();
                                messageBoxFail.ShowDialog();
                            }
                        }
                    }

                }

            });
        }
        public string ToShortDateTime(DatePicker st)
        {
            string date = st.SelectedDate.Value.Year.ToString() + "-" + st.SelectedDate.Value.Month.ToString() + "-" + st.SelectedDate.Value.Day.ToString();
            return date;
        }
    }
}
