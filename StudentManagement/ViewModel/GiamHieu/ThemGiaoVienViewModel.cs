using Microsoft.Win32;
using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
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
//using System.Drawing.Image;

namespace StudentManagement.ViewModel.GiamHieu
{
    internal class ThemGiaoVienViewModel : BaseViewModel
    {
        // khai báo biến
        //public int MatKhau;
        public string ImagePath { get; set; }
        public ThemGiaoVien ThemGiaoVienWD { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand LoadWindow { get; set; }
        public ICommand AddGiaoVien { get; set; }
        public ICommand ChangeImage { get; set; }
        public ThemGiaoVienViewModel()
        {
            ImagePath = null;
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
            LoadWindow = new RelayCommand<ThemGiaoVien>((parameter) => { return true; }, (parameter) =>
            {
                ThemGiaoVienWD = parameter;
            });

            CancelCommand = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemGiaoVienWD.Close();
            });
            AddGiaoVien = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemGiaoVienMoi();
            });

        }

        public string ToShortDateTime(DatePicker st)
        {
            string date = st.SelectedDate.Value.Year.ToString()+"-"+st.SelectedDate.Value.Month.ToString()+"-"+st.SelectedDate.Value.Day.ToString();
            return date;
        }


        bool IsValidEmail(string email)
        {
            if (!Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                return false;
            }
            return true;
        }

        public static void SendAccountByEmail(string Account, string Password, string to)
        {
            string from, subject, messageBody, header;
            header = "Chào mừng bạn đến với hệ thống Student Management! \n Thông báo tạo tài khoản giáo viên thành công \n";
            messageBody = header + "Tên đăng nhập tài khoản giáo viên của bạn là: " + Account + "\n Mật khẩu của bạn là: " + Password;
            from = "studentsp111111@gmail.com";
            subject = "Student management - Tài khoản giáo viên";
            MailMessage message = new MailMessage(from, to, subject, messageBody);
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = true;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(from, "dfmsetbdrstlnenr");
            try
            {
                client.Send(message);
                MessageBox.Show("Tạo tài khoản giáo viên thành công! Tài khoản giáo viên đã được gửi đến email " + to);
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
            }
        }

        void ThemGiaoVienMoi()
        {
            if (ThemGiaoVienWD.TenGV.Text == "" |
                ThemGiaoVienWD.NgaySinh.Text == "" |
                ThemGiaoVienWD.DiaChi.Text == "" |
                ThemGiaoVienWD.Email.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin");
            }
            else
            if (!IsValidEmail(ThemGiaoVienWD.Email.Text)
                )
            {
                MessageBox.Show("Email không đúng cú pháp!");
                ThemGiaoVienWD.Email.Focus();
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
                        } catch (Exception)
                        {
                            MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                            return; 
                        }
                        string CmdString1 = "INSERT INTO GiaoVien(TenGiaoVien,NgaySinh,GioiTinh,DiaChi,Email,MaTruong) VALUES (N'" +
                        ThemGiaoVienWD.TenGV.Text + "' , CAST(N'" +
                        ToShortDateTime(ThemGiaoVienWD.NgaySinh) + "' AS DATE) ," + ThemGiaoVienWD.GioiTinh.SelectedIndex.ToString() +
                        ", N'" + ThemGiaoVienWD.DiaChi.Text + "' , '" + ThemGiaoVienWD.Email.Text + "', 1);";
                        SqlCommand cmd1 = new SqlCommand(CmdString1, con);
                        cmd1.ExecuteScalar();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }



                    //Tao tai khoan va mat khau
                    Random rnd = new Random();
                    string MatKhau = rnd.Next(100000, 999999).ToString();
                    string TaiKhoan = "gv";
                    string emailOfNewUser = "";
                    int maSo = 0;
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
                        string CmdString = "select top 1 MaGiaoVien,Email from GiaoVien order by MaGiaoVien desc";
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        StudentManagement.Model.GiaoVien teacher = new StudentManagement.Model.GiaoVien
                        {
                            MaGiaoVien = reader.GetInt32(0),
                            Email = reader.GetString(1)
                        };
                        maSo = teacher.MaGiaoVien;
                        emailOfNewUser = teacher.Email;
                        TaiKhoan += maSo.ToString();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    

                    //Update tai khoan va mat khau, avatar
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

                        string uriImage = "";
                        if (ImagePath == null)
                        {
                            var projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                            var filePath = Path.Combine(projectPath, "Resources","Images", "user_image.jpg");
                            uriImage = filePath;
                        }
                        else uriImage = ImagePath;

                        ByteArrayToBitmapImageConverter converter = new ByteArrayToBitmapImageConverter();
                        byte[] buffer = converter.ImageToBinary(uriImage);
                        string CmdString = "Update GiaoVien set Username = '" + TaiKhoan + "', UserPassword = '" + MatKhau + "', AnhThe = @image " +
                                            " where MaGiaoVien =" + maSo.ToString();
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        cmd.Parameters.AddWithValue("@image", buffer);
                        cmd.ExecuteScalar();


                        SendAccountByEmail(TaiKhoan, MatKhau, emailOfNewUser);
                        con.Close();
                        ThemGiaoVienWD.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
