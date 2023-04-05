using StudentManagement.Model;
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
    public class XepLopViewModel:BaseViewModel
    {

        // khai báo biến
        private ObservableCollection<StudentManagement.Model.HocSinh> _danhSachHocSinh;
        public ObservableCollection<StudentManagement.Model.HocSinh> DanhSachHocSinh { get => _danhSachHocSinh; set { _danhSachHocSinh = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _namSinhCmb;
        public ObservableCollection<string> NamSinhCmb { get => _namSinhCmb; set { _namSinhCmb = value; OnPropertyChanged(); } }
        private bool[] _selectCheckBox;
        public bool[] SelectCheckBox { get => _selectCheckBox; set { _selectCheckBox = value; OnPropertyChanged(); } }

        // khai báo ICommand
        public ICommand FindTheoNamSinh { get; set; }
        public ICommand Filter { get; set; }
        public ICommand DanhDau { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand XepLop { get; set; }
        public XepLopViewModel()
        {
            LoadNamSinh();
            LoadDanhSachHocSinh();
            SelectCheckBox = new bool[DanhSachHocSinh.Count];
            // define command
            FindTheoNamSinh = new RelayCommand<object>((parameter) => { return true; },(parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                string value = cmb.SelectedItem as string;
                LoadDanhSachTheoNamSinh(value);
            });
            Filter = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
             {
                 TextBox textBox = parameter as TextBox;
                 string text = textBox.Text;
                 LocDanhSach(text);
             });
            DanhDau = new RelayCommand<DataGrid>((parameter) => { return true; }, (parameter) =>
            {
                int location = parameter.SelectedIndex;
                SelectCheckBox[location] = !SelectCheckBox[location];
            });
            Cancel = new RelayCommand<Window>((parameter) => { return true; }, (parameter) => { parameter.Close(); });
            XepLop = new RelayCommand<DataGrid>((parameter) => { return true; }, (parameter) =>
            {
                
            });
        }
        void LoadNamSinh()
        {
            NamSinhCmb = new ObservableCollection<string>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select distinct Year(NgaySinh) from HocSinh where TenHocSinh is not null";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NamSinhCmb.Add(reader.GetInt32(0).ToString());
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
        public void LoadDanhSachHocSinh()
        {
            DanhSachHocSinh = new ObservableCollection<Model.HocSinh>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select * from HocSinh where TenHocSinh is not null";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh
                        {
                            MaHocSinh = reader.GetInt32(0),
                            TenHocSinh = reader.GetString(1),
                            NgaySinh = reader.GetDateTime(2),
                            GioiTinh = reader.GetBoolean(3),
                            DiaChi = reader.GetString(4),
                            Email = reader.GetString(5),
                        };
                        DanhSachHocSinh.Add(student);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }

        public void LoadDanhSachTheoNamSinh(string value)
        {
            DanhSachHocSinh.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select * from HocSinh where TenHocSinh is not null and Year(NgaySinh) ="+value+"";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh
                        {
                            MaHocSinh = reader.GetInt32(0),
                            TenHocSinh = reader.GetString(1),
                            NgaySinh = reader.GetDateTime(2),
                            GioiTinh = reader.GetBoolean(3),
                            DiaChi = reader.GetString(4),
                            Email = reader.GetString(5),
                        };
                        DanhSachHocSinh.Add(student);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
        public void LocDanhSach(string value)
        {
            DanhSachHocSinh.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select * from HocSinh where TenHocSinh is not null and TenHocSinh like '%" + value + "%'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh
                        {
                            MaHocSinh = reader.GetInt32(0),
                            TenHocSinh = reader.GetString(1),
                            NgaySinh = reader.GetDateTime(2),
                            GioiTinh = reader.GetBoolean(3),
                            DiaChi = reader.GetString(4),
                            Email = reader.GetString(5),
                        };
                        DanhSachHocSinh.Add(student);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
    }
}
