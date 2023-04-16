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
    public class ThongTinHocSinhViewModel : BaseViewModel
    {
        public ThongTinHocSinh ThongTinHocSinhWD { get; set; }
        public string NienKhoaQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }
        private ObservableCollection<StudentManagement.Model.HocSinh> _danhSachHocSinh;
        public ObservableCollection<StudentManagement.Model.HocSinh> DanhSachHocSinh { get => _danhSachHocSinh; set { _danhSachHocSinh = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _nienKhoaCmb;
        public ObservableCollection<string> NienKhoaCmb { get => _nienKhoaCmb; set { _nienKhoaCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.Khoi> _khoiCmb;
        public ObservableCollection<StudentManagement.Model.Khoi> KhoiCmb { get => _khoiCmb; set { _khoiCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.Lop> _lopCmb;
        public ObservableCollection<StudentManagement.Model.Lop> LopCmb { get => _lopCmb; set { _lopCmb = value; OnPropertyChanged(); } }
        //public ICommand Test { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterLop { get; set; }
        public ICommand StudentSearch { get; set; }
        public ICommand AddStudent { get; set; }
        public ThongTinHocSinhViewModel()
        {
            NienKhoaQueries = "";
            KhoiQueries = "";
            LopQueries = "";
            LoadThongTinCmb();
            LoadThongTinHocSinh();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThongTinHocSinhWD = parameter as ThongTinHocSinh;
                ThongTinHocSinhWD.cmbNienKhoa.SelectedIndex = 0;
                ThongTinHocSinhWD.cmbKhoi.SelectedIndex = 0;
                ThongTinHocSinhWD.cmbLop.SelectedIndex = 0;
            });
            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    string item = cmb.SelectedItem as string;
                    if (item != null)
                    {
                        NienKhoaQueries = item.ToString();
                        FilterLopFromSelection();
                        if (ThongTinHocSinhWD.cmbLop.SelectedIndex >= 0)
                            LoadThongTinHocSinh();
                        else DanhSachHocSinh.Clear();
                    }
                }
            });
            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null) 
                { 
                    Khoi item = cmb.SelectedItem as Khoi;
                    if (item != null)
                    {
                        KhoiQueries = item.MaKhoi.ToString();
                        FilterLopFromSelection();
                    }
                }
            });
            FilterLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    Lop item = cmb.SelectedItem as Lop;
                    if (item != null)
                    {
                        LopQueries = item.MaLop.ToString();
                        if (ThongTinHocSinhWD.cmbLop.SelectedIndex >= 0)
                            LoadThongTinHocSinh();
                        else DanhSachHocSinh.Clear();
                    }
                }
            });
            StudentSearch = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                DanhSachHocSinh.Clear();
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString = "select MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, DiaChi from HocSinh where MaLop = " + LopQueries + " and TenHocSinh like '%" + parameter.Text + "%'";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh();
                            student.MaHocSinh = reader.GetInt32(0);
                            student.TenHocSinh = reader.GetString(1);
                            student.NgaySinh = reader.GetDateTime(2);
                            student.GioiTinh = reader.GetBoolean(3);
                            student.DiaChi = reader.GetString(4);
                            DanhSachHocSinh.Add(student);
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
            });
        }
        public void LoadThongTinHocSinh()
        {
            DanhSachHocSinh = new ObservableCollection<Model.HocSinh>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, DiaChi from HocSinh where MaLop = " + LopQueries;
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh();
                        student.MaHocSinh = reader.GetInt32(0);
                        student.TenHocSinh = reader.GetString(1);
                        student.NgaySinh = reader.GetDateTime(2);
                        student.GioiTinh = reader.GetBoolean(3);
                        student.DiaChi = reader.GetString(4);
                        DanhSachHocSinh.Add(student);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
        public void LoadThongTinCmb()
        {
            NienKhoaCmb = new ObservableCollection<string>();
            KhoiCmb = new ObservableCollection<StudentManagement.Model.Khoi>();
            LopCmb = new ObservableCollection<StudentManagement.Model.Lop>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select distinct NienKhoa from Lop";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NienKhoaCmb.Add(reader.GetString(0));
                        if (String.IsNullOrEmpty(NienKhoaQueries))
                        {
                            NienKhoaQueries = reader.GetString(0);
                        }
                    }
                    reader.NextResult();
                }
                con.Close();

                con.Open();
                CmdString = "select distinct MaKhoi,Khoi from Khoi";
                cmd = new SqlCommand(CmdString, con);
                reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Khoi item = new Khoi();
                        item.MaKhoi = reader.GetInt32(0);
                        item.TenKhoi = reader.GetString(1);
                        if (String.IsNullOrEmpty(KhoiQueries))
                        {
                            KhoiQueries = reader.GetInt32(0).ToString();
                        }
                        KhoiCmb.Add(item);
                    }
                    reader.NextResult();
                }
                con.Close();

                con.Open();
                CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries+"' and MaKhoi = " + KhoiQueries;
                cmd = new SqlCommand(CmdString, con);
                reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Lop item = new Lop();
                        item.MaLop = reader.GetInt32(0);
                        item.TenLop = reader.GetString(1);
                        if (String.IsNullOrEmpty(LopQueries))
                        {
                            LopQueries = reader.GetInt32(0).ToString();
                        }
                        LopCmb.Add(item);
                    }
                    reader.NextResult();
                }
                con.Close();
                
                
            }
        }
        public void FilterLopFromSelection()
        {
            LopCmb.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select Malop, TenLop from Lop where MaKhoi = " + KhoiQueries + " and NienKhoa = '" + NienKhoaQueries + "'";
                //MessageBox.Show(CmdString);
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Lop item = new Lop();
                        item.MaLop = reader.GetInt32(0);
                        item.TenLop = reader.GetString(1);
                        LopCmb.Add(item);
                    }
                    reader.NextResult();
                }
                con.Close();
                if (LopCmb.Count > 0)
                {
                    ThongTinHocSinhWD.cmbLop.SelectedIndex = 0;
                }
                else
                {
                    ThongTinHocSinhWD.cmbLop.SelectedIndex = -1;
                }
            }
        }
    }
}
