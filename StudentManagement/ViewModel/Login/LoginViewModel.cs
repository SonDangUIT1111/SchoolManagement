using StudentManagement.Views.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.Login
{
    public class LoginViewModel:BaseViewModel, IDataErrorInfo
    {

        // khai báo biến
        public bool IsLoggedIn { get; set; }
        private string _username;
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        // khai báo usercontrol
        public LoginWindow LoginWindow { get; set; }

        // khai báo command
        public ICommand GoToRegisterCommand { get; set; }
        public ICommand GoToForgotPasswordCommand { get; set; }
        public ICommand LoginSuccess { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand PasswordEyeChangedCommand { get; set; }
        public ICommand ShowPassword { get; set; }
        public ICommand UnshowPassword { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand TurnBackRoleForm { get; set; }
        public ICommand TurnToLoginForm { get; set; }

        // in lỗi để username trống
        public string Error { get { return null; } }

        public string this[string columnName]
        {
            get
            {
                if (LoginWindow != null)
                {
                    string ErrorMess = null;
                    if (LoginWindow.Account.IsFocused == true)
                    {
                        switch (columnName)
                        {
                            case "Username":
                                if (String.IsNullOrEmpty(Username))
                                    ErrorMess = "Username can not be empty";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        ErrorMess = "";
                    }
                    return ErrorMess;
                }
                else return null;
                
            }
        }


        public LoginViewModel()
        {
            IsLoggedIn = false;
            Username = "";
            Password = "";

            // binding text 
            PasswordChangedCommand = new RelayCommand<PasswordBox>((paramater) => { return true; }, (paramater) => { Password = paramater.Password; });
            PasswordEyeChangedCommand = new RelayCommand<TextBox>((paramater) => { return true; }, (paramater) => { Password = paramater.Text; });

            LoadData = new RelayCommand<LoginWindow>((parameter) => { return true; }, (parameter) =>
            {
                LoginWindow = parameter;
            });
          
            // navigate
            LoginSuccess = new RelayCommand<Window>((paramater) => { return true; }, (paramater) =>
            {
                Log(paramater);
            });
            GoToForgotPasswordCommand = new RelayCommand<LoginWindow>((paramater) => { return true; }, (paramater) =>
            {
                Username = "";
                paramater.Close();
                ForgotPasswordWindow forgotPassword = new ForgotPasswordWindow();
                forgotPassword.ShowDialog();

            });

            // show password
            ShowPassword = new RelayCommand<LoginWindow>((paramater) => { return true; }, (paramater) =>
            {
                paramater.ShowPass.Visibility = Visibility.Hidden;
                paramater.UnshowPass.Visibility = Visibility.Visible;
                paramater.PasswordEye.Text = paramater.Password.Password;
                paramater.PasswordEye.Visibility = Visibility.Visible;
                paramater.Password.Visibility = Visibility.Hidden;
            });
            UnshowPassword = new RelayCommand<LoginWindow>((paramater) => { return true; }, (paramater) =>
            {
                paramater.ShowPass.Visibility = Visibility.Visible;
                paramater.UnshowPass.Visibility = Visibility.Hidden;
                paramater.Password.Visibility = Visibility.Visible;
                paramater.Password.Password = paramater.PasswordEye.Text;
                paramater.PasswordEye.Visibility = Visibility.Hidden;
            });

            // switch role and login
            TurnBackRoleForm = new RelayCommand<object>((paramater) => { return true; }, (parameter) =>
            {
                LoginWindow.GiamHieuRole.IsChecked = false;
                LoginWindow.GiaoVienRole.IsChecked = false;
                LoginWindow.HocSinhRole.IsChecked = false;  
                LoginWindow.LoginForm.Visibility = Visibility.Collapsed;
                LoginWindow.RoleForm.Visibility = Visibility.Visible;
                Username = "";
                Password = null;

            });
            TurnToLoginForm = new RelayCommand<object>((paramater) => { return true; }, (parameter) =>
            {
                LoginWindow.RoleForm.Visibility = Visibility.Collapsed;
                LoginWindow.LoginForm.Visibility = Visibility.Visible;
               
            });
        }
        void Log(Window paramater)
        {
            if (paramater == null)
                return;
            if (Username == "" || Password == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
            }
            else
            {
                string passEncode = CreateMD5(Base64Encode(Password));
                if (LoginWindow.GiaoVienRole.IsChecked == true)
                {
                    MessageBox.Show("Teacher");
                }
                if (LoginWindow.GiamHieuRole.IsChecked == true)
                {
                    MessageBox.Show("President");
                }
                if (LoginWindow.HocSinhRole.IsChecked == true)
                {
                    MessageBox.Show("Student");
                }
                //var AccCount = DataProvider.Ins.DB.UserAccounts.Where(x => x.UserName == Username).Count();
                //if (AccCount > 0)
                //{
                //    var CheckPass = DataProvider.Ins.DB.UserAccounts.Where(x => x.UserName == Username && x.UserPassword == passEncode).Count();
                //    if (CheckPass > 0)
                //    {
                //        IsLoggedIn = true;
                //        p.Close();
                //    }
                //    else
                //    {
                //        IsLoggedIn = false;
                //        MessageBoxOK wd = new MessageBoxOK();
                //        var data = wd.DataContext as MessageBoxOKViewModel;
                //        data.Content = "Wrong password";
                //        wd.ShowDialog();
                //        return;
                //    }
                //}
                //else
                //{
                //    IsLoggedIn = false;
                //    MessageBoxOK wd = new MessageBoxOK();
                //    var data = wd.DataContext as MessageBoxOKViewModel;
                //    data.Content = "User Account does not exists";
                //    wd.ShowDialog();
                //}
            }

        }

        // hàm mã hóa mật khẩu
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
    }
}
