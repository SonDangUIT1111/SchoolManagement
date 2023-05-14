﻿using StudentManagement.Model;
using StudentManagement.Views.GiaoVien;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiaoVien
{
    internal class LopHocViewModel : BaseViewModel
    {

        private int _idGiaoVien;
        public int IdGiaoVien { get { return _idGiaoVien; } set { _idGiaoVien = value; } }
        public string NienKhoaQueries;
        public string KhoiQueries;
        public string LopQueries;
        public StudentManagement.Model.Lop LopDaChon { get; set; }
        public LopHoc LopHocWD { get; set; }

        private ObservableCollection<StudentManagement.Model.HocSinh> _danhSachhs;
        public ObservableCollection<StudentManagement.Model.HocSinh> DanhSachhs { get => _danhSachhs; set { _danhSachhs = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Khoi> _danhsachkhoi;
        public ObservableCollection<Model.Khoi> DanhSachKhoi { get => _danhsachkhoi; set { _danhsachkhoi = value; OnPropertyChanged(); } }

        private ObservableCollection<string> _danhsachnienkhoa;
        public ObservableCollection<string> DanhSachNienKhoa { get => _danhsachnienkhoa; set { _danhsachnienkhoa = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.Lop> _danhsachlop;
        public ObservableCollection<StudentManagement.Model.Lop> DanhSachLop { get => _danhsachlop; set { _danhsachlop = value; OnPropertyChanged(); } }
        public ICommand LoadWindow { get; set; }
        public ICommand LocHocSinh { get; set; }
        public ICommand LoadKhoi { get; set; }
        public ICommand LoadLop { get; set; }
        public ICommand LoadHocSinh { get; set; }
        public ICommand UpdateHocSinh { get; set; }
        public LopHocViewModel()
        {
            DanhSachKhoi = new ObservableCollection<Model.Khoi>();
            DanhSachhs = new ObservableCollection<Model.HocSinh>();
            DanhSachNienKhoa = new ObservableCollection<string>();
            DanhSachLop = new ObservableCollection<Model.Lop>();
            LoadWindow = new RelayCommand<LopHoc>((parameter) => { return true; }, (parameter) =>
            {
                LopHocWD = parameter;
                LoadDanhSachNienKhoa();
            });
            LocHocSinh = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                TextBox tb = parameter;
                LocHocSinhTheoTen(tb);
            });
            LoadKhoi = new RelayCommand<String>((parameter) => { return true; }, (parameter) =>
            {
                if (LopHocWD.ChonKhoa != null && LopHocWD.ChonKhoa.SelectedItem != null)
                {
                    NienKhoaQueries = LopHocWD.ChonKhoa.SelectedItem.ToString();
                }
                LoadDanhSachKhoi();

            });
            LoadLop = new RelayCommand<String>((parameter) => { return true; }, (parameter) =>
            {
                if (LopHocWD.ChonKhoi != null && LopHocWD.ChonKhoi.SelectedItem != null)
                {
                    Khoi item = LopHocWD.ChonKhoi.SelectedItem as Khoi;
                    KhoiQueries = item.MaKhoi.ToString();
                }
                LoadDanhSachLop();
            });
            LoadHocSinh = new RelayCommand<String>((parameter) => { return true; }, (parameter) =>
            {
                if (LopHocWD.ChonLop != null && LopHocWD.ChonLop.SelectedItem != null)
                {
                    Lop item = LopHocWD.ChonLop.SelectedItem as Lop;
                    LopQueries = item.MaLop.ToString(); 
                }
                LoadDanhSachHocSinh();
            });
            UpdateHocSinh = new RelayCommand<Model.HocSinh>((parameter) => { return true; }, (parameter) =>
             {
                 if (!IsGiaoVienChuNhiem(parameter.MaHocSinh.ToString()))
                 {
                     MessageBox.Show("Bạn không thể chỉnh sửa đối tượng này");
                     return;
                 }
                 SuaHocSinh window = new SuaHocSinh();
                 SuaHocSinhViewModel data = window.DataContext as SuaHocSinhViewModel;
                 data.HocSinhHienTai = parameter;
                 window.ShowDialog();
                 LoadDanhSachHocSinh();
             });
        }
        public bool IsGiaoVienChuNhiem(string ID)
        {
            int IdGVCN;
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        con.Open();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return false;
                    }
                    string CmdString = "select MaGVCN from HocSinh,Lop where HocSinh.MaLop = Lop.MaLop and MaHocSinh = " + ID;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows) reader.Read();
                    IdGVCN = reader.GetInt32(0);
                    con.Close();
                    if (IdGVCN != IdGiaoVien) return false;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
        }
        public void LoadDanhSachHocSinh()
        {
            DanhSachhs.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try 
                    { 
                        con.Open(); 
                    } catch (Exception)
                    { 
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return; 
                    }
                    string CmdString = "select MaHocSinh,TenHocSinh,NgaySinh,GioiTinh,DiaChi,Email,AnhThe from HocSinh where MaLop = \'" + LopQueries + "\'";
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
                            student.Email = reader.GetString(5);
                            student.Avatar = (byte[])reader.GetValue(6);
                            DanhSachhs.Add(student);
                        }
                        reader.NextResult();
                    }
                    con.Close();
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void LoadDanhSachKhoi()
        {
            DanhSachKhoi.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try 
                    { 
                        con.Open();
                    } catch (Exception)
                    { 
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                        return; 
                    }
                    string CmdString = "select MaKhoi,Khoi from Khoi";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DanhSachKhoi.Add(new Khoi()
                            {
                                MaKhoi = reader.GetInt32(0),
                                TenKhoi = reader.GetString(1),
                            });
                            if (String.IsNullOrEmpty(KhoiQueries))
                                KhoiQueries = reader.GetInt32(0).ToString();
                        }
                        reader.NextResult();
                    }
                    con.Close();
                    if (LopHocWD.ChonKhoi != null && LopHocWD.ChonKhoi.Items.Count > 0 && LopHocWD.ChonKhoi.SelectedIndex != 0)
                    {
                        LopHocWD.ChonKhoi.SelectedIndex = 0;
                        LoadDanhSachLop();
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        public void LoadDanhSachNienKhoa()
        {
            DanhSachNienKhoa.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try 
                    { 
                        con.Open();
                    } catch (Exception) 
                    { 
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                        return;
                    }
                    string CmdString = "Select distinct NienKhoa from Lop";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string nienkhoa = reader.GetString(0);
                            DanhSachNienKhoa.Add(nienkhoa);
                            if (String.IsNullOrEmpty(NienKhoaQueries))
                                NienKhoaQueries = reader.GetString(0);
                        }
                        reader.NextResult();
                    }
                    con.Close();
                    if (LopHocWD.ChonKhoa != null && LopHocWD.ChonKhoa.Items.Count > 0 && LopHocWD.ChonKhoa.SelectedIndex != 0)
                    {
                        LopHocWD.ChonKhoa.SelectedIndex = 0;
                        LoadDanhSachKhoi();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void LoadDanhSachLop()
        {
            DanhSachLop.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try
                    { 
                        con.Open();
                    } catch (Exception) 
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                        return; 
                    }
                    if (String.IsNullOrEmpty(KhoiQueries))
                        KhoiQueries = "0";
                    string CmdString = "select MaLop,TenLop from Lop where MaKhoi = " + KhoiQueries +
                        " and NienKhoa = '" + NienKhoaQueries + "'";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.Lop lop = new StudentManagement.Model.Lop();
                            lop.MaLop = reader.GetInt32(0);
                            lop.TenLop = reader.GetString(1);
                            DanhSachLop.Add(lop);
                            if (String.IsNullOrEmpty(LopQueries))
                                LopQueries = reader.GetInt32(0).ToString();
                        }
                        reader.NextResult();
                    }
                    con.Close();
                    if (LopHocWD.ChonLop != null && LopHocWD.ChonLop.Items.Count > 0 && LopHocWD.ChonLop.SelectedIndex != 0)
                    {
                        LopHocWD.ChonLop.SelectedIndex = 0;
                        LoadDanhSachHocSinh();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void LocHocSinhTheoTen(TextBox tb)
        {
            DanhSachhs.Clear();
            string value = tb.Text;
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        con.Open();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return;
                    }
                    string CmdString = "select MaHocSinh,TenHocSinh,NgaySinh,GioiTinh,DiaChi,Email,AnhThe from HocSinh where MaLop = " + LopQueries + " and TenHocSinh like N'%"+value+"%'";
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
                            student.Email = reader.GetString(5);
                            student.Avatar = (byte[])reader.GetValue(6);
                            DanhSachhs.Add(student);
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
