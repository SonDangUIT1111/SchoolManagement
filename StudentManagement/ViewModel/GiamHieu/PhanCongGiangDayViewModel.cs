using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PhanCongGiangDay = StudentManagement.Views.GiamHieu.PhanCongGiangDay;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class PhanCongGiangDayViewModel : BaseViewModel
    {
        public PhanCongGiangDay PhanCongGiangDayWD { get; set; }
        public string NienKhoaQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }

        private ObservableCollection<StudentManagement.Model.PhanCongGiangDay> _danhSachPhanCong;
        public ObservableCollection<StudentManagement.Model.PhanCongGiangDay> DanhSachPhanCong { get => _danhSachPhanCong; set { _danhSachPhanCong = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _nienKhoaCmb;
        public ObservableCollection<string> NienKhoaCmb { get => _nienKhoaCmb; set { _nienKhoaCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.Khoi> _khoiCmb;
        public ObservableCollection<StudentManagement.Model.Khoi> KhoiCmb { get => _khoiCmb; set { _khoiCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.Lop> _lopCmb;
        public ObservableCollection<StudentManagement.Model.Lop> LopCmb { get => _lopCmb; set { _lopCmb = value; OnPropertyChanged(); } }
        public ICommand LoadData { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterLop { get; set; }
        public ICommand UpdatePhanCong { get; set; }
        public ICommand RemovePhanCong { get; set; }
        public ICommand AddPhanCong { get; set; }
        public ICommand SearchPhanCong { get; set; }

        public PhanCongGiangDayViewModel()
        {
            NienKhoaQueries = "";
            KhoiQueries = "";
            LopQueries = "";
            LoadThongTinCmb();
            LoadThongTinPhanCong();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                PhanCongGiangDayWD = parameter as PhanCongGiangDay;
                PhanCongGiangDayWD.cmbNienKhoa.SelectedIndex = 0;
                PhanCongGiangDayWD.cmbKhoi.SelectedIndex = 0;
                PhanCongGiangDayWD.cmbLop.SelectedIndex = 0;
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
                        if (PhanCongGiangDayWD.cmbLop.SelectedIndex >= 0)
                            LoadThongTinPhanCong();
                        else DanhSachPhanCong.Clear();
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
                        if (PhanCongGiangDayWD.cmbLop.SelectedIndex >= 0)
                            LoadThongTinPhanCong();
                        else DanhSachPhanCong.Clear();
                    }
                }
            });
            AddPhanCong = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemPhanCong window = new ThemPhanCong();
                ThemPhanCongViewModel data = window.DataContext as ThemPhanCongViewModel;
                window.ShowDialog();
                LoadThongTinPhanCong();
            });
            UpdatePhanCong = new RelayCommand<Model.PhanCongGiangDay>((parameter) => { return true; }, (parameter) =>
            {
                SuaPhanCong window = new SuaPhanCong();
                SuaPhanCongViewModel data = window.DataContext as SuaPhanCongViewModel;
                data.PhanCongHienTai = parameter;
                window.ShowDialog();
                LoadThongTinPhanCong();
            });
            RemovePhanCong = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                Model.PhanCongGiangDay item = parameter as Model.PhanCongGiangDay;
                XoaPhanCong(item);
                LoadThongTinPhanCong();
            });
            SearchPhanCong = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                DanhSachPhanCong.Clear();
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try
                    {
                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                        string CmdString = "select MaPhanCong, NienKhoa, TenLop, SiSo, TenMon, TenGiaoVien from PhanCongGiangDay where MaLop = N'" + LopQueries + "' and TenMon like N'%" + parameter.Text + "%'";
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                StudentManagement.Model.PhanCongGiangDay phancong = new StudentManagement.Model.PhanCongGiangDay();
                                phancong.MaPhanCong = reader.GetInt32(0);
                                phancong.NienKhoa = reader.GetString(1);
                                phancong.TenLop = reader.GetString(2);
                                phancong.SiSo = reader.GetInt32(3);
                                phancong.TenMon = reader.GetString(4);
                                phancong.TenGiaoVien = reader.GetString(5);
                                DanhSachPhanCong.Add(phancong);
                            }
                            reader.NextResult();
                        }
                        con.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                    }
                }
            });
        }
        public void LoadThongTinCmb()
        {
            NienKhoaCmb = new ObservableCollection<string>();
            KhoiCmb = new ObservableCollection<StudentManagement.Model.Khoi>();
            LopCmb = new ObservableCollection<StudentManagement.Model.Lop>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
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

                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
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

                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
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
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                }
            }
        }
        public void LoadThongTinPhanCong()
        {
            DanhSachPhanCong = new ObservableCollection<Model.PhanCongGiangDay>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                    string CmdString = "select MaPhanCong, NienKhoa, TenLop, SiSo, TenMon, TenGiaoVien from PhanCongGiangDay where MaLop = N'" + LopQueries + "'";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.PhanCongGiangDay phancong = new StudentManagement.Model.PhanCongGiangDay();
                            phancong.MaPhanCong = reader.GetInt32(0);
                            phancong.NienKhoa = reader.GetString(1);
                            phancong.TenLop = reader.GetString(2);
                            phancong.SiSo = reader.GetInt32(3);
                            phancong.TenMon = reader.GetString(4);
                            phancong.TenGiaoVien = reader.GetString(5);
                            DanhSachPhanCong.Add(phancong);
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                }
            }
        }
        public void FilterLopFromSelection()
        {
            LopCmb.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
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
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra đường truyền");
                }
                if (LopCmb.Count > 0)
                {
                    PhanCongGiangDayWD.cmbLop.SelectedIndex = 0;
                }
                else
                {
                    PhanCongGiangDayWD.cmbLop.SelectedIndex = -1;
                }
            }
        }
        public void XoaPhanCong(Model.PhanCongGiangDay item)
        {
            MessageBoxResult ConfirmDelete = System.Windows.MessageBox.Show("Bạn có chắc chắn xóa phân công?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (ConfirmDelete == MessageBoxResult.Yes)
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try
                    {
                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                        SqlCommand cmd;
                        string CmdString = "Delete from PhanCongGiangDay where MaPhanCong = " + item.MaPhanCong;
                        //MessageBox.Show(CmdString);
                        cmd = new SqlCommand(CmdString, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                    }
                    MessageBox.Show("Đã xóa thành công!");
                }
        }
    }
}
