using StudentManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class DanhSachLopViewModel:BaseViewModel
    {
        private ObservableCollection<StudentManagement.Model.HocSinh> _danhSachLop;
        public ObservableCollection<StudentManagement.Model.HocSinh> DanhSachLop { get => _danhSachLop; set { _danhSachLop = value;OnPropertyChanged(); } }
        public DanhSachLopViewModel()
        {
            LoadDanhSachHocSinh();
        }
        public void LoadDanhSachHocSinh()
        {
            DanhSachLop = new ObservableCollection<Model.HocSinh>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select * from HocSinh where MaLop = '100'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh();
                        student.TenHocSinh = reader.GetString(1);
                        DanhSachLop.Add(student);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }

    }
}
