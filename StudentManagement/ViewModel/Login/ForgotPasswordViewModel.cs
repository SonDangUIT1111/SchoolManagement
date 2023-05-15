using StudentManagement.Model;
using StudentManagement.Views.Login;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.Login
{
    public class ForgotPasswordViewModel : BaseViewModel
    {

        // biến flag
        public bool IsSend { get; set; }
        public bool IsVerified { get; set; }
        public bool IsSignedUp { get; set; }
        public int RandomCode { get; set; }

        // biến info 
        private int _indexRole;
        public int IndexRole { get { return _indexRole; } set { _indexRole = value; } }

        private string _emailProtected;
        public string EmailProtected { get => _emailProtected; set { _emailProtected = value; OnPropertyChanged(); } }
        private string _newPassword;
        public string NewPassword { get => _newPassword; set { _newPassword = value; OnPropertyChanged(); } }
        private string _confirmNewPassword;

        private string _code;
        public string Code { get => _code; set { _code = value; OnPropertyChanged(); } }
        public string ConfirmNewPassword { get => _confirmNewPassword; set { _confirmNewPassword = value; OnPropertyChanged(); } }


        // khai báo ICommand 
        public ICommand ToLogin_ForgotPassword { get; set; }
        public ICommand SendCodeCommand { get; set; }
        public ICommand NewPasswordChangedCommand { get; set; }
        public ICommand NewPassEyeChangedCommand { get; set; }
        public ICommand ConfirmNewPasswordChangedCommand { get; set; }
        public ICommand ConfirmEyeChangedCommand { get; set; }
        public ICommand VerifiedCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand CheckCode { get; set; }
        public ICommand ShowNewPassword { get; set; }
        public ICommand UnshowNewPassword { get; set; }
        public ICommand ShowConfirmPassword { get; set; }
        public ICommand UnshowConfirmPassword { get; set; }
        public ICommand ShowPassword_Register { get; set; }
        public ICommand UnshowPassword_Register { get; set; }
        public ICommand ShowConfirmPassword_Register { get; set; }
        public ICommand UnshowConfirmPassword_Register { get; set; }

        public ForgotPasswordViewModel()
        {
            //Declare random code
            RandomCode = 0;

            //Flags to check
            IsSend = false;
            IsVerified = false;
            IsSignedUp = false;

            // define
            NewPassEyeChangedCommand = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) => { NewPassword = parameter.Text; });
            ConfirmEyeChangedCommand = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) => { ConfirmNewPassword = parameter.Text; });
            NewPasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { NewPassword = parameter.Password; });
            ConfirmNewPasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { ConfirmNewPassword = parameter.Password; });

            SendCodeCommand = new RelayCommand<Window>((parameter) => { return true; }, (parameter) =>
            {
                Send(parameter);
            });

            ToLogin_ForgotPassword = new RelayCommand<ForgotPasswordWindow>((parameter) => { return true; }, (parameter) =>
            {
                if (parameter == null)
                    return;
                EmailProtected = "";
                parameter.CodeVerified.Text = "";
                parameter.Close();
                LoginWindow login = new LoginWindow();
                login.ShowDialog();

            });

            //

            CheckCode = new RelayCommand<Window>((parameter) =>
            {
                var window = parameter as ForgotPasswordWindow;
                var code = window.CodeVerified.Text;
                if (!String.IsNullOrEmpty(code))
                {
                    if (code.Length == 6)
                        return true;
                    else
                    {
                        window.VerifiedButton.IsEnabled = false;
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            , (parameter) =>
            {
                var window = parameter as ForgotPasswordWindow;
                window.VerifiedButton.IsEnabled = true;
            });
            VerifiedCommand = new RelayCommand<Window>((parameter) =>
            {
                if (IsSend == true)

                    return true;
                else return false;
            },
            (parameter) =>
            {
                Verified(parameter);
            });
            ChangePasswordCommand = new RelayCommand<Window>((parameter) =>
            {
                if (IsVerified == true)
                {
                    return true;
                }
                else return false;
            }, (parameter) =>
            {
                Change(parameter);
            });
            ShowNewPassword = new RelayCommand<ForgotPasswordWindow>((parameter) => { return true; }, (parameter) =>
            {
                parameter.ShowNewPass.Visibility = Visibility.Hidden;
                parameter.UnshowNewPass.Visibility = Visibility.Visible;
                parameter.NewPassEye.Text = parameter.NewPassword.Password;
                parameter.NewPassEye.Visibility = Visibility.Visible;
                parameter.NewPassword.Visibility = Visibility.Hidden;
            });
            UnshowNewPassword = new RelayCommand<ForgotPasswordWindow>((parameter) => { return true; }, (parameter) =>
            {
                parameter.ShowNewPass.Visibility = Visibility.Visible;
                parameter.UnshowNewPass.Visibility = Visibility.Hidden;
                parameter.NewPassword.Visibility = Visibility.Visible;
                parameter.NewPassword.Password = parameter.NewPassEye.Text;
                parameter.NewPassEye.Visibility = Visibility.Hidden;
            });
            ShowConfirmPassword = new RelayCommand<ForgotPasswordWindow>((parameter) => { return true; }, (parameter) =>
            {
                parameter.ShowConfirmPass.Visibility = Visibility.Hidden;
                parameter.UnshowConfirmPass.Visibility = Visibility.Visible;
                parameter.ConfirmPassEye.Text = parameter.ConfirmNewPassword.Password;
                parameter.ConfirmPassEye.Visibility = Visibility.Visible;
                parameter.ConfirmNewPassword.Visibility = Visibility.Hidden;
            });
            UnshowConfirmPassword = new RelayCommand<ForgotPasswordWindow>((parameter) => { return true; }, (parameter) =>
            {
                parameter.ShowConfirmPass.Visibility = Visibility.Visible;
                parameter.UnshowConfirmPass.Visibility = Visibility.Hidden;
                parameter.ConfirmNewPassword.Visibility = Visibility.Visible;
                parameter.ConfirmNewPassword.Password = parameter.ConfirmPassEye.Text;
                parameter.ConfirmPassEye.Visibility = Visibility.Hidden;
            });

        }

        void Send(Window parameter)
        {
            if (parameter == null)
                return;
            //check fully information
            if (String.IsNullOrEmpty(EmailProtected))
            {
                MessageBox.Show("Vui lòng điền email bảo vệ tài khoản.");
                return;
            }
            else
            {
                if (IndexRole == -1)
                {
                    MessageBox.Show("Vui lòng chọn chức vụ");
                    return;
                }
                else if (IndexRole == 0)
                {
                    int checkUser = 0;
                    string CmdString = string.Empty;
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
                            CmdString = "Select count(*) from GiamHieu where Email = '" + EmailProtected + "'";
                            SqlCommand cmd = new SqlCommand(CmdString, con);
                            checkUser = Convert.ToInt32(cmd.ExecuteScalar());
                            con.Close();
                        }
                        catch (Exception)
                        {
                            
                        }

                    }
                    if (checkUser > 0)
                    {
                        IsSend = true;
                        Random rd = new Random();
                        RandomCode = rd.Next(100000, 999999);
                        string RandomCodeString = RandomCode.ToString();
                        SendCodeByEmail(RandomCodeString, EmailProtected);
                        return;
                    }
                    else
                    {
                        IsSend = false;
                        MessageBox.Show("Email chưa được đăng ký.");
                        return;
                    }
                }
                else if (IndexRole == 1)
                {
                    int checkUser = 0;
                    string CmdString = string.Empty;
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
                            CmdString = "Select count(*) from GiaoVien where Email = '" + EmailProtected + "'";
                            SqlCommand cmd = new SqlCommand(CmdString, con);
                            checkUser = Convert.ToInt32(cmd.ExecuteScalar());
                            con.Close();
                        }
                        catch (Exception)
                        {
                            
                        }
                    }
                    if (checkUser > 0)
                    {
                        IsSend = true;
                        Random rd = new Random();
                        RandomCode = rd.Next(100000, 999999);
                        string RandomCodeString = RandomCode.ToString();
                        SendCodeByEmail(RandomCodeString, EmailProtected);
                        return;
                    }
                    else
                    {
                        IsSend = false;
                        MessageBox.Show("Email chưa được đăng ký.");
                        return;
                    }
                }
                else if (IndexRole == 2)
                {
                    int checkUser = 0;
                    string CmdString = string.Empty;
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
                            CmdString = "Select count(*) from HocSinh where Email = '" + EmailProtected + "'";
                            SqlCommand cmd = new SqlCommand(CmdString, con);
                            checkUser = Convert.ToInt32(cmd.ExecuteScalar());
                            con.Close();
                        }
                        catch (Exception)
                        {

                        }
                    }
                    if (checkUser > 0)
                    {
                        IsSend = true;
                        Random rd = new Random();
                        RandomCode = rd.Next(100000, 999999);
                        string RandomCodeString = RandomCode.ToString();
                        SendCodeByEmail(RandomCodeString, EmailProtected);
                        return;
                    }
                    else
                    {
                        IsSend = false;
                        MessageBox.Show("Email chưa được đăng ký.");
                        return;
                    }
                }
            }
        }
        void Verified(Window parameter)
        {
            if (parameter == null)
                return;
            var window = parameter as ForgotPasswordWindow;
            try
            {
                if (Int32.Parse(window.CodeVerified.Text) == RandomCode)
                {
                    IsVerified = true;
                    MessageBox.Show("Xác thực thành công, vui lòng nhập mật khẩu mới");
                    return;
                }
                else
                {
                    MessageBox.Show("Mã xác thực không chính xác, vui lòng nhập lại");
                    IsVerified = false;
                    return;
                }
            }
            catch (Exception)
            {
                IsVerified = false;
                MessageBox.Show("Định dạng mã xác thực không hợp lệ, vui lòng nhập 6 chữ số để xác thực");
            }
        }
        void Change(Window parameter)
        {
            if (parameter == null)
                return;
            if (String.IsNullOrEmpty(NewPassword))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới");
                return;
            }
            if (String.IsNullOrEmpty(ConfirmNewPassword))
            {
                MessageBox.Show("Vui lòng xác nhận lại mật khẩu mới");
                return;
            }
            //check validation of password
            int countUpcase = 0, countNum = 0;
            foreach (char c in NewPassword)
            {
                if (c >= 'A' && c <= 'Z')
                    countUpcase++;
                if (c >= '0' && c <= '9')
                    countNum++;
            }
            if (countNum == 0 || countUpcase == 0)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất một kí tự in hoa và một kí tự số");
                return;
            }
            if (ConfirmNewPassword != NewPassword)
            {
                MessageBox.Show("Mật khẩu xác nhận không chính xác");
                return;
            }
            else
            {
                string CmdString = string.Empty;
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
                        if (IndexRole == 0)
                        {
                            CmdString = "Update GiamHieu Set UserPassword = '" + NewPassword + "' Where Email ='" + EmailProtected + "'";
                        }
                        else if (IndexRole == 1)
                        {
                            CmdString = "Update GiaoVien Set UserPassword = '" + NewPassword + "' Where Email ='" + EmailProtected + "'";
                        }
                        else if (IndexRole == 2)
                        {
                            CmdString = "Update HocSinh Set UserPassword = '" + NewPassword + "' Where Email ='" + EmailProtected + "'";
                        }
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        cmd.ExecuteScalar();
                        con.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
                MessageBox.Show("Đổi mật khẩu thành công");
                parameter.Close();
                LoginWindow login = new LoginWindow();
                login.ShowDialog();
            }
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string CreateMD5(string input)
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
        public static void SendCodeByEmail(string codesend, string to)
        {
            string from, subject, messageBody;
            messageBody = "Your verified code is " + codesend;
            from = "studentsp111111@gmail.com";
            subject = "Student management - Changing Password";
            MailMessage message = new MailMessage(from, to, subject, messageBody);
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = true;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(from, "dfmsetbdrstlnenr");
            try
            {
                client.Send(message);
                MessageBox.Show("Mã xác thực đã được gửi đến email bảo vệ của bạn");
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
            }
        }

    }
}
