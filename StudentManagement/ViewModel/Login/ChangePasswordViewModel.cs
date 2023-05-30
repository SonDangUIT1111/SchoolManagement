using StudentManagement.Converter;
using StudentManagement.Model;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.Login;
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
        public string Id{ get { return _id; } set { _id = value; OnPropertyChanged(); } }

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
            if (ChangePasswordWD.PasswordOld.Password == "" | ChangePasswordWD.PasswordNew.Password == "" | ChangePasswordWD.PasswordNewConfirm.Password == "" )
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin");
                return;
            }

            if (ChangePasswordWD.PasswordOld.Password != MatKhau)
            {
                MessageBox.Show("Sai mật khẩu cũ");
                return;
            }
            if (ChangePasswordWD.PasswordNew.Password != ChangePasswordWD.PasswordNewConfirm.Password)
            {
                MessageBox.Show("Sai mật khẩu xác nhận");
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
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
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
                        MessageBox.Show("Cập nhật thành công!");
                        con.Close();
                    }
                    ChangePasswordWD.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }
    }
}
