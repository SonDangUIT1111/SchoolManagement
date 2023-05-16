﻿using Microsoft.Win32;
using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class ThemHocSinhMoiViewModel : BaseViewModel
    {
        public ThemHocSinhMoi ThemHocSinhWD { get; set; }
        public string ImagePath { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand ChangeImage { get; set; }
        public ICommand CreateStudent { get; set; }
        public ICommand CancelAdd { get; set; }
        public ObservableCollection<string> ListCommand = new ObservableCollection<string>();
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
                if (String.IsNullOrEmpty(ThemHocSinhWD.Hoten.Text) || String.IsNullOrEmpty(ThemHocSinhWD.NgaySinh.SelectedDate.Value.ToString()) ||
                    String.IsNullOrEmpty(ThemHocSinhWD.DiaChi.Text) || String.IsNullOrEmpty(ThemHocSinhWD.Email.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                }
                else if (!Regex.IsMatch(ThemHocSinhWD.Email.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    MessageBox.Show("Email không hợp lệ, vui lòng nhập lại!");
                }
                else
                {
                    MessageBoxResult ConfirmAdd = System.Windows.MessageBox.Show("Bạn có muốn thêm học sinh này không?", "Add Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (ConfirmAdd == MessageBoxResult.Yes)
                    {
                        ListCommand.Clear();
                        using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                        {
                            try
                            {
                                try 
                                { 
                                    con.Open(); 
                                } catch (Exception) 
                                {
                                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                                    return; 
                                }
                                string CmdString = @"insert into HocSinh (TenHocSinh, NgaySinh, GioiTinh, DiaChi, Email,AnhThe) VALUES (N'" 
                                + ThemHocSinhWD.Hoten.Text + "', CAST(N'" +ToShortDateTime(ThemHocSinhWD.NgaySinh) + "' AS DATE), ";
                                if (ThemHocSinhWD.Male.IsChecked == true)
                                {
                                    CmdString += "1, ";
                                }
                                else
                                {
                                    CmdString += "0, ";
                                }
                                CmdString = CmdString + "N'" + ThemHocSinhWD.DiaChi.Text + "', '" + ThemHocSinhWD.Email.Text + "', @imagebinary) ";

                                string uriImage = "";
                                if (ImagePath == null)
                                {
                                    var projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                                    var filePath = Path.Combine(projectPath, "Resources", "Images", "user_image.jpg");
                                    uriImage = filePath;
                                }
                                else uriImage = ImagePath;

                                ByteArrayToBitmapImageConverter converter = new ByteArrayToBitmapImageConverter();
                                byte[] buffer = converter.ImageToBinary(uriImage);
                                // Định nghĩa sqlcommand
                                SqlCommand cmd = new SqlCommand(CmdString, con);
                                SqlParameter sqlParam = cmd.Parameters.AddWithValue("@imagebinary", buffer);
                                sqlParam.DbType = DbType.Binary;
                                cmd.ExecuteNonQuery();

                                // Tạo tài khoản cho học sinh vừa thêm
                                Random rnd = new Random();
                                string MatKhau = rnd.Next(100000, 999999).ToString();
                                string TaiKhoan = "hs";
                                CmdString = "select top 1 MaHocSinh,Email from HocSinh order by MaHocSinh desc";
                                cmd = new SqlCommand(CmdString, con);
                                SqlDataReader reader = cmd.ExecuteReader();
                                reader.Read();
                                StudentManagement.Model.HocSinh hocsinh = new StudentManagement.Model.HocSinh
                                {
                                    MaHocSinh = reader.GetInt32(0),
                                    Email = reader.GetString(1)
                                };
                                reader.Close();
                                TaiKhoan += hocsinh.MaHocSinh.ToString();

                                // Tạo bảng điểm cho học sinh
                                CmdString = "select MaMon from MonHoc where ApDung = 1";
                                cmd = new SqlCommand(CmdString, con);
                                SqlDataReader reader1 = cmd.ExecuteReader();
                                while (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        string CmdString1 = "insert into HeThongDiem (HocKy,MaMon,MaHocSinh,Diem15Phut,Diem1Tiet,DiemTrungBinh,XepLoai) values (1,"+ reader1.GetInt32(0).ToString()
                                                    +", "+hocsinh.MaHocSinh.ToString() + ",NULL,NULL,NULL,NULL)  ";
                                        CmdString1 = CmdString1 + "insert into HeThongDiem (HocKy,MaMon,MaHocSinh,Diem15Phut,Diem1Tiet,DiemTrungBinh,XepLoai) values (2," + reader1.GetInt32(0).ToString()
                                                                + ", " + hocsinh.MaHocSinh.ToString() + ",NULL,NULL,NULL,NULL) ";
                                        ListCommand.Add(CmdString1);
                                    }
                                    reader1.NextResult();
                                }
                                reader1.Close();

                                for (int i = 0;i<ListCommand.Count;i++)
                                {
                                    CmdString = ListCommand[i].ToString();
                                    cmd = new SqlCommand(CmdString, con);
                                    cmd.ExecuteNonQuery();
                                }

                                CmdString = "insert into ThanhTich (HocKy,MaHocSinh,TrungBinhHocKy,XepLoai) values (1,"+hocsinh.MaHocSinh.ToString()
                                            +",NULL,NULL) insert into ThanhTich (HocKy,MaHocSinh,TrungBinhHocKy,XepLoai) values (2, "+hocsinh.MaHocSinh.ToString()+",NULL,NULL) ";
                                cmd = new SqlCommand(CmdString, con);
                                cmd.ExecuteNonQuery();

                                // cập nhật lại tài khoản
                                CmdString = "Update HocSinh set Username = '" + TaiKhoan + "', UserPassword = '" + MatKhau 
                                            + "' where MaHocSinh =" + hocsinh.MaHocSinh.ToString();
                                cmd = new SqlCommand(CmdString, con);
                                cmd.ExecuteNonQuery();


                                con.Close();
                                SendAccountByEmail(TaiKhoan, MatKhau, hocsinh.Email);
                                ThemHocSinhWD.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                return;
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
        public static void SendAccountByEmail(string Account, string Password, string to)
        {
            string from, subject, messageBody, header;
            header = "Chào mừng bạn đến với hệ thống Student Management! \n Thông báo tạo tài khoản học sinh thành công \n";
            messageBody = header + "Tên đăng nhập tài khoản học sinh của bạn là: " + Account + "\n Mật khẩu của bạn là: " + Password;
            from = "studentsp111111@gmail.com";
            subject = "Student management - Tài khoản học sinh";
            MailMessage message = new MailMessage(from, to, subject, messageBody);
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = true;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(from, "dfmsetbdrstlnenr");
            try
            {
                client.Send(message);
                MessageBox.Show("Tạo tài khoản học sinh thành công! Tài khoản học sinh đã được gửi đến email " + to);
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
            }
        }
    }
}