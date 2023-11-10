using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.Login;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentManagement.ViewModel.Login
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private string _id;
        public string Id { get { return _id; } set { _id = value; OnPropertyChanged(); } }

        private bool _ishs;
        public bool IsHS { get { return _ishs; } set { _ishs = value; OnPropertyChanged(); } }

        private string _taikhoan;
        public string TaiKhoan { get { return _taikhoan; } set { _taikhoan = value; OnPropertyChanged(); } }
        private string _matkhau;
        public string MatKhau { get => _matkhau; set { _matkhau = value; OnPropertyChanged(); } }
        public ChangePasswordWindow ChangePasswordWD { get; set; }
        private StudentManagement.Model.HocSinh _hocsinhhientai;
        public StudentManagement.Model.HocSinh HocSinhHienTai { get => _hocsinhhientai; set { _hocsinhhientai = value; OnPropertyChanged(); } }

        private StudentManagement.Model.GiaoVien _giaovienhientai;
        public StudentManagement.Model.GiaoVien GiaoVienHienTai { get => _giaovienhientai; set { _giaovienhientai = value; OnPropertyChanged(); } }
        public ICommand LoadWindow { get; set; }
        public ICommand ChangePW { get; set; }

        private readonly ISqlConnectionWrapper sqlConnection;

        public ChangePasswordViewModel(ISqlConnectionWrapper sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public ChangePasswordViewModel()
        {
            HocSinhHienTai = new StudentManagement.Model.HocSinh();
            LoadWindow = new RelayCommand<ChangePasswordWindow>((parameter) => { return true; }, (parameter) =>
            {
                ChangePasswordWD = parameter;

            });
            ChangePW = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (!CheckValidPassword(ChangePasswordWD.PasswordOld.Password, ChangePasswordWD.PasswordNew.Password, ChangePasswordWD.PasswordNewConfirm.Password))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng nhập đầy đủ thông tin";
                    MB.ShowDialog();
                    return;
                }
                try
                {
                    GetMatKhauCu();
                }
                catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                }


                if (CreateMD5(Base64Encode(ChangePasswordWD.PasswordOld.Password)).ToLower() != MatKhau)
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Sai mật khẩu cũ";
                    MB.ShowDialog();
                    return;
                }
                if (!ValidatePassword(ChangePasswordWD.PasswordNew.Password, ChangePasswordWD.PasswordNewConfirm.Password))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Mật khẩu phải chứa ít nhất 1 kí tự in hoa và một kí tự số. Mật khẩu xác nhận phải trùng với mật khẩu mới!";
                    MB.ShowDialog();
                    return;
                }
                if (CreateMD5(Base64Encode(ChangePasswordWD.PasswordNew.Password)).ToLower() == MatKhau)
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng nhập mật khẩu mới khác mật khẩu hiện tại.";
                    MB.ShowDialog();
                    return;
                }
                try
                {
                    UpdateMatKhauMoi(ChangePasswordWD.PasswordNew.Password);
                    ChangePasswordWD.Close();
                }
                catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                    return;
                }
            });
        }

        public bool CheckValidPassword(string oldPass, string newPass, string confirmPass)
        {
            if (string.IsNullOrEmpty(oldPass) || string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
                return false;
            return true;
        }

        public bool ValidatePassword(string newPass, string confirmPass)
        {
            if (newPass == null || confirmPass == null || newPass == "" || confirmPass == "")
                return false;
            bool flagUpcase = false, flagNum = false;
            foreach (char c in newPass)
            {
                if (c >= 'A' && c < 'Z' + 1)
                    flagUpcase = true;
                if (c >= '0' && c < '9' + 1)
                    flagNum = true;
            }
            return flagNum && flagUpcase && newPass == confirmPass;
        }

        public void GetMatKhauCu()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                sqlConnectionWrap.Open();
                try
                {
                    string cmdText = "";
                    if (IsHS)
                    {
                        cmdText = "select UserPassword from HocSinh where MaHocSinh = " + Id;
                    }
                    else
                    {
                        cmdText = "select UserPassword from GiaoVien where MaGiaoVien = " + Id;
                    }
                    SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();
                    MatKhau = reader.GetString(0);
                    sqlConnectionWrap.Close();
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }

        }

        public void UpdateMatKhauMoi(string matkhaumoi)
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string passEncode = CreateMD5(Base64Encode(matkhaumoi));
                    string CmdString;
                    if (IsHS)
                    {
                        CmdString = "Update HocSinh set UserPassword = \'" + passEncode +
                                            "\' where MaHocSinh = " + Id;
                    }
                    else
                    {
                        CmdString = "Update GiaoVien set UserPassword= \'" + passEncode +
                                                "\' where MaGiaoVien = " + Id;
                    }
                    {
                        SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                        cmd.ExecuteScalar();
                        //MessageBoxSuccessful MB = new MessageBoxSuccessful();
                        //MB.ShowDialog();
                        sqlConnectionWrap.Close();
                    }
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                    return;
                }
            }
        }

        // hàm mã hóa mật khẩu
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
