using Microsoft.Win32;
using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
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
    internal class SuaGiaoVienViewModel : BaseViewModel
    {
        public SuaGiaoVien SuaGiaoVienWD { get; set; }
        public string ImagePath { get; set; }

        private StudentManagement.Model.GiaoVien _giaovienhientai;
        public StudentManagement.Model.GiaoVien GiaoVienHienTai { get => _giaovienhientai; set { _giaovienhientai = value; OnPropertyChanged(); } }
        public ICommand CancelCommand { get; set; }
        public ICommand LoadWindow { get; set; }
        public ICommand ChangeImage { get; set; }
        public ICommand ChangeGiaoVien { get; set; }

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
                        MessageBox.Show("Lỗi, không cập nhật được hình ảnh.");
                    }
                }
            });
            ChangeGiaoVien = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                CapNhatGiaoVien();
            });
        }
        bool IsValidEmail(string email)
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
                MessageBox.Show("Vui lòng điền đầy đủ thông tin");
                return;
            }
            else
            if (!IsValidEmail(SuaGiaoVienWD.Email.Text)
                )
            {
                MessageBox.Show("Email không đúng cú pháp!");
                SuaGiaoVienWD.Email.Focus();
            }
            else
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
                        string CmdString = "Update GiaoVien set TenGiaoVien = N'" + SuaGiaoVienWD.TenGV.Text +
                            "', NgaySinh = CAST(N'" + ToShortDateTime(SuaGiaoVienWD.NgaySinh) +
                            "' AS DATE), GioiTinh = " + SuaGiaoVienWD.GioiTinh.SelectedIndex.ToString() +
                            ", DiaChi = N'" + SuaGiaoVienWD.DiaChi.Text +
                            "', Email = '" + SuaGiaoVienWD.Email.Text +
                            "', AnhThe = @image where MaGiaoVien = " + GiaoVienHienTai.MaGiaoVien.ToString();

                        if (ImagePath != null)
                        {
                            ByteArrayToBitmapImageConverter converter = new ByteArrayToBitmapImageConverter();
                            byte[] buffer = converter.ImageToBinary(ImagePath);
                            SqlCommand cmd = new SqlCommand(CmdString, con);
                            cmd.Parameters.AddWithValue("@image", buffer);
                            cmd.ExecuteScalar();
                            con.Close();
                            MessageBox.Show("Cập nhật thành công.");
                            SuaGiaoVienWD.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        public string ToShortDateTime(DatePicker st)
        {
            string date = st.SelectedDate.Value.Year.ToString() + "-" + st.SelectedDate.Value.Month.ToString() + "-" + st.SelectedDate.Value.Day.ToString();
            return date;
        }
    }
}
