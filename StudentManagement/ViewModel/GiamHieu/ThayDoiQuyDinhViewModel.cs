using Microsoft.Win32;
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
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class ThayDoiQuyDinhViewModel: BaseViewModel
    {
        private ObservableCollection<StudentManagement.Model.QuiDinh> _danhSachQuyDinh;
        public ObservableCollection<StudentManagement.Model.QuiDinh> DanhSachQuyDinh { get => _danhSachQuyDinh; set { _danhSachQuyDinh = value; OnPropertyChanged(); } }
        public ThayDoiQuyDinhViewModel()
        {
            LoadThongTinCmb();
        }
        public void LoadThongTinCmb()
        {
            DanhSachQuyDinh = new ObservableCollection<StudentManagement.Model.QuiDinh>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select * from QuiDinh";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QuiDinh item = new QuiDinh();
                        item.MaQuiDinh = reader.GetInt32(0);
                        item.TenQuiDinh = reader.GetString(1);
                        item.GiaTri = reader.GetInt32(2);
                        DanhSachQuyDinh.Add(item);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
    }
    
}
