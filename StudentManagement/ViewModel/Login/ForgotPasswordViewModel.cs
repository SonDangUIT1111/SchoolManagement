using StudentManagement.Views.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.Login
{
    public class ForgotPasswordViewModel:BaseViewModel, IDataErrorInfo
    {

        // biến flag
        public bool IsSend { get; set; }
        public bool IsVerified { get; set; }
        public bool IsSignedUp { get; set; }
        public int RandomCode { get; set; }

        // biến info 
        private string _Username;
        public string Username { get => _Username; set { _Username = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        private string _ConfirmPassword;
        public string ConfirmPassword { get => _ConfirmPassword; set { _ConfirmPassword = value; OnPropertyChanged(); } }
        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }
        private string _EmailProtected;
        public string EmailProtected { get => _EmailProtected; set { _EmailProtected = value; OnPropertyChanged(); } }
        private string _NewPassword;
        public string NewPassword { get => _NewPassword; set { _NewPassword = value; OnPropertyChanged(); } }
        private string _ConfirmNewPassword;

        private string _Code;
        public string Code { get => _Code; set { _Code = value; OnPropertyChanged(); } }
        public string ConfirmNewPassword { get => _ConfirmNewPassword; set { _ConfirmNewPassword = value; OnPropertyChanged(); } }
        public string Error { get { return null; } }


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

        public string this[string columnName]
        {
            get
            {
                string ErrorMess = null;
                switch (columnName)
                {
                    case "Username":
                        if (String.IsNullOrEmpty(Username))
                            ErrorMess = "Username can not be empty";
                        if (Username.Length < 4)
                            ErrorMess = "Username lenght has to be greater or equal to 4";
                        break;
                    case "Email":
                        if (String.IsNullOrEmpty(Email))
                            ErrorMess = "Email can not be empty";
                        break;

                }
                return ErrorMess;
            }
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
                //MessageBoxOK MB = new MessageBoxOK();
                //var data = MB.DataContext as MessageBoxOKViewModel;
                //data.Content = "Please enter the email has assigned";
                //MB.ShowDialog();
                //return;
            }
            else
            {
                //check email exists ?
                //var EmailCountm = DataProvider.Ins.DB.UserAccounts.Where(x => x.UserEmail == EmailProtected).Count();
                //if exists
                //if (EmailCountm > 0)
                //{
                //    IsSend = true;
                //    Random rd = new Random();
                //    RandomCode = rd.Next(100000, 999999);
                //    string RandomCodeString = RandomCode.ToString();
                //    SendCodeByEmail(RandomCodeString, EmailProtected);
                //    return;
                //}
                //else
                //{
                    //IsSend = false;
                    //MessageBoxOK MB = new MessageBoxOK();
                    //var data = MB.DataContext as MessageBoxOKViewModel;
                    //data.Content = "This email has not been assigned";
                    //MB.ShowDialog();
                    //return;
                //}
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
                    //IsVerified = true;
                    //MessageBoxOK MB = new MessageBoxOK();
                    //var data = MB.DataContext as MessageBoxOKViewModel;
                    //data.Content = "Successfully verified, please enter your new password";
                    //MB.ShowDialog();
                    //return;
                }
                else
                {
                    //MessageBoxOK MB = new MessageBoxOK();
                    //var data = MB.DataContext as MessageBoxOKViewModel;
                    //data.Content = "The code is not right";
                    //MB.ShowDialog();
                    //IsVerified = false;
                }
            }
            catch (Exception)
            {
                //IsVerified = false;
                //MessageBoxOK MB = new MessageBoxOK();
                //var data = MB.DataContext as MessageBoxOKViewModel;
                //data.Content = "Code format is not correct";
                //MB.ShowDialog();
            }
        }
        void Change(Window parameter)
        {
            if (parameter == null)
                return;
            if (String.IsNullOrEmpty(NewPassword))
            {
                //MessageBoxOK MB = new MessageBoxOK();
                //var data = MB.DataContext as MessageBoxOKViewModel;
                //data.Content = "Please enter new password";
                //MB.ShowDialog();
                //return;
            }
            if (String.IsNullOrEmpty(ConfirmNewPassword))
            {
                //MessageBoxOK MB = new MessageBoxOK();
                //var data = MB.DataContext as MessageBoxOKViewModel;
                //data.Content = "Please confirm new password";
                //MB.ShowDialog();
                //return;
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
                //MessageBoxOK MB = new MessageBoxOK();
                //var data = MB.DataContext as MessageBoxOKViewModel;
                //data.Content = "Password must contain at least 1 Upcase and 1 number";
                //MB.ShowDialog();
                //return;
            }
            if (ConfirmNewPassword != NewPassword)
            {
                //MessageBoxOK MB = new MessageBoxOK();
                //var data = MB.DataContext as MessageBoxOKViewModel;
                //data.Content = "Confirm wrong";
                //MB.ShowDialog();
                //return;
            }
            else
            {
                //string encodenewPass = CreateMD5(Base64Encode(NewPassword));
                //var acc = DataProvider.Ins.DB.UserAccounts.Where(x => x.UserEmail == EmailProtected).SingleOrDefault();
                //acc.UserPassword = encodenewPass;
                //DataProvider.Ins.DB.SaveChanges();
                //MessageBoxSuccessful MB = new MessageBoxSuccessful();
                //MB.ShowDialog();
                //p.Close();
                //Login login = new Login();
                //login.ShowDialog();
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
            from = "spksk1111@gmail.com";
            subject = "Student management - Changing Password";
            MailMessage message = new MailMessage(from, to, subject, messageBody);
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = true;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(from, "aonfbkjdjndadyso");
            try
            {
                client.Send(message);
                //MessageBoxOK MB = new MessageBoxOK();
                //var data = MB.DataContext as MessageBoxOKViewModel;
                //data.Content = "The code verified has been sent to your email protect";
                //MB.ShowDialog();
            }
            catch (Exception)
            {
                //MessageBoxFail MB = new MessageBoxFail();
                //MB.ShowDialog();
            }
        }

    }
}
