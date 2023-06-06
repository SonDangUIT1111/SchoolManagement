using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.Login;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentManagement.ViewModel.Login
{
    internal class ChangePasswordViewModel : BaseViewModel
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
        public ChangePasswordViewModel()
        {
            HocSinhHienTai = new StudentManagement.Model.HocSinh();
            LoadWindow = new RelayCommand<ChangePasswordWindow>((parameter) => { return true; }, (parameter) =>
            {
                ChangePasswordWD = parameter;

            });
            ChangePW = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                CapNhatMatKhau();
            });
        }
        public void CapNhatMatKhau()
        {
            if (ChangePasswordWD.PasswordOld.Password == "" || ChangePasswordWD.PasswordNew.Password == "" || ChangePasswordWD.PasswordNewConfirm.Password == "")
            {
                MessageBoxOK MB = new MessageBoxOK();
                var data = MB.DataContext as MessageBoxOKViewModel;
                data.Content = "Vui lòng nhập đầy đủ thông tin";
                MB.ShowDialog();
                return;
            }

            if (ChangePasswordWD.PasswordOld.Password != MatKhau)
            {
                MessageBoxOK MB = new MessageBoxOK();
                var data = MB.DataContext as MessageBoxOKViewModel;
                data.Content = "Sai mật khẩu cũ";
                MB.ShowDialog();
                return;
            }
            if (ChangePasswordWD.PasswordNew.Password != ChangePasswordWD.PasswordNewConfirm.Password)
            {
                MessageBoxOK MB = new MessageBoxOK();
                var data = MB.DataContext as MessageBoxOKViewModel;
                data.Content = "Sai mật khẩu xác nhận";
                MB.ShowDialog();
                return;
            }
            if (ChangePasswordWD.PasswordNew.Password == MatKhau)
            {
                MessageBoxOK MB = new MessageBoxOK();
                var data = MB.DataContext as MessageBoxOKViewModel;
                data.Content = "Vui lòng nhập mật khẩu mới khác mật khẩu hiện tại.";
                MB.ShowDialog();
                return;
            }
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
                        MessageBoxFail MB = new MessageBoxFail();
                        MB.ShowDialog();
                        return;
                    }
                    string CmdString;
                    if (IsHS)
                    {
                        CmdString = "Update HocSinh set UserPassword = \'" + ChangePasswordWD.PasswordNew.Password +
                                            "\' where MaHocSinh = " + Id;
                    }
                    else
                    {
                        CmdString = "Update GiaoVien set UserPassword= \'" + ChangePasswordWD.PasswordNew.Password +
                                                "\' where MaGiaoVien = " + Id;
                    }
                    {
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        cmd.ExecuteScalar();
                        MessageBoxSuccessful MB = new MessageBoxSuccessful();
                        MB.ShowDialog();
                        con.Close();
                    }
                    ChangePasswordWD.Close();
                }
                catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                    return;
                }
            }
        }
    }
}
