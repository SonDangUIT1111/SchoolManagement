using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class DanhSachGiaoVienViewModel : BaseViewModel
    {
        private ObservableCollection<StudentManagement.Model.GiaoVien> _danhSachGiaoVien;
        public ObservableCollection<StudentManagement.Model.GiaoVien> DanhSachGiaoVien { get => _danhSachGiaoVien; set { _danhSachGiaoVien = value; OnPropertyChanged(); } }

        //declare ICommand
        public ICommand LocGiaoVien { get; set; }

        public DanhSachGiaoVienViewModel()
        {
            LoadDanhSachGiaoVien();
            LocGiaoVien = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                TextBox tb = parameter;
                LocGiaoVienTheoTen(tb.Text);
            });
        }
        public void LoadDanhSachGiaoVien()
        {
            DanhSachGiaoVien = new ObservableCollection<Model.GiaoVien>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select * from GiaoVien";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.GiaoVien teacher = new StudentManagement.Model.GiaoVien();
                        teacher.TenGiaoVien = reader.GetString(1);
                        teacher.MaGiaoVien = reader.GetInt32(0);
                        teacher.NgaySinh = reader.GetDateTime(2);
                        teacher.GioiTinh = reader.GetBoolean(3);
                        teacher.DiaChi = reader.GetString(4);
                        teacher.Email = reader.GetString(5);
                        DanhSachGiaoVien.Add(teacher);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
        public void LocGiaoVienTheoTen(string value)
        {
            DanhSachGiaoVien.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select * from GiaoVien where TenGiaoVien is not null and TenGiaoVien like '%" + value + "%'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.GiaoVien teacher = new StudentManagement.Model.GiaoVien
                        {
                            MaGiaoVien = reader.GetInt32(0),
                            TenGiaoVien = reader.GetString(1),
                            NgaySinh = reader.GetDateTime(2),
                            GioiTinh = reader.GetBoolean(3),
                            DiaChi = reader.GetString(4),
                            Email = reader.GetString(5),
                        };
                        DanhSachGiaoVien.Add(teacher);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }

    }
}
