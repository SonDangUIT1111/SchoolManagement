﻿using StudentManagement.Model;
using StudentManagement.Views.GiaoVien;
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

namespace StudentManagement.ViewModel.GiaoVien
{
    internal class LopHocViewModel : BaseViewModel
    {
        public StudentManagement.Model.Lop LopDaChon { get; set; }
        public LopHoc LopHocWD { get; set; }

        private ObservableCollection<StudentManagement.Model.HocSinh> _danhSachhs;
        public ObservableCollection<StudentManagement.Model.HocSinh> DanhSachhs { get => _danhSachhs; set { _danhSachhs = value; OnPropertyChanged(); } }
        
        private ObservableCollection<String> _danhsachkhoi;
        public ObservableCollection<String> DanhSachKhoi { get => _danhsachkhoi; set { _danhsachkhoi = value; OnPropertyChanged(); } }

        private ObservableCollection<String> _danhsachnienkhoa;
        public ObservableCollection<String> DanhSachNienKhoa { get => _danhsachnienkhoa; set { _danhsachnienkhoa = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.Lop> _danhsachlop;
        public ObservableCollection<StudentManagement.Model.Lop> DanhSachLop { get => _danhsachlop; set { _danhsachlop = value; OnPropertyChanged(); } }
        public ICommand LoadWindow { get; set; }
        public ICommand LocHocSinh { get; set; }
        public ICommand LoadKhoa { get; set; }
        public ICommand LoadLop { get; set; }
        public ICommand LoadHocSinh { get; set; }
        public LopHocViewModel()
        {
            LoadWindow = new RelayCommand<LopHoc>((parameter) => { return true; }, (parameter) =>
            {
                LopHocWD = parameter;
                LopHocWD.ChonKhoa.IsEnabled = false;
                LopHocWD.ChonLop.IsEnabled = false;
            });
            LocHocSinh = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                TextBox tb = parameter;
            });
            LoadKhoa = new RelayCommand<String>((parameter) => { return true; }, (parameter) =>
            { 
                LoadDanhSachNienKhoa();
            });
            LoadLop = new RelayCommand<String>((parameter) => { return true; }, (parameter) =>
            {
                LoadDanhSachLop();
            });
            LoadHocSinh = new RelayCommand<String>((parameter) => { return true; }, (parameter) =>
            {
                LoadDanhSachHocSinh();
            });
            //LoadDanhSachHocSinh();
            LoadDanhSachKhoi();
            //
            //LoadDanhSachLop();
        }
        public void LoadDanhSachHocSinh()
        {
            if (LopHocWD.ChonLop.SelectedItem == null) return;//MessageBox.Show("lag begin");
            DanhSachhs = new ObservableCollection<Model.HocSinh>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                //string CmdString = "select MaHocSinh,TenHocSinh from HocSinh where MaLop = '100'";
                string CmdString = "select MaHocSinh,TenHocSinh from HocSinh where MaLop = \'" + LopDaChon.MaLop.ToString() + "\'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh();
                        student.TenHocSinh = reader.GetString(1);
                        DanhSachhs.Add(student);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
        public void LoadDanhSachKhoi()
        {
            DanhSachKhoi = new ObservableCollection<String>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select Khoi from Khoi";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string khoi = reader.GetString(0);
                        DanhSachKhoi.Add(khoi);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
            if (DanhSachhs != null) DanhSachhs.Clear();
            
        }
        public void LoadDanhSachNienKhoa()
        {
            LopHocWD.ChonKhoa.IsEnabled = true;
            LopHocWD.ChonLop.IsEnabled = false;
            DanhSachNienKhoa = new ObservableCollection<String>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "Select distinct NienKhoa from Lop where Khoi = \'" + LopHocWD.ChonKhoi.SelectedItem.ToString() + "\'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string nienkhoa = reader.GetString(0);
                        DanhSachNienKhoa.Add(nienkhoa);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
            if (DanhSachhs != null) DanhSachhs.Clear();
        }
        public void LoadDanhSachLop()
        {
            if (LopHocWD.ChonKhoa.SelectedItem == null) { LopHocWD.ChonLop.IsEnabled = false; return; }
            LopHocWD.ChonLop.IsEnabled = true;
            DanhSachLop = new ObservableCollection<StudentManagement.Model.Lop>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select MaLop,TenLop from Lop where Khoi = \'" + LopHocWD.ChonKhoi.SelectedItem.ToString() + 
                    "\' and NienKhoa = \'" + LopHocWD.ChonKhoa.SelectedItem.ToString() + "\'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.Lop lop= new StudentManagement.Model.Lop();
                        lop.MaLop = reader.GetInt32(0);
                        lop.TenLop = reader.GetString(1);
                        
                        DanhSachLop.Add(lop);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
            if (DanhSachhs != null) DanhSachhs.Clear();
        }
    }
}
