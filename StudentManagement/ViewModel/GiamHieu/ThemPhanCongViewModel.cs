﻿using Microsoft.Win32;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using StudentManagement.Views.GiamHieu;
using System.Text.RegularExpressions;
using StudentManagement.Model;
using System.Data.SqlClient;
using StudentManagement.Converter;
using System.Data;
using System.Collections.ObjectModel;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class ThemPhanCongViewModel:BaseViewModel
    {
        public ThemPhanCong ThemPhanCongWD { get; set; }
        public string NienKhoaQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }

        private ObservableCollection<string> _nienKhoaCmb;
        public ObservableCollection<string> NienKhoaCmb { get => _nienKhoaCmb; set { _nienKhoaCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.Khoi> _khoiCmb;
        public ObservableCollection<StudentManagement.Model.Khoi> KhoiCmb { get => _khoiCmb; set { _khoiCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.Lop> _lopCmb;
        public ObservableCollection<StudentManagement.Model.Lop> LopCmb { get => _lopCmb; set { _lopCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.MonHoc> _monHocCmb;
        public ObservableCollection<StudentManagement.Model.MonHoc> MonHocCmb { get => _monHocCmb; set { _monHocCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.GiaoVien> _giaoVienCmb;
        public ObservableCollection<StudentManagement.Model.GiaoVien> GiaoVienCmb { get => _giaoVienCmb; set { _giaoVienCmb = value; OnPropertyChanged(); } }
        public ICommand LoadData { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterLop { get; set; }
        public ICommand AddPhanCong { get; set; }
        public ICommand HuyThemPC { get; set; }

        public ThemPhanCongViewModel() {
            LoadThongTinCmb();
            LoadOptionFromSelection();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemPhanCongWD = parameter as ThemPhanCong;
                ThemPhanCongWD.cmbNienKhoa.SelectedIndex = 0;
                ThemPhanCongWD.cmbKhoi.SelectedIndex = 0;
                ThemPhanCongWD.cmbLop.SelectedIndex = 0;
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
                        if (ThemPhanCongWD.cmbLop.SelectedIndex >= 0)
                            LoadOptionFromSelection();
                        //else DanhSachHocSinh.Clear();
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
                        if (ThemPhanCongWD.cmbLop.SelectedIndex >= 0)
                            LoadOptionFromSelection();
                        //else DanhSachHocSinh.Clear();
                    }
                }
            });
            AddPhanCong = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                string nienkhoa = NienKhoaQueries;
                Lop lop = ThemPhanCongWD.cmbLop.SelectedItem as Lop;
                Model.MonHoc monhoc = ThemPhanCongWD.cmbMonHoc.SelectedItem as Model.MonHoc;
                Model.GiaoVien giaovien = ThemPhanCongWD.cmbGiaoVien.SelectedItem as Model.GiaoVien;
                if (monhoc == null)
                {
                    MessageBox.Show("Vui lòng chọn môn học!");
                } else if (giaovien==null)
                {
                    MessageBox.Show("Vui lòng chọn giáo viên giảng dạy!");
                } else
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                    {
                        con.Open();
                        string CmdString = "insert into PhanCongGiangDay(NienKhoa,MaLop,TenLop,SiSo,MaMon,TenMon,MaGiaoVienPhuTrach,TenGiaoVien) values (N'" + nienkhoa + "'," + lop.MaLop + ",N'" + lop.TenLop + "'," + lop.SiSo + "," + monhoc.MaMon + ",N'" + monhoc.TenMon + "'," + giaovien.MaGiaoVien + ",N'" + giaovien.TenGiaoVien + "')";
                        //MessageBox.Show(CmdString);
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    MessageBox.Show("Thêm phân công giảng dạy thành công!");
                    ThemPhanCongWD.Close();
                }
            });
            HuyThemPC = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemPhanCongWD.Close();
            });
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
                CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries + "' and MaKhoi = " + KhoiQueries;
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
        public void LoadOptionFromSelection()
        { 
            MonHocCmb = new ObservableCollection<StudentManagement.Model.MonHoc>();
            GiaoVienCmb = new ObservableCollection<StudentManagement.Model.GiaoVien>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select * from MonHoc where TenMon not in (select TenMon from PhanCongGiangDay where MaLop="+LopQueries+")";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Model.MonHoc item = new Model.MonHoc();
                        item.MaMon = reader.GetInt32(0);
                        item.TenMon = reader.GetString(1);
                        MonHocCmb.Add(item);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select MaGiaoVien, TenGiaoVien from GiaoVien";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Model.GiaoVien item = new Model.GiaoVien();
                        item.MaGiaoVien = reader.GetInt32(0);
                        item.TenGiaoVien = reader.GetString(1);
                        GiaoVienCmb.Add(item);
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
                string CmdString = "select Malop, TenLop, SiSo from Lop where MaKhoi = " + KhoiQueries + " and NienKhoa = '" + NienKhoaQueries + "'";
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
                        item.SiSo = reader.GetInt32(2);
                        LopCmb.Add(item);
                    }
                    reader.NextResult();
                }
                con.Close();
                if (LopCmb.Count > 0)
                {
                    ThemPhanCongWD.cmbLop.SelectedIndex = 0;
                }
                else
                {
                    ThemPhanCongWD.cmbLop.SelectedIndex = -1;
                }
            }
        }
    }
}
