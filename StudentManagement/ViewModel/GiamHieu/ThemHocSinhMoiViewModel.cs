using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using StudentManagement.Views.GiamHieu;
using System.Text.RegularExpressions;
using StudentManagement.Model;
using System.Data.SqlClient;
using StudentManagement.Converter;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class ThemHocSinhMoiViewModel: BaseViewModel
    {
        public ThemHocSinhMoi ThemHocSinhWD { get; set; }
        public string ImagePath { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand ChangeImage { get; set; }
        public ICommand CreateStudent { get; set; }
        public ThemHocSinhMoiViewModel()
        {
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemHocSinhWD = parameter as ThemHocSinhMoi;
                
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
            CreateStudent = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if(String.IsNullOrEmpty(ThemHocSinhWD.Hoten.Text) || String.IsNullOrEmpty(ThemHocSinhWD.NgaySinh.SelectedDate.Value.ToString()) || 
                    String.IsNullOrEmpty(ThemHocSinhWD.DiaChi.Text) || String.IsNullOrEmpty(ThemHocSinhWD.Email.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                } else if (!Regex.IsMatch(ThemHocSinhWD.Email.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    MessageBox.Show("Email không hợp lệ, vui lòng nhập lại!");
                } else
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                    {
                        con.Open();
                        string CmdString = "insert into HocSinh (TenHocSinh, NgaySinh, GioiTinh, DiaChi, Email,AnhThe) VALUES ('" + ThemHocSinhWD.Hoten.Text + "', '" + ThemHocSinhWD.NgaySinh.SelectedDate.Value.Year + '-' + ThemHocSinhWD.NgaySinh.SelectedDate.Value.Month + '-' + ThemHocSinhWD.NgaySinh.SelectedDate.Value.Day + "', ";
                        if (ThemHocSinhWD.Male.IsChecked == true)
                        {
                            CmdString += "1, ";
                        }
                        else
                        {
                            CmdString += "0, ";
                        }
                        CmdString = CmdString + "'" + ThemHocSinhWD.DiaChi.Text + "', '" + ThemHocSinhWD.Email.Text + "', ";
                        BinaryToBitmapImageConverter converter = new BinaryToBitmapImageConverter();
                        byte[] buffer = converter.ImageToBinary(ImagePath);
                        CmdString = CmdString + "@imagebinary)";
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        cmd.Parameters.AddWithValue("@imagebinary", buffer);
                        MessageBox.Show(CmdString);
                        cmd.ExecuteReader();


                        con.Close();
                    }
                }

            });
        }
    }
}
