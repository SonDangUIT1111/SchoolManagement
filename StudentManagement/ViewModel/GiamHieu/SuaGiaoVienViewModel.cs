using Microsoft.Win32;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using StudentManagement.Model;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

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
            //GiaoVienHienTai.TenGiaoVien = "skajdl";

            LoadWindow = new RelayCommand<SuaGiaoVien>((parameter) => { return true; }, (parameter) =>
            {
                SuaGiaoVienWD = parameter;
                //MessageBox.Show(GiaoVienHienTai.GioiTinh);
                //MessageBox.Show(GiaoVienHienTai.TenGiaoVien);
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
            //MessageBox.Show(Avatar);
            // MessageBox.Show("testin end");
            if (SuaGiaoVienWD.TenGV.Text == "" |
                SuaGiaoVienWD.NgaySinh.Text == "" |
                SuaGiaoVienWD.DiaChi.Text == "" |
                SuaGiaoVienWD.Email.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin");
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
                        con.Open();

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi không thể truy cập cơ sở dữ liệu");
                        return;
                    }
                    string CmdString = "Update GiaoVien set TenGiaoVien = \'" + SuaGiaoVienWD.TenGV.Text +
                        "\', NgaySinh = \'" + SuaGiaoVienWD.NgaySinh.DisplayDate.ToString() +
                        "\', GioiTinh = " + SuaGiaoVienWD.GioiTinh.SelectedIndex.ToString() +
                        ", DiaChi = N\'" + SuaGiaoVienWD.DiaChi.Text +
                        "\', Email = \'" + SuaGiaoVienWD.Email.Text +
                        "\' where MaGiaoVien = " + GiaoVienHienTai.MaGiaoVien.ToString();
                    //MessageBox.Show(CmdString);
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    cmd.ExecuteScalar();
                    con.Close();
                    MessageBox.Show("Cập nhật thành công!");
                }
            }
        }
    }
}
