using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.Login;
using System;
using System.Collections.Generic;
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
        public string Id{ get { return _id; } set { _id = value; } }

        private string _taikhoan;
        public string TaiKhoan { get => _taikhoan; set { _taikhoan = value; OnPropertyChanged(); } }
        private string _matkhau;
        public string MatKhau { get => _matkhau; set { _matkhau = value; OnPropertyChanged(); } }
        public ChangePasswordWindow ChangePWWD { get; set; }
        private StudentManagement.Model.HocSinh _hocsinhhientai;
        public StudentManagement.Model.HocSinh HocSinhHienTai { get => _hocsinhhientai; set { _hocsinhhientai = value; OnPropertyChanged(); } }

        private StudentManagement.Model.GiaoVien _giaovienhientai;
        public StudentManagement.Model.GiaoVien GiaoVienHienTai { get => _giaovienhientai; set { _giaovienhientai = value; OnPropertyChanged(); } }
        public ICommand LoadWindow { get; set; }
        public ICommand ChangePW { get; set; }
        public ChangePasswordViewModel()
        {
            MatKhau = "";
            Id = "";
            HocSinhHienTai = new StudentManagement.Model.HocSinh();
            LoadWindow = new RelayCommand<ChangePasswordWindow>((parameter) => { return true; }, (parameter) =>
            {
                ChangePWWD = parameter;

            });
            ChangePW = new RelayCommand<string>((parameter) => { return true; }, (parameter) =>
            {
                CapNhatMatKhau(parameter);
            });
        }
        public void CapNhatMatKhau(string mk)
        {
            if (ChangePWWD.PasswordOld.Password == "" | ChangePWWD.PasswordNew.Password == "" | ChangePWWD.PasswordNewConfirm.Password == "" )
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin");
            }

            if (ChangePWWD.PasswordOld.Password != "") { }
        }
    }
}
