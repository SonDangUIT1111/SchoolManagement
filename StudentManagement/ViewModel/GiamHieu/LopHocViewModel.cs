using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class LopHocViewModel: BaseViewModel
    {
        private Frame _frame;

        private ObservableCollection<StudentManagement.Model.Lop> _danhSachLopHoc;
        public ObservableCollection<StudentManagement.Model.Lop> DanhSachLopHoc { get => _danhSachLopHoc; set { _danhSachLopHoc = value; OnPropertyChanged(); } }
       

        public DanhSachLop DanhSachLopPage;
        public ICommand SwitchDanhSachLop { get; set; }

        public LopHocViewModel()
        {
            LoadDanhSachHocSinh();
        }
        public void LoadDanhSachHocSinh()
        {
            DanhSachLopHoc = new ObservableCollection<Model.Lop>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select * from Lop";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.Lop lophoc = new StudentManagement.Model.Lop();
                        lophoc.TenLop = reader.GetString(1);
                        DanhSachLopHoc.Add(lophoc);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
    }
}
