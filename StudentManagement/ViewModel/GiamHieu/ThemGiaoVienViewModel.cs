using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    internal class ThemGiaoVienViewModel : BaseViewModel
    {
        // khai báo biến
        public ThemGiaoVien ThemGiaoVienWD { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand LoadWindow { get; set; }
        public ICommand AddGiaoVien { get; set; }
        public ICommand RemoveGiaoVien { get; set; }
        public ThemGiaoVienViewModel()
        {
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

        public string ToShortDateTime(string st) {
            string Converted_String="";
            for (int i = 0; i < st.Length; i++)
            {
                if (st[i] == '/')
                    Converted_String += '-';
                else
                    Converted_String += st[i];
            }
            return Converted_String;
        }
            
        void ThemGiaoVienMoi()
        {
            if (ThemGiaoVienWD.TenGV.Text == "" |
                ThemGiaoVienWD.NgaySinh.Text == "" |
                ThemGiaoVienWD.DiaChi.Text == "" |
                ThemGiaoVienWD.Email.Text == "" |
                ThemGiaoVienWD.TaiKhoan.Text == "" |
                ThemGiaoVienWD.MatKhau.Text == "")
                MessageBox.Show("Vui lòng điền đầy đủ thông tin");
            else
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString = "INSERT INTO GiaoVien(TenGiaoVien,NgaySinh,GioiTinh,DiaChi,Email,Username,UserPassword,MaTruong) VALUES (\'" +
                        ThemGiaoVienWD.TenGV.Text + "\' , \'" +
                        ToShortDateTime(ThemGiaoVienWD.NgaySinh.DisplayDate.ToString()) + "\' ," + ThemGiaoVienWD.GioiTinh.SelectedIndex.ToString() +
                        ", \'" + ThemGiaoVienWD.DiaChi.Text + "\' , \'" + ThemGiaoVienWD.Email.Text + "\' , \'" +
                        ThemGiaoVienWD.TaiKhoan.Text + "\' , \' " + ThemGiaoVienWD.MatKhau.Text + "\' , 1);";
                    //MessageBox.Show(CmdString);
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    cmd.ExecuteScalar();
                    MessageBox.Show("Thêm giáo viên thành công!");
                    con.Close();
                }
            }
        }
    }
}
