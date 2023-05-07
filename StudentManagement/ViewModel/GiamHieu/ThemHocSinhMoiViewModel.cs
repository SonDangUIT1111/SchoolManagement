﻿using Microsoft.Win32;
using System;
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
using System.Data;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class ThemHocSinhMoiViewModel: BaseViewModel
    {
        public ThemHocSinhMoi ThemHocSinhWD { get; set; }
        public string ImagePath { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand ChangeImage { get; set; }
        public ICommand CreateStudent { get; set; }
        public ICommand CancelAdd { get; set; }
        public ThemHocSinhMoiViewModel()
        {
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemHocSinhWD = parameter as ThemHocSinhMoi;
                // set default day
                int defaultYear = DateTime.Now.Year - 15;
                DateTime defaultTime = new DateTime(defaultYear, 1, 1);
                ThemHocSinhWD.NgaySinh.SelectedDate = defaultTime;
                
            });
            CancelAdd = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemHocSinhWD.Close();

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
                    MessageBoxResult ConfirmAdd = System.Windows.MessageBox.Show("Bạn có muốn thêm học sinh này không?", "Add Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (ConfirmAdd == MessageBoxResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                        {
                            con.Open();
                            string CmdString = @"insert into HocSinh (TenHocSinh, NgaySinh, GioiTinh, DiaChi, Email,AnhThe) VALUES (N'" + ThemHocSinhWD.Hoten.Text + "', '" + ThemHocSinhWD.NgaySinh.SelectedDate.Value.Year + '-' + ThemHocSinhWD.NgaySinh.SelectedDate.Value.Month + '-' + ThemHocSinhWD.NgaySinh.SelectedDate.Value.Day + "', ";
                            if (ThemHocSinhWD.Male.IsChecked == true)
                            {
                                CmdString += "1, ";
                            }
                            else
                            {
                                CmdString += "0, ";
                            }
                            CmdString = CmdString + "N'" + ThemHocSinhWD.DiaChi.Text + "', '" + ThemHocSinhWD.Email.Text + "', ";

                            // tạo đệm lưu ảnh avatar
                            ByteArrayToBitmapImageConverter converter = new ByteArrayToBitmapImageConverter();
                            byte[] buffer = converter.ImageToBinary(ImagePath);


                            CmdString += "@imagebinary)";
                            // Định nghĩa @imagebinary
                            SqlCommand cmd = new SqlCommand(CmdString, con);
                            SqlParameter sqlParam = cmd.Parameters.AddWithValue("@imagebinary", buffer);
                            sqlParam.DbType = DbType.Binary;
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        MessageBox.Show("Thêm học sinh thành công!");
                        ThemHocSinhWD.Close();
                    }
                    
                }

            });
        }
    }
}
