﻿using StudentManagement.Model;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class DanhSachLopViewModel : BaseViewModel
    {
        private int _maLop;
        public int MaLop { get { return _maLop; } set { _maLop = value; } }
        private string _tenLop;
        public string TenLop { get { return _tenLop; } set { _tenLop = value;  } }
        public DanhSachLop DanhSachLopWindow { get; set; }
        private ObservableCollection<StudentManagement.Model.HocSinh> _danhSachLop;
        public ObservableCollection<StudentManagement.Model.HocSinh> DanhSachLop { get => _danhSachLop; set { _danhSachLop = value;  } }


        private bool _dataGridVisibility;

        public bool DataGridVisibility{get{return _dataGridVisibility;}set{_dataGridVisibility = value;}}

        private bool _progressBarVisibility;

        public bool ProgressBarVisibility { get {return _progressBarVisibility;}set{_progressBarVisibility = value;}
        }

        // declare ICommand

        public ICommand ThemHocSinh { get; set; }
        public ICommand RemoveKhoiLop { get; set; }
        public ICommand LoadWindow { get; set; }
        public ICommand LocHocSinh { get; set; }
        public ICommand Back { get; set; }
        public async Task LoadDanhSachHocSinh()
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open();

                    string CmdString = "select * from HocSinh where TenHocSinh is not null and MaLop = " + MaLop.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh
                        {
                            MaHocSinh = reader.GetInt32(0),
                            TenHocSinh = reader.GetString(1),
                            NgaySinh = reader.GetDateTime(2),
                            GioiTinh = reader.GetBoolean(3),
                            DiaChi = reader.GetString(4),
                            Email = reader.GetString(5),
                            Avatar = (byte[])reader[6],
                        };
                        DanhSachLop.Add(student);
                    }
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }

        public int[] XoaHocSinh(Model.HocSinh value)
        {
            int[] result = new int[3] ;
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {

                    sqlConnectionWrapper.Open();
                    SqlCommand cmd;
                    string CmdString = "Update HocSinh set MaLop = null where MaHocSinh = " + value.MaHocSinh;
                    cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    result[0] = cmd.ExecuteNonQuery();

                    CmdString = "Update HeThongDiem set MaLop = null where MaHocSinh = " + value.MaHocSinh;
                    cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    result[1] =  cmd.ExecuteNonQuery();

                    CmdString = "Update ThanhTich set MaLop = null where MaHocSinh = " + value.MaHocSinh;
                    cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    result[2] = cmd.ExecuteNonQuery();

                return result;
                
            }
        }
        public int[] HoanTac(Model.HocSinh value)
        {
            int[] result = new int[3] ;
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {

                    sqlConnectionWrapper.Open();
                    SqlCommand cmd;
                    string CmdString = "Update HocSinh set MaLop = " + MaLop.ToString() + " where MaHocSinh = " + value.MaHocSinh;
                    cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                result[0] = cmd.ExecuteNonQuery();

                CmdString = "Update HeThongDiem set MaLop = " + MaLop.ToString() + " where MaHocSinh = " + value.MaHocSinh;
                    cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                result[1] = cmd.ExecuteNonQuery();

                CmdString = "Update ThanhTich set MaLop = " + MaLop.ToString() + " where MaHocSinh = " + value.MaHocSinh;
                    cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                result[2] = cmd.ExecuteNonQuery();
                return result;
            }
        }
        public void LocHocSinhTheoTen(string value)
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open();
                    string CmdString = "select * from HocSinh where TenHocSinh is not null and MaLop = " + MaLop.ToString()
                                        + " and TenHocSinh like N'%" + value + "%'";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
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
                                Avatar = (byte[])reader[6],
                            };
                            DanhSachLop.Add(student);
                        }
                        reader.NextResult();
                    }
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }

        public DanhSachLopViewModel()
        {
            // Stryker disable all
            MaLop = 100;
            TenLop = "10A1";
            DanhSachLop = new ObservableCollection<Model.HocSinh>();
            LoadWindow = new RelayCommand<DanhSachLop>((parameter) => { return true; }, async (parameter) =>
            {
                DataGridVisibility = false;
                ProgressBarVisibility = true;
                DanhSachLopWindow = parameter;
                DanhSachLop.Clear();
                await LoadDanhSachHocSinh();
                DataGridVisibility = true;
                ProgressBarVisibility = false;
            });
            ThemHocSinh = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                XepLopChoHocSinh window = new XepLopChoHocSinh();
                XepLopViewModel data = window.DataContext as XepLopViewModel;
                data.LopHocDangChon.MaLop = MaLop;
                data.LopHocDangChon.TenLop = TenLop;
                window.ShowDialog();
                DataGridVisibility = false;
                ProgressBarVisibility = true;
                DanhSachLop.Clear();
                await LoadDanhSachHocSinh();
                DataGridVisibility = true;
                ProgressBarVisibility = false;
            });
            RemoveKhoiLop = new RelayCommand<Model.HocSinh>((parameter) => { return true; }, async (parameter) =>
            {
                Model.HocSinh item = parameter;
                XoaHocSinh(item);
                DataGridVisibility = false;
                ProgressBarVisibility = true;
                DanhSachLop.Clear();
                await LoadDanhSachHocSinh();
                DataGridVisibility = true;
                ProgressBarVisibility = false;
                // Hiện snackbar thông báo xóa thành công, có thể hoàn tác
                DanhSachLopWindow.Snackbar.MessageQueue?.Enqueue(
                $"Xóa thành công",
                $"Hoàn tác",
                param => { HoanTac(item); },
                TimeSpan.FromSeconds(5));
            });
            LocHocSinh = new RelayCommand<TextBox>((parameter) => { return true; }, async (parameter) =>
            {
                TextBox tb = parameter;
                DanhSachLop.Clear();
                LocHocSinhTheoTen(tb.Text);
            });
            Back = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {

                try
                {
                    DanhSachLopWindow.NavigationService.GoBack();
                }
                catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                }
            });
        }

    }
}