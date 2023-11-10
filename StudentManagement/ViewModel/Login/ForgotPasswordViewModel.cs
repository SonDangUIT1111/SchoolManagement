using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.Login;
using StudentManagement.Views.MessageBox;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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

        private readonly ISqlConnectionWrapper sqlConnection;

        public ForgotPasswordViewModel(ISqlConnectionWrapper sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
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
                if (parameter == null)
                    return;
                //check fully information
                if (CheckValidEmail(EmailProtected))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng điền email bảo vệ tài khoản hợp lệ";
                    MB.ShowDialog();
                    return;
                }
                if (IndexRole<0 || IndexRole > 2)
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng chọn chức vụ";
                    MB.ShowDialog();
                    return;
                }
                int checkUser = GetThongTin();
                
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
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Email chưa được đăng ký";
                    MB.ShowDialog();
                    return;
                }
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
                if (parameter == null)
                    return;
                var window = parameter as ForgotPasswordWindow;
                if (!CheckValidCode(window.CodeVerified.Text))
                {
                    IsVerified = false;
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Định dạng mã xác thực không hợp lệ, vui lòng nhập 6 chữ số để xác thực";
                    MB.ShowDialog();
                }
                if (Int32.Parse(window.CodeVerified.Text) == RandomCode)
                {
                    IsVerified = true;
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Xác thực thành công, vui lòng nhập mật khẩu mới.";
                    MB.ShowDialog();
                    return;
                }
                else
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Mã xác thực không chính xác, vui lòng kiểm tra lại.";
                    MB.ShowDialog();
                    IsVerified = false;
                    return;
                }
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
                if (parameter == null)
                    return;
                if (String.IsNullOrEmpty(NewPassword))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng nhập mật khẩu mới";
                    MB.ShowDialog();
                    return;
                }
                if (String.IsNullOrEmpty(ConfirmNewPassword))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng xác nhận mật khẩu mới";
                    MB.ShowDialog();
                    return;
                }
                //check validation of password
                if (!CheckValidPassword(NewPassword))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Mật khẩu phải có ít nhất một chữ số và một kí tự in hoa.";
                    MB.ShowDialog();
                    return;
                }
                if (ConfirmNewPassword != NewPassword)
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Mật khẩu xác nhận không chính xác";
                    MB.ShowDialog();
                    return;
                }
                else
                {
                    DoiMatKhauMoi();
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Đổi mật khẩu thành công";
                    MB.ShowDialog();
                    parameter.Close();
                    LoginWindow login = new LoginWindow();
                    login.ShowDialog();
                }
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

        public void DoiMatKhauMoi()
        {
            string CmdString = string.Empty;
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string passEncode = CreateMD5(Base64Encode(NewPassword));
                    CmdString = "Update "+ (IndexRole==0?"GiamHieu":IndexRole==1?"GiaoVien":"HocSinh") +" Set UserPassword = '" + passEncode + "' Where Email ='" + EmailProtected + "'";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    cmd.ExecuteScalar();
                    sqlConnectionWrap.Close();
                }
                catch (Exception)
                {
                }
            }
        }
        public int GetThongTin()
        {
            string CmdString = string.Empty;
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    CmdString = "Select count(*) from " + (IndexRole == 0 ? "GiamHieu" : IndexRole == 1 ? "GiaoVien" : "HocSinh") + " where Email = '" + IndexRole + "'";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    int check = Convert.ToInt32(cmd.ExecuteScalar());
                    sqlConnection.Close();
                    return check;
                }
                catch (Exception)
                {
                    return -1;
                }

            }
        }

        public bool CheckValidPassword(string pass)
        {
            if (String.IsNullOrEmpty(pass)) return false;
            bool flagUpcase = false, flagNum = false;
            foreach (char c in pass)
            {
                if (c >= 'A' && c < 'Z' + 1)
                    flagUpcase = true;
                if (c >= '0' && c < '9' + 1)
                    flagNum = true; 
            }
            return flagNum && flagUpcase;
        }

        public bool CheckValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") && !String.IsNullOrEmpty(email);
        }


        public bool CheckValidCode(string code)
        {
            if (code.Length != 6) return false;
            try
            {
                Int32.Parse(code);
                return code.Length == 6;
            }
            catch
            {
                return false;
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
        public void SendCodeByEmail(string codesend, string to)
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
                //MessageBoxOK MB = new MessageBoxOK();
                //var data = MB.DataContext as MessageBoxOKViewModel;
                //data.Content = "Mã xác thực đã được gửi đến email bảo vệ của bạn";
                //MB.ShowDialog();
            }
            catch (Exception)
            {
                //MessageBoxFail messageBoxFail = new MessageBoxFail();   
                //messageBoxFail.ShowDialog();
            }
        }

    }
}
