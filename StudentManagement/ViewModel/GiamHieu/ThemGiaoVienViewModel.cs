using Microsoft.Win32;
using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Data.SqlClient;
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

        public string ToShortDateTime(string st)
        {
            string Converted_String = "";
            for (int i = 0; i < st.Length; i++)
            {
                if (st[i] == '/')
                    Converted_String += '-';
                else
                    Converted_String += st[i];
            }
            return Converted_String;
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
                MessageBox.Show("Tài khoản giáo viên đã được gửi đến email " + to);
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
            }
        }

        void ThemGiaoVienMoi()
        {
            //MessageBox.Show(Avatar);
            // MessageBox.Show("testin end");
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
                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return;
                    }
                    string CmdString = "INSERT INTO GiaoVien(TenGiaoVien,NgaySinh,GioiTinh,DiaChi,Email,MaTruong) VALUES (\'" +
                        ThemGiaoVienWD.TenGV.Text + "\' , \'" +
                        ToShortDateTime(ThemGiaoVienWD.NgaySinh.DisplayDate.ToString()) + "\' ," + ThemGiaoVienWD.GioiTinh.SelectedIndex.ToString() +
                        ", N\'" + ThemGiaoVienWD.DiaChi.Text + "\' , \'" + ThemGiaoVienWD.Email.Text + "\', 1);";
                    //MessageBox.Show(CmdString);
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    cmd.ExecuteScalar();
                    con.Close();


                    //Tao tai khoan va mat khau
                    Random rnd = new Random();
                    string MatKhau = rnd.Next(100000, 999999).ToString();
                    string TaiKhoan = "gv";

                    try
                    {
                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                    }
                    CmdString = "select top 1 * from GiaoVien order by MaGiaoVien desc";
                    cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    StudentManagement.Model.GiaoVien teacher = new StudentManagement.Model.GiaoVien
                    {
                        MaGiaoVien = reader.GetInt32(0),
                        TenGiaoVien = reader.GetString(1),
                        NgaySinh = reader.GetDateTime(2),
                        GioiTinh = reader.GetBoolean(3),
                        DiaChi = reader.GetString(4),
                        Email = reader.GetString(5)
                    };
                    int MaSo = teacher.MaGiaoVien;
                    TaiKhoan += MaSo.ToString();
                    //MessageBox.Show("TK: "+TaiKhoan+" MK: "+MatKhau);
                    con.Close();

                    //Update tai khoan va mat khau
                    try
                    {
                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                    }
                    CmdString = "Update GiaoVien set Username = \'" + TaiKhoan + "\', UserPassword = \'" + MatKhau + "\' where MaGiaoVien =" + MaSo.ToString();
                    cmd = new SqlCommand(CmdString, con);
                    cmd.ExecuteScalar();
                    SendAccountByEmail(TaiKhoan, MatKhau, teacher.Email);
                    con.Close();

                    //Update anh dai dien
                    ByteArrayToBitmapImageConverter converter = new ByteArrayToBitmapImageConverter();
                    byte[] buffer = converter.ImageToBinary(ImagePath);
                    try
                    {
                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                    }
                    string cmdstring = "update GiaoVien set AnhThe = @image where MaGiaoVien = " + MaSo.ToString();
                    cmd = new SqlCommand(cmdstring, con);
                    cmd.Parameters.AddWithValue("@image", buffer);
                    cmd.ExecuteScalar();
                    con.Close();

                    MessageBox.Show("Thêm giáo viên thành công! (" + MaSo.ToString() + ")");
                }
            }
        }
    }
}
