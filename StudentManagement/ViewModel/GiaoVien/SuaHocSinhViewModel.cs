using Microsoft.Win32;
using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StudentManagement.ViewModel.GiaoVien
{
    public class SuaHocSinhViewModel : BaseViewModel
    {

        const string ngaysinh = "', NgaySinh = '";
        const string emailstring = "', Email = '";
        public SuaHocSinh SuaHocSinhWD { get; set; }
        public string ImagePath { get; set; }

        private StudentManagement.Model.HocSinh _hocsinhhientai;
        public StudentManagement.Model.HocSinh HocSinhHienTai { get => _hocsinhhientai; set { _hocsinhhientai = value;  } }
        public ICommand CancelCommand { get; set; }
        public ICommand LoadWindow { get; set; }
        public ICommand ChangeImage { get; set; }
        public ICommand ChangeHocSinh { get; set; }

        public bool IsValidEmail(string email)
        {
            if (!Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                return false;
            }
            return true;
        }
        public int CapNhatHocSinh()
        {
            int count = 0;
                using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
                {
                        sqlConnectionWrapper.Open();

                        List<int> quiDinh = new List<int>();
                        string cmdTest = "select GiaTri from QuiDinh";
                        using (var cmd1 = new SqlCommand(cmdTest, sqlConnectionWrapper.GetSqlConnection()))
                        using (var readerTest = cmd1.ExecuteReader())
                        {
                            while (readerTest.HasRows)
                            {
                                while (readerTest.Read())
                                {
                                    quiDinh.Add(readerTest.GetInt32(0));
                                }
                                readerTest.NextResult();
                            }
                        }


 

                        if (DateTime.Now.Year - SuaHocSinhWD.NgaySinh.SelectedDate.Value.Year > quiDinh[2] || DateTime.Now.Year - SuaHocSinhWD.NgaySinh.SelectedDate.Value.Year < quiDinh[1])
                        {
                            return -1;
                        }

                        string CmdString = "Update HocSinh set TenHocSinh = N'" + SuaHocSinhWD.TenHS.Text +
                            ngaysinh + ToShortDateTime(SuaHocSinhWD.NgaySinh) +
                            "', GioiTinh = " + SuaHocSinhWD.GioiTinh.SelectedIndex.ToString() +
                            ", DiaChi = N'" + SuaHocSinhWD.DiaChi.Text +
                            emailstring + SuaHocSinhWD.Email.Text +
                            "' ,AnhThe = @imagebinary" +
                            " where MaHocSinh = " + HocSinhHienTai.MaHocSinh;


                        if (ImagePath != null)
                        {
                            ByteArrayToBitmapImageConverter converter = new ByteArrayToBitmapImageConverter();
                            byte[] buffer = converter.ImageToBinary(ImagePath);

                            using (var cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection()))
                            {
                                cmd.Parameters.AddWithValue("@imagebinary", buffer);
                                count = cmd.ExecuteNonQuery();
                            }

                            // Display a success message here or return true
                        }
                return count;
                        //SuaHocSinhWD.Close();

                }
            
        }


        public string ToShortDateTime(DatePicker st)
        {
            if (st.SelectedDate.HasValue)
            {
                string date = st.SelectedDate.Value.Year.ToString() + "-" + st.SelectedDate.Value.Month.ToString() + "-" + st.SelectedDate.Value.Day.ToString();
                return date;
            }else
            {
                return string.Empty;

            }
        }

        public SuaHocSinhViewModel()
        {
            // Stryker disable all
            HocSinhHienTai = new StudentManagement.Model.HocSinh();
            LoadWindow = new RelayCommand<SuaHocSinh>((parameter) => { return true; }, (parameter) =>
            {
                SuaHocSinhWD = parameter;
            });
            CancelCommand = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                SuaHocSinhWD.Close();
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
            ChangeHocSinh = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {

                if (SuaHocSinhWD.TenHS.Text == "" ||
                SuaHocSinhWD.NgaySinh.Text == "" ||
                SuaHocSinhWD.DiaChi.Text == "" ||
                SuaHocSinhWD.Email.Text == "")
                {
                    // Display an error message here or return false
                    return;
                }
                else if (!IsValidEmail(SuaHocSinhWD.Email.Text))
                {
                    // Display an error message here or return false
                    return;
                }
                else
                {
                    CapNhatHocSinh();
                }
            });
        }
    }
}
