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
    public class XepLopViewModel:BaseViewModel
    {

        // khai báo biến
        private Lop _lopHocDangChon;
        public Lop LopHocDangChon { get { return _lopHocDangChon; } set { _lopHocDangChon = value;OnPropertyChanged(); } }
        public XepLopChoHocSinh XepLopWD { get; set; }
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
        public ICommand CancelCommand { get; set; }
        public ICommand XepLop { get; set; }
        public ICommand LoadWindow { get; set; }
        public XepLopViewModel()
        {
            LopHocDangChon = new Lop(); 
            LoadNamSinh();
            // define command
            LoadWindow = new RelayCommand<XepLopChoHocSinh>((parameter) => { return true; }, (parameter) =>
            {
                XepLopWD = parameter;
                LoadDanhSachHocSinh();
                SelectCheckBox = new bool[DanhSachHocSinh.Count];
            });
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
            CancelCommand = new RelayCommand<object>((parameter) => { return true; }, (parameter) => 
            {
                XepLopWD.Close();
            });
            XepLop = new RelayCommand<DataGrid>((parameter) => { return true; }, (parameter) =>
            {
                ThemHocSinhVaoLop();
            });
        }
        void LoadNamSinh()
        {
            NamSinhCmb = new ObservableCollection<string>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select distinct Year(NgaySinh) from HocSinh where TenHocSinh is not null and (MaLop <> "+LopHocDangChon.MaLop.ToString()
                                    + " or MaLop is null)";
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
                string CmdString = "select * from HocSinh where TenHocSinh is not null and (MaLop <> " + LopHocDangChon.MaLop.ToString()
                                    +" or MaLop is null)";
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
                string CmdString = "select * from HocSinh where TenHocSinh is not null and Year(NgaySinh) ="+value+ " and (MaLop <> " + LopHocDangChon.MaLop.ToString()
                                    +" or MaLop is null)" + " and TenHocSinh like '%"+XepLopWD.tbSearch.Text+"%'";
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
                string CmdString = "select * from HocSinh where TenHocSinh is not null and TenHocSinh like '%" + value + "%' and (MaLop <>" + LopHocDangChon.MaLop.ToString()
                                    + " or MaLop is null)";

                if (XepLopWD.cmbNamSinh.SelectedItem != null)
                {
                    CmdString = "select * from HocSinh where TenHocSinh is not null and TenHocSinh like '%" + value + "%' and (MaLop <>" + LopHocDangChon.MaLop.ToString()
                                    + " or MaLop is null) and Year(NgaySinh) = "+XepLopWD.cmbNamSinh.SelectedItem.ToString() ;
                }
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
        void ThemHocSinhVaoLop()
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thêm những học sinh này vào lớp "
                                                        +LopHocDangChon.TenLop,"Thông báo",MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString = "";
                    SqlCommand cmd;
                    for (int i = 0; i < SelectCheckBox.Length; i++)
                    {
                        if (SelectCheckBox[i] == true)
                        {
                            CmdString = "Update HocSinh set MaLop = " + LopHocDangChon.MaLop + " ,TenLop = '" + LopHocDangChon.TenLop + "' " +
                                        " where MaHocSinh = " + DanhSachHocSinh[i].MaHocSinh;
                            cmd = new SqlCommand(CmdString, con);
                            cmd.ExecuteScalar();

                            CmdString = "Update HeThongDiem set MaLop = " + LopHocDangChon.MaLop + " ,TenLop = '" + LopHocDangChon.TenLop + "' " +
                                        " where MaHocSinh = " + DanhSachHocSinh[i].MaHocSinh;
                            cmd = new SqlCommand(CmdString, con);
                            cmd.ExecuteScalar();

                            CmdString = "Update ThanhTich set MaLop = " + LopHocDangChon.MaLop + " ,TenLop = '" + LopHocDangChon.TenLop + "' " +
                                        " where MaHocSinh = " + DanhSachHocSinh[i].MaHocSinh;
                            cmd = new SqlCommand(CmdString, con);
                            cmd.ExecuteScalar();
                        }
                    }
                    con.Close();
                }
            }
            MessageBox.Show("Thêm thành công");
            XepLopWD.Close();
            
        }
    }
}
