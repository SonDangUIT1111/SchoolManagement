using Microsoft.Win32;
using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class SuaGiaoVienViewModel : BaseViewModel
    {
        public SuaGiaoVien SuaGiaoVienWD { get; set; }
        public string ImagePath { get; set; }

        private StudentManagement.Model.GiaoVien _giaovienhientai;
        public StudentManagement.Model.GiaoVien GiaoVienHienTai { get => _giaovienhientai; set { _giaovienhientai = value; OnPropertyChanged(); } }
        public ICommand CancelCommand { get; set; }
        public ICommand LoadWindow { get; set; }
        public ICommand ChangeImage { get; set; }
        public ICommand ChangeGiaoVien { get; set; }
        private readonly ISqlConnectionWrapper sqlConnection;

        public SuaGiaoVienViewModel(ISqlConnectionWrapper sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public SuaGiaoVienViewModel()
        {

            GiaoVienHienTai = new StudentManagement.Model.GiaoVien { };

            LoadWindow = new RelayCommand<SuaGiaoVien>((parameter) => { return true; }, (parameter) =>
            {
                SuaGiaoVienWD = parameter;
            });

            CancelCommand = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                SuaGiaoVienWD.Close();
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
            ChangeGiaoVien = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                CapNhatGiaoVien();
            });
        }
        public bool IsValidEmail(string email)
        {
            if (!Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                return false;
            }
            return true;
        }
        public void CapNhatGiaoVien()
        {
            if (SuaGiaoVienWD.TenGV.Text == "" |
                SuaGiaoVienWD.NgaySinh.Text == "" |
                SuaGiaoVienWD.DiaChi.Text == "" |
                SuaGiaoVienWD.Email.Text == "")
            {
                //MessageBoxOK MB = new MessageBoxOK();
                //var data = MB.DataContext as MessageBoxOKViewModel;
                //data.Content = "Vui lòng điền đầy đủ thông tin";
                //MB.ShowDialog();
                return;
            }
            else
            if (!IsValidEmail(SuaGiaoVienWD.Email.Text)
                )
            {
                //MessageBoxOK MB = new MessageBoxOK();
                //var data = MB.DataContext as MessageBoxOKViewModel;
                //data.Content = "Email không đúng cú pháp!";
                //MB.ShowDialog();
                //SuaGiaoVienWD.Email.Focus();
                return;
            }
            else
            {
                using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
                {
                    try
                    {
                        try
                        {
                            sqlConnectionWrapper.Open();

                        }
                        catch (Exception)
                        {
                            //MessageBoxFail messageBoxFail = new MessageBoxFail();
                            //messageBoxFail.ShowDialog();
                            return;
                        }
                        string CmdString = "Update GiaoVien set TenGiaoVien = N'" + SuaGiaoVienWD.TenGV.Text +
                            "', NgaySinh = CAST(N'" + ToShortDateTime(SuaGiaoVienWD.NgaySinh) +
                            "' AS DATE), GioiTinh = " + SuaGiaoVienWD.GioiTinh.SelectedIndex.ToString() +
                            ", DiaChi = N'" + SuaGiaoVienWD.DiaChi.Text +
                            "', Email = '" + SuaGiaoVienWD.Email.Text +
                            "'";

                        if (ImagePath != null)
                        {
                            CmdString = CmdString + " , AnhThe = @image where MaGiaoVien = " + GiaoVienHienTai.MaGiaoVien.ToString();
                            StudentManagement.Converter.ByteArrayToBitmapImageConverter converter = new StudentManagement.Converter.ByteArrayToBitmapImageConverter();
                            byte[] buffer = converter.ImageToBinary(ImagePath);
                            SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                            cmd.Parameters.AddWithValue("@image", buffer);
                            cmd.ExecuteScalar();
                            sqlConnectionWrapper.Close();
               
                            //MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                            //messageBoxSuccessful.ShowDialog();
                        }
                        else
                        {
                            CmdString = CmdString + " where MaGiaoVien = " + GiaoVienHienTai.MaGiaoVien.ToString();
                            SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                            cmd.ExecuteScalar();
                            //MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                            //messageBoxSuccessful.ShowDialog();
                            sqlConnectionWrapper.Close();
                        }
                        ImagePath = null;
                        SuaGiaoVienWD.Close();
                    }
                    catch (Exception)
                    {
                        //MessageBoxFail messageBoxFail = new MessageBoxFail();
                        //messageBoxFail.ShowDialog();
                    }
                }
            }
        }
        public string ToShortDateTime(DatePicker st)
        {
            if (st.SelectedDate.HasValue)
            {
                string date = st.SelectedDate.Value.Year.ToString() + "-" + st.SelectedDate.Value.Month.ToString() + "-" + st.SelectedDate.Value.Day.ToString();
                return date;
            }
            else
            {
                return string.Empty;

            }
        }
    }
}
