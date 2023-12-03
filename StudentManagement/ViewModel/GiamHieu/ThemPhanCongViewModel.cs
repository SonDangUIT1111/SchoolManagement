using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class ThemPhanCongViewModel : BaseViewModel
    {
        public ThemPhanCong ThemPhanCongWD { get; set; }
        public string NienKhoaQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }

        private ObservableCollection<string> _nienKhoaCmb;
        public ObservableCollection<string> NienKhoaCmb { get => _nienKhoaCmb; set { _nienKhoaCmb = value;  } }
        private ObservableCollection<StudentManagement.Model.Khoi> _khoiCmb;
        public ObservableCollection<StudentManagement.Model.Khoi> KhoiCmb { get => _khoiCmb; set { _khoiCmb = value;  } }
        private ObservableCollection<StudentManagement.Model.Lop> _lopCmb;
        public ObservableCollection<StudentManagement.Model.Lop> LopCmb { get => _lopCmb; set { _lopCmb = value;  } }
        private ObservableCollection<StudentManagement.Model.MonHoc> _monHocCmb;
        public ObservableCollection<StudentManagement.Model.MonHoc> MonHocCmb { get => _monHocCmb; set { _monHocCmb = value;  } }
        private ObservableCollection<StudentManagement.Model.GiaoVien> _giaoVienCmb;
        public ObservableCollection<StudentManagement.Model.GiaoVien> GiaoVienCmb { get => _giaoVienCmb; set { _giaoVienCmb = value;  } }
        public ICommand LoadData { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterLop { get; set; }
        public ICommand AddPhanCong { get; set; }
        public ICommand HuyThemPC { get; set; }

        public void LoadThongTinCmb()
        {
            //NienKhoaQueries = null;
            //KhoiQueries = null;
            //LopQueries = null;
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open();
                    string CmdString = "select distinct NienKhoa from Lop";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
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
                    sqlConnectionWrapper.Close();

                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }

                try
                {
                    sqlConnectionWrapper.Open();
                    string CmdString = "select distinct MaKhoi,Khoi from Khoi";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

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
                    sqlConnectionWrapper.Close();

                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }


                try
                {
                    sqlConnectionWrapper.Open();
                    string CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries + "' and MaKhoi = " + KhoiQueries;

                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

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
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }
        public void LoadOptionFromSelection()
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open();
                    string CmdString = "select * from MonHoc where MaMon not in (select MaMon from PhanCongGiangDay where MaLop=" + LopQueries + ")";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
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
                    sqlConnectionWrapper.Close();
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }

                try
                {
                    sqlConnectionWrapper.Open();
                    string CmdString = "select MaGiaoVien, TenGiaoVien from GiaoVien";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
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
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }

            }

        }
        public void FilterLopFromSelection()
        {
            using(var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open(); 
                    string CmdString = "select Malop, TenLop, SiSo from Lop where MaKhoi = " + KhoiQueries + " and NienKhoa = '" + NienKhoaQueries + "'";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
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
                }
                catch (Exception)
                {

                }

            }
        }
        public void FilterKhoiFromSelection()
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open(); 
                    string CmdString = "select MaKhoi, Khoi from Khoi";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Khoi item = new Khoi();
                            item.MaKhoi = reader.GetInt32(0);
                            item.TenKhoi = reader.GetString(1);
                            KhoiCmb.Add(item);
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
        public ThemPhanCongViewModel()
        {
            // Stryker disable all
            MonHocCmb = new ObservableCollection<StudentManagement.Model.MonHoc>();
            GiaoVienCmb = new ObservableCollection<StudentManagement.Model.GiaoVien>();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemPhanCongWD = parameter as ThemPhanCong;
                NienKhoaCmb = new ObservableCollection<string>();
                KhoiCmb = new ObservableCollection<StudentManagement.Model.Khoi>();
                LopCmb = new ObservableCollection<StudentManagement.Model.Lop>();
                LopCmb.Clear();
                LoadThongTinCmb();
                FilterLopFromSelection();
                if (LopCmb.Count > 0)
                {
                    ThemPhanCongWD.cmbLop.SelectedIndex = 0;
                }
                else
                {
                    ThemPhanCongWD.cmbLop.SelectedIndex = -1;
                }
                MonHocCmb.Clear();
                GiaoVienCmb.Clear();
                LoadOptionFromSelection();
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
                        KhoiCmb.Clear();
                        FilterKhoiFromSelection();
                        if (KhoiCmb.Count > 0)
                        {
                            ThemPhanCongWD.cmbKhoi.SelectedIndex = 0;
                        }
                        else
                        {
                            ThemPhanCongWD.cmbKhoi.SelectedIndex = -1;
                        }
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
                        LopCmb.Clear();
                        FilterLopFromSelection();
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
                        {
                            MonHocCmb.Clear();
                            GiaoVienCmb.Clear();
                            LoadOptionFromSelection();
                        }
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
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng chọn môn học";
                    MB.ShowDialog();
                }
                else if (giaovien == null)
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng chọn giáo viên giảng dạy";
                    MB.ShowDialog();
                }
                else
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
                                MessageBoxFail messageBoxFail = new MessageBoxFail();
                                messageBoxFail.ShowDialog();
                                return;
                            }

                            string cmdText = "Select * from PhanCongGiangDay where MaLop = " + lop.MaLop.ToString() + " and MaMon = " + monhoc.MaMon.ToString() + " ";
                            SqlCommand cmdTest = new SqlCommand(cmdText, con);
                            int checkExists = Convert.ToInt32(cmdTest.ExecuteScalar());
                            if (checkExists > 0)
                            {
                                MessageBoxOK messageBoxOK = new MessageBoxOK();
                                MessageBoxOKViewModel data = messageBoxOK.DataContext as MessageBoxOKViewModel;
                                data.Content = "Đã tồn tại phân công này, vui lòng xem xét lại";
                                messageBoxOK.ShowDialog();
                                return;
                            }


                            string CmdString = "insert into PhanCongGiangDay(MaLop,MaMon,MaGiaoVienPhuTrach) values (" + lop.MaLop.ToString() + "," + monhoc.MaMon.ToString() + "," + giaovien.MaGiaoVien + ")";
                            SqlCommand cmd = new SqlCommand(CmdString, con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBoxSuccessful messaeboxBoxSuccessful = new MessageBoxSuccessful();
                            messaeboxBoxSuccessful.ShowDialog();
                            ThemPhanCongWD.Close();
                        }
                        catch (Exception)
                        {
                            MessageBoxFail messageBoxFail = new MessageBoxFail();
                            messageBoxFail.ShowDialog();
                            return;
                        }

                    }
                }
            });
            HuyThemPC = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemPhanCongWD.Close();
            });
        }
    }
}
