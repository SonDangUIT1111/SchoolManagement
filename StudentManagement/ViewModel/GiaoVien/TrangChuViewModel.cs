﻿using StudentManagement.Model;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.GiaoVien;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiaoVien
{
    public class TrangChuViewModel : BaseViewModel
    {
        //declare variable
        private Model.GiaoVien _currentUser;
        public Model.GiaoVien CurrentUser { get { return _currentUser; } set { _currentUser = value;OnPropertyChanged(); } }
        public GiaoVienWindow GiaoVienWD { get; set; }
        //declare Pages
        public StudentManagement.Views.GiamHieu.BaoCaoMonHoc BaoCaoPage { get; set; }
        public StudentManagement.Views.GiamHieu.BaoCaoTongKetHocKy BaoCaoHocKyPage { get; set; }
        public StudentManagement.Views.GiaoVien.LopHoc LopHocPage { get; set; }
        public StudentManagement.Views.GiaoVien.ThanhTichHocSinh ThanhTichHocSinhPage { get; set; }
        public StudentManagement.Views.GiaoVien.HeThongBangDiem HeThongBangDiemPage { get; set; }
        public StudentManagement.Views.GiaoVien.ThongTinTruong ThongTinTruongPage { get; set; }
        public StudentManagement.Views.GiaoVien.SuaThongTinCaNhan ThongTinCaNhanPage { get; set; }

        //declare ICommand
        public ICommand LoadWindow { get; set; }
        public ICommand SwitchThongTinTruong { get; set; }
        public ICommand SwitchLopHoc { get; set; }
        public ICommand SwitchThanhTichHocSinh { get; set; }
        public ICommand SwitchQuanLiBangDiem { get; set; }
        public ICommand SwitchBaoCaoMonHoc { get; set; }
        public ICommand SwitchBaoCaoHocKy { get; set; }
        public ICommand SuaThongTinCaNhan { get; set; }
        public TrangChuViewModel()
        {
            ThongTinTruongPage = new Views.GiaoVien.ThongTinTruong();
            LopHocPage = new Views.GiaoVien.LopHoc();
            ThanhTichHocSinhPage = new ThanhTichHocSinh();
            HeThongBangDiemPage = new HeThongBangDiem();
            BaoCaoPage = new Views.GiamHieu.BaoCaoMonHoc();
            BaoCaoHocKyPage = new Views.GiamHieu.BaoCaoTongKetHocKy();
            CurrentUser = new StudentManagement.Model.GiaoVien();
            CurrentUser.MaGiaoVien = 100000;
            //define ICommand
            LoadWindow = new RelayCommand<GiaoVienWindow>((parameter) => { return true; }, (parameter) =>
            {
                GiaoVienWD = parameter;
                LoadThongTinCaNhan();
            });

            SwitchThongTinTruong = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = ThongTinTruongPage;
            });
            SwitchLopHoc = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                StudentManagement.ViewModel.GiaoVien.LopHocViewModel vm = LopHocPage.DataContext as StudentManagement.ViewModel.GiaoVien.LopHocViewModel;
                vm.IdGiaoVien = CurrentUser.MaGiaoVien;
                parameter.Content = LopHocPage;
            });
            SwitchThanhTichHocSinh = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = ThanhTichHocSinhPage;
            });
            SwitchQuanLiBangDiem = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = HeThongBangDiemPage;
            });
            SwitchBaoCaoMonHoc = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = BaoCaoPage;
            });
            SwitchBaoCaoHocKy = new RelayCommand<Frame>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Content = BaoCaoHocKyPage;
            });
            SuaThongTinCaNhan = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {

                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try
                    {
                        SuaGiaoVien window = new SuaGiaoVien();
                        SuaGiaoVienViewModel data = window.DataContext as SuaGiaoVienViewModel;
                        data.GiaoVienHienTai = CurrentUser;
                        window.ShowDialog();
                        LoadThongTinCaNhan();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });

        }
        public void LoadThongTinCaNhan()
        {
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
                    string CmdString = "select TenGiaoVien,NgaySinh,GioiTinh,DiaChi,Email,AnhThe from GiaoVien where MaGiaoVien = " + CurrentUser.MaGiaoVien.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows) reader.Read();
                    CurrentUser.TenGiaoVien = reader.GetString(0);
                    CurrentUser.NgaySinh = reader.GetDateTime(1);
                    CurrentUser.GioiTinh = reader.GetBoolean(2);
                    CurrentUser.DiaChi = reader.GetString(3);
                    CurrentUser.Email = reader.GetString(4);
                    CurrentUser.Avatar = (byte[])reader[5];
                    con.Close();
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
