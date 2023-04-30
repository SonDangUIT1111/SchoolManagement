using Microsoft.Win32;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.GiaoVien;
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
using StudentManagement.Converter;
using StudentManagement.Model;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace StudentManagement.ViewModel.GiaoVien
{
    internal class SuaHocSinhViewModel : BaseViewModel
    {
        public SuaHocSinh SuaHocSinhWD { get; set; }
        public string ImagePath { get; set; }

        private StudentManagement.Model.HocSinh _hocsinhhientai;
        public StudentManagement.Model.HocSinh HocSinhHienTai { get => _hocsinhhientai; set { _hocsinhhientai = value; OnPropertyChanged(); } }
        public ICommand CancelCommand { get; set; }
        public ICommand LoadWindow { get; set; }
        public ICommand ChangeImage { get; set; }
        public ICommand ChangeHocSinh { get; set; }
        public SuaHocSinhViewModel() 
        {
            HocSinhHienTai = new StudentManagement.Model.HocSinh { };
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
                        MessageBox.Show("Lỗi, không cập nhật được hình ảnh.");
                    }
                }
            });
            ChangeHocSinh = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                CapNhatHocSinh();
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
        public void CapNhatHocSinh()
        {
            //MessageBox.Show(Avatar);
            // MessageBox.Show("testin end");
            if (SuaHocSinhWD.TenHS.Text == "" |
                SuaHocSinhWD.NgaySinh.Text == "" |
                SuaHocSinhWD.DiaChi.Text == "" |
                SuaHocSinhWD.Email.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin");
            }
            else
            if (!IsValidEmail(SuaHocSinhWD.Email.Text)
                )
            {
                MessageBox.Show("Email không đúng cú pháp!");
                SuaHocSinhWD.Email.Focus();
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
                    string CmdString = "Update HocSinh set TenHocSinh = \'" + SuaHocSinhWD.TenHS.Text +
                        "\', NgaySinh = \'" + SuaHocSinhWD.NgaySinh.DisplayDate.ToString() +
                        "\', GioiTinh = " + SuaHocSinhWD.GioiTinh.SelectedIndex.ToString() +
                        ", DiaChi = N\'" + SuaHocSinhWD.DiaChi.Text +
                        "\', Email = \'" + SuaHocSinhWD.Email.Text +
                        "\' where MaHocSinh = " + HocSinhHienTai.MaHocSinh.ToString();
                    //MessageBox.Show(CmdString);
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    cmd.ExecuteScalar();
                    con.Close();
                    if (ImagePath != null)
                    {
                        //MessageBox.Show(ImagePath);
                        ByteArrayToBitmapImageConverter converter = new ByteArrayToBitmapImageConverter();
                        byte[] buffer = converter.ImageToBinary(ImagePath);
                        con.Open();
                        string cmdstring = "update HocSinh set AnhThe = @image where MaHocSinh = " + HocSinhHienTai.MaHocSinh.ToString();
                        cmd = new SqlCommand(cmdstring, con);
                        cmd.Parameters.AddWithValue("@image", buffer);
                        cmd.ExecuteScalar();
                        con.Close();
                    }
                    MessageBox.Show("Cập nhật thành công!");
                }
            }
        }
    }
}
