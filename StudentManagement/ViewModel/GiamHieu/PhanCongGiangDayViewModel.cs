using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.HocSinh;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
        public bool everLoaded { get; set; }

        private bool _dataGridVisibility;

        public bool DataGridVisibility
        {
            get
            {
                return _dataGridVisibility;
            }
            set
            {
                _dataGridVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _progressBarVisibility;

        public bool ProgressBarVisibility
        {
            get
            {
                return _progressBarVisibility;
            }
            set
            {
                _progressBarVisibility = value;
                OnPropertyChanged();
            }
        }


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
        public ICommand MouseEnterComboBox { get; set; }
        public ICommand MouseLeaveComboBox { get; set; }

        public PhanCongGiangDayViewModel()
        {
            everLoaded = false;
            NienKhoaQueries = "";
            KhoiQueries = "";
            LopQueries = "";
            LoadThongTinCmb();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                if (everLoaded == false)
                {
                    PhanCongGiangDayWD = parameter as PhanCongGiangDay;
                    try
                    {
                        PhanCongGiangDayWD.cmbNienKhoa.SelectedIndex = 0;
                        PhanCongGiangDayWD.cmbKhoi.SelectedIndex = 0;
                        PhanCongGiangDayWD.cmbLop.SelectedIndex = 0;
                        
                        ProgressBarVisibility = true;
                        DataGridVisibility = false;
                        await LoadThongTinPhanCong();
                        ProgressBarVisibility = false;
                        DataGridVisibility = true;

                    }
                    catch (Exception)
                    {

                    }
                    everLoaded = true;
                }
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
                        FilterKhoiFromSelection();
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
            FilterLop = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    Lop item = cmb.SelectedItem as Lop;
                    if (item != null)
                    {
                        LopQueries = item.MaLop.ToString();
                        if (PhanCongGiangDayWD.cmbLop.SelectedIndex >= 0)
                        {
                            ProgressBarVisibility = true;
                            DataGridVisibility = false;
                            await LoadThongTinPhanCong();
                            ProgressBarVisibility = false;
                            DataGridVisibility = true;
                        }
                        else DanhSachPhanCong.Clear();
                    }
                }
            });
            AddPhanCong = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ThemPhanCong window = new ThemPhanCong();
                ThemPhanCongViewModel data = window.DataContext as ThemPhanCongViewModel;
                window.ShowDialog();
                ProgressBarVisibility = true;
                DataGridVisibility = false;
                await LoadThongTinPhanCong();
                ProgressBarVisibility = false;
                DataGridVisibility = true;
            });
            UpdatePhanCong = new RelayCommand<Model.PhanCongGiangDay>((parameter) => { return true; }, async (parameter) =>
            {
                SuaPhanCong window = new SuaPhanCong();
                SuaPhanCongViewModel data = window.DataContext as SuaPhanCongViewModel;
                data.PhanCongHienTai = parameter;
                window.ShowDialog();
                ProgressBarVisibility = true;
                DataGridVisibility = false;
                await LoadThongTinPhanCong();
                ProgressBarVisibility = false;
                DataGridVisibility = true;
            });
            RemovePhanCong = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                Model.PhanCongGiangDay item = parameter as Model.PhanCongGiangDay;
                XoaPhanCong(item);
                ProgressBarVisibility = true;
                DataGridVisibility = false;
                await LoadThongTinPhanCong();
                ProgressBarVisibility = false;
                DataGridVisibility = true;
            });
            SearchPhanCong = new RelayCommand<TextBox>((parameter) => { return true; }, async (parameter) =>
            {
                DanhSachPhanCong.Clear();
                ProgressBarVisibility = true;
                DataGridVisibility = false;
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
                        string CmdString = "select MaPhanCong, NienKhoa, TenLop, SiSo, TenMon, TenGiaoVien from PhanCongGiangDay pc join Lop l on pc.MaLop = l.MaLop " +
                                            " join MonHoc mh on mh.MaMon = pc.MaMon join GiaoVien gv on gv.MaGiaoVien = pc.MaGiaoVienPhuTrach " +
                                            " where pc.MaLop = " + LopQueries + " and TenMon like N'%" + parameter.Text + "%'";
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
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    ProgressBarVisibility = false;
                    DataGridVisibility = true;
                }
            });
            MouseEnterComboBox = new RelayCommand<ComboBox>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Focus();
            });
            MouseLeaveComboBox = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                PhanCongGiangDayWD.btnTrick.Focus();
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
                    try 
                    { 
                        con.Open(); 
                    } catch (Exception) 
                    { 
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                        return; 
                    }
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

                    try 
                    { 
                        con.Open(); 
                    } catch (Exception) 
                    { 
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                        return; 
                    }
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

                    try 
                    { 
                        con.Open(); 
                    } catch (Exception) 
                    { 
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                        return; 
                    }
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public async Task LoadThongTinPhanCong()
        {
            DanhSachPhanCong = new ObservableCollection<Model.PhanCongGiangDay>();

            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        await con.OpenAsync();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return;
                    }

                    string CmdString = "select MaPhanCong, NienKhoa, TenLop, SiSo, TenMon, TenGiaoVien from PhanCongGiangDay " +
                                       " pc join Lop l on pc.MaLop = l.MaLop join GiaoVien gv on gv.MaGiaoVien = pc.MaGiaoVienPhuTrach join MonHoc mh on mh.MaMon = pc.MaMon " +
                                       " where l.MaLop = " + LopQueries + "";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
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

                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
                    try 
                    { 
                        con.Open(); 
                    } catch (Exception) 
                    { 
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                        return; 
                    }
                    string CmdString = "select Malop, TenLop from Lop where MaKhoi = " + KhoiQueries + " and NienKhoa = '" + NienKhoaQueries + "'";
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
        public void FilterKhoiFromSelection()
        {
            KhoiCmb.Clear();
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
                    string CmdString = "select MaKhoi, Khoi from Khoi ";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
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
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (KhoiCmb.Count > 0)
                {
                    PhanCongGiangDayWD.cmbKhoi.SelectedIndex = 0;
                }
                else
                {
                    PhanCongGiangDayWD.cmbKhoi.SelectedIndex = -1;
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
                        try 
                        { 
                            con.Open();
                        } catch (Exception)
                        { 
                            MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                            return; 
                        }
                        SqlCommand cmd;
                        string CmdString = "Delete from PhanCongGiangDay where MaPhanCong = " + item.MaPhanCong;
                        cmd = new SqlCommand(CmdString, con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đã xóa thành công!");
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
