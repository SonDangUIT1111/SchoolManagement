using Microsoft.Win32;
using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Drawing.Image;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class ThemGiaoVienViewModel : BaseViewModel
    {
        // khai báo biến
        //public int MatKhau;
        public string ImagePath { get; set; }
        public ThemGiaoVien ThemGiaoVienWD { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand LoadWindow { get; set; }
        public ICommand AddGiaoVien { get; set; }
        public ICommand ChangeImage { get; set; }

        private readonly ISqlConnectionWrapper sqlConnection;

        public ThemGiaoVienViewModel(ISqlConnectionWrapper sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
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
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
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
                int result = ThemGiaoVienMoi(ThemGiaoVienWD.TenGV.Text,ThemGiaoVienWD.NgaySinh,ThemGiaoVienWD.DiaChi.Text,ThemGiaoVienWD.Email.Text,
                    ThemGiaoVienWD.GioiTinh.SelectedIndex.ToString());
                if (result == -2)
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng điền đầy đủ thông tin";
                    MB.ShowDialog();
                }
                else if (result == -1)
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Email không đúng cú pháp!";
                    MB.ShowDialog();
                    ThemGiaoVienWD.Email.Focus();
                }
                else if (result == 1)
                {
                    ThemGiaoVienWD.Close();
                }
                else
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                }
            });

        }

        public string ToShortDateTime(DatePicker st)
        {
            string date = st.SelectedDate.Value.Year.ToString()+"-"+st.SelectedDate.Value.Month.ToString()+"-"+st.SelectedDate.Value.Day.ToString();
            return date;
        }


        public bool IsValidEmail(string email)
        {
            if (!Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                return false;
            }
            return true;
        }

        public void SendAccountByEmail(string Account, string Password, string to)
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
                //MessageBoxOK MB = new MessageBoxOK();
                //var data = MB.DataContext as MessageBoxOKViewModel;
                //data.Content = "Tạo tài khoản giáo viên thành công! Tài khoản giáo viên đã được gửi đến email " + to;
                //MB.ShowDialog();
            }
            catch (Exception)
            {
                //MessageBoxFail messageBoxFail = new MessageBoxFail();
                //messageBoxFail.ShowDialog();
            }
        }

        public int ThemGiaoVienMoi(string tengv, DatePicker ngaysinh, string diachi,string email,string gioitinh)
        {
            if (String.IsNullOrEmpty(tengv) |
                String.IsNullOrEmpty(ngaysinh.Text) |
                String.IsNullOrEmpty(diachi) |
                String.IsNullOrEmpty(email))
            {
                return -2;
            }
            else
            if (!IsValidEmail(email)
                )
            {
                return -1;
            }
            else
            {
                using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
                {
                    try
                    {
                        sqlConnectionWrap.Open();
                        string CmdString1 = "INSERT INTO GiaoVien(TenGiaoVien,NgaySinh,GioiTinh,DiaChi,Email,MaTruong) VALUES (N'" +
                        tengv + "' , CAST(N'" +
                        ToShortDateTime(ngaysinh) + "' AS DATE) ," + gioitinh +
                        ", N'" + diachi + "' , '" + email + "', 1);";
                        SqlCommand cmd1 = new SqlCommand(CmdString1, sqlConnectionWrap.GetSqlConnection());
                        cmd1.ExecuteScalar();
                        sqlConnectionWrap.Close();
                        //Tao tai khoan va mat khau
                        Random rnd = new Random();
                        string MatKhau = rnd.Next(100000, 999999).ToString();
                        string TaiKhoan = "gv";
                        string emailOfNewUser = "";
                        int maSo = 0;
                        sqlConnectionWrap.Open();
                        string CmdString = "select top 1 MaGiaoVien,Email from GiaoVien order by MaGiaoVien desc";
                        SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
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
                        sqlConnectionWrap.Close();
                        //Update tai khoan va mat khau, avatar
                        sqlConnectionWrap.Open();

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
                        string passEncode = CreateMD5(Base64Encode(MatKhau));
                        string CmdString2 = "Update GiaoVien set Username = '" + TaiKhoan + "', UserPassword = '" + passEncode + "', AnhThe = @image " +
                                            " where MaGiaoVien =" + maSo.ToString();
                        SqlCommand cmd2 = new SqlCommand(CmdString2, sqlConnectionWrap.GetSqlConnection());
                        cmd2.Parameters.AddWithValue("@image", buffer);
                        cmd2.ExecuteScalar();
                        SendAccountByEmail(TaiKhoan, MatKhau, emailOfNewUser);
                        sqlConnectionWrap.Close();
                        ImagePath = null;
                        return 1;
                    }
                    catch (Exception)
                    {
                        return -3;
                    }
                }
            }
        }

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                //return Convert.ToHexString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to .NET 5
                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
