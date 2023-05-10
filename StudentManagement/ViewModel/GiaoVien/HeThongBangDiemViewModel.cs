using StudentManagement.Model;
using StudentManagement.Views.GiaoVien;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiaoVien
{
    public class HeThongBangDiemViewModel : BaseViewModel
    {

        // khai báo biến
        private int _idUser;
        public int IdUser { get { return _idUser; } set { _idUser = value; } }
        private bool _justReadOnly;
        public bool JustReadOnly { get { return _justReadOnly; } set { _justReadOnly = value; OnPropertyChanged(); } }
        private bool _canUserEdit;
        public bool CanUserEdit { get { return _canUserEdit; } set { _canUserEdit = value; OnPropertyChanged(); } }
        public string NienKhoaQueries { get; set; }
        public int HocKyQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }
        public string MonHocQueries { get; set; }
        public HeThongBangDiem HeThongBangDiemWD { get; set; }
        private ObservableCollection<StudentManagement.Model.HeThongDiem> _danhSachDiem;
        public ObservableCollection<StudentManagement.Model.HeThongDiem> DanhSachDiem { get => _danhSachDiem; set { _danhSachDiem = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _nienKhoaCmb;
        public ObservableCollection<string> NienKhoaCmb { get => _nienKhoaCmb; set { _nienKhoaCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<Lop> _lopDataCmb;
        public ObservableCollection<Lop> LopDataCmb { get => _lopDataCmb; set { _lopDataCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<Khoi> _khoiDataCmb;
        public ObservableCollection<Khoi> KhoiDataCmb { get => _khoiDataCmb; set { _khoiDataCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<MonHoc> _monDataCmb;
        public ObservableCollection<MonHoc> MonDataCmb { get => _monDataCmb; set { _monDataCmb = value; OnPropertyChanged(); } }

        // khai báo ICommand
        public ICommand LoadWindow { get; set; }
        public ICommand MouseEnterComboBox { get; set; }
        public ICommand MouseLeaveComboBox { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterHocKy { get; set; }
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterLop { get; set; }
        public ICommand FilterMonHoc { get; set; }
        public ICommand LuuDiem { get; set; }
        public HeThongBangDiemViewModel()
        {
            IdUser = 100000;
            JustReadOnly = true;
            CanUserEdit = false;
            DanhSachDiem = new ObservableCollection<HeThongDiem>();
            LoadWindow = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                HeThongBangDiemWD = parameter as HeThongBangDiem;
                LoadDuLieuComboBox();
                XacDinhQuyenHan();
                LoadDanhSachBangDiem();
            });
            MouseEnterComboBox = new RelayCommand<ComboBox>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Focus();
            });
            MouseLeaveComboBox = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                HeThongBangDiemWD.btnTrick.Focus();
            });
            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    FilterLopFromSelection();
                }
            });
            FilterHocKy = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    if (cmb.SelectedItem.ToString().Contains("1"))
                        HocKyQueries = 1;
                    else
                        HocKyQueries = 2;
                    XacDinhQuyenHan();
                    LoadDanhSachBangDiem();
                }
            });
            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    Khoi item = cmb.SelectedItem as Khoi;
                    KhoiQueries = item.MaKhoi.ToString();
                    FilterLopFromSelection();
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
                        XacDinhQuyenHan();
                        LoadDanhSachBangDiem();
                    }
                }
            });
            FilterMonHoc = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    MonHoc item = cmb.SelectedItem as MonHoc;
                    MonHocQueries = item.MaMon.ToString();
                    XacDinhQuyenHan();
                    LoadDanhSachBangDiem();
                }
            });
            LuuDiem = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn lưu. (Lưu ý, những học sinh có điểm ở trạng thái chốt điểm sẽ không được lưu)", "Thông báo", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (DanhSachDiem[0].TrangThai == true)
                    {
                        MessageBox.Show("Danh sách điểm này đã được chốt, không thể sửa.");
                        LoadDanhSachBangDiem();
                        return;
                    }
                    if (KiemTraDiemHopLe() == false)
                    {
                        MessageBox.Show("Điểm nhập không hợp lệ, vui lòng kiểm tra lại.");
                        return;
                    }
                    LuuBangDiem();
                }
            });
        }
        public void LoadDuLieuComboBox()
        {
            NienKhoaQueries = KhoiQueries = LopQueries = MonHocQueries = "";
            HocKyQueries = 1;
            HeThongBangDiemWD.cmbHocKy.Items.Add("Học kỳ 1");
            HeThongBangDiemWD.cmbHocKy.Items.Add("Học kỳ 2");
            HeThongBangDiemWD.cmbHocKy.SelectedIndex = 0;
            NienKhoaCmb = new ObservableCollection<string>();
            MonDataCmb = new ObservableCollection<MonHoc>();
            LopDataCmb = new ObservableCollection<Lop>();
            KhoiDataCmb = new ObservableCollection<Khoi>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
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
                            HeThongBangDiemWD.cmbNienKhoa.SelectedIndex = 0;
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
                        KhoiDataCmb.Add(item);
                        if (String.IsNullOrEmpty(KhoiQueries))
                        {
                            KhoiQueries = reader.GetInt32(0).ToString();
                            HeThongBangDiemWD.cmbKhoi.SelectedIndex = 0;
                        }
                    }
                    reader.NextResult();
                }
                con.Close();

                if (!String.IsNullOrEmpty(NienKhoaQueries))
                {
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
                            LopDataCmb.Add(item);
                            if (String.IsNullOrEmpty(LopQueries))
                            {
                                LopQueries = reader.GetInt32(0).ToString();
                                HeThongBangDiemWD.cmbLop.SelectedIndex = 0;
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }

                try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                CmdString = "select MaMon,TenMon from MonHoc";
                cmd = new SqlCommand(CmdString, con);
                reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MonHoc item = new MonHoc();
                        item.MaMon = reader.GetInt32(0);
                        item.TenMon = reader.GetString(1);
                        MonDataCmb.Add(item);
                        if (String.IsNullOrEmpty(MonHocQueries))
                        {
                            MonHocQueries = reader.GetInt32(0).ToString();
                            HeThongBangDiemWD.cmbMonHoc.SelectedIndex = 0;
                        }
                    }
                    reader.NextResult();
                }
                con.Close();
            }

        }

        public void LoadDanhSachBangDiem()
        {
            DanhSachDiem.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                string wherecommand = "";
                if (!String.IsNullOrEmpty(NienKhoaQueries))
                {
                    wherecommand = wherecommand + " where NienKhoa = '" + NienKhoaQueries +
                                    "' and MaLop = " + LopQueries + " and HocKy = " + HocKyQueries.ToString();
                }
                if (!String.IsNullOrEmpty(MonHocQueries))
                {
                    wherecommand = wherecommand + " and MaMon = " + MonHocQueries;
                }
                string CmdString = "select * from HeThongDiem " + wherecommand;
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                int sothuthu = 1;
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.HeThongDiem diem = new StudentManagement.Model.HeThongDiem
                        {
                            MaDiem = reader.GetInt32(0),
                            NienKhoa = reader.GetString(1),
                            HocKy = reader.GetInt32(2),
                            MaLop = reader.GetInt32(3),
                            TenLop = reader.GetString(4),
                            MaMon = reader.GetInt32(5),
                            TenMon = reader.GetString(6),
                            MaHocSinh = reader.GetInt32(7),
                            TenHocSinh = reader.GetString(8),
                            Diem15Phut = (decimal)reader.GetDecimal(9),
                            Diem1Tiet = (decimal)reader.GetDecimal(10),
                            DiemTB = (decimal)reader.GetDecimal(11),
                            XepLoai = reader.GetBoolean(12),
                            TrangThai = reader.GetBoolean(13),
                        };
                        DanhSachDiem.Add(diem);
                        sothuthu++;
                    }
                    reader.NextResult();
                }
                con.Close();
            }
            HeThongBangDiemWD.cmbLop.Focus();
            HeThongBangDiemWD.btnTrick.Focus();
        }
        public void FilterLopFromSelection()
        {
            LopDataCmb.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                string CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries + "' and MaKhoi = " + KhoiQueries;
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.Lop item = new StudentManagement.Model.Lop
                        {
                            MaLop = reader.GetInt32(0),
                            TenLop = reader.GetString(1),
                        };
                        LopDataCmb.Add(item);
                    }
                    reader.NextResult();
                }
                con.Close();
                HeThongBangDiemWD.cmbLop.SelectedIndex = 0;
            }
        }
        public void XacDinhQuyenHan()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                string CmdString = "";
                int checkUser = 0;
                try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                CmdString = "Select count(*) from PhanCongGiangDay where MaGiaoVienPhuTrach = " + IdUser.ToString()
                        + " and MaLop = " + LopQueries + " and MaMon = " + MonHocQueries;
                SqlCommand cmd = new SqlCommand(CmdString, con);
                checkUser = Convert.ToInt32(cmd.ExecuteScalar());

                if (checkUser > 0)
                {
                    HeThongBangDiemWD.tbThongBaoChan.Visibility = Visibility.Hidden;
                    HeThongBangDiemWD.tbThongBaoQuyen.Visibility = Visibility.Visible;
                    JustReadOnly = false;
                    CanUserEdit = true;
                }
                else
                {
                    HeThongBangDiemWD.tbThongBaoChan.Visibility = Visibility.Visible;
                    HeThongBangDiemWD.tbThongBaoQuyen.Visibility = Visibility.Hidden;
                    JustReadOnly = true;
                    CanUserEdit = false;
                }
                con.Close();
            }

        }
        public void LuuBangDiem()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                string CmdString = "";
                SqlCommand cmd;
                int madiem = 0;
                decimal diem15phut, diem1tiet, dtb = 0;
                int xeploai = 0;
                try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                for (int i = 0; i < DanhSachDiem.Count; i++)
                {
                    madiem = DanhSachDiem[i].MaDiem;
                    diem15phut = (decimal)DanhSachDiem[i].Diem15Phut;
                    diem1tiet = (decimal)DanhSachDiem[i].Diem1Tiet;
                    dtb = (diem15phut + diem1tiet) / 2;
                    if (dtb >= 5)
                    {
                        xeploai = 1;
                    }
                    else xeploai = 0;
                    CmdString = "update HeThongDiem "
                               + "set Diem15Phut = " + diem15phut.ToString() + ", Diem1Tiet = " + diem1tiet.ToString()
                               + ", DiemTrungBinh = " + dtb.ToString() + " ,XepLoai = " + xeploai.ToString()
                               + " where MaDiem = " + madiem.ToString();
                    cmd = new SqlCommand(CmdString, con);
                    try
                    {
                        cmd.ExecuteScalar();
                    }
                    catch (Exception)
                    {
                    }
                }
                MessageBox.Show("Lưu thành công");
                LoadDanhSachBangDiem();
                con.Close();

            }

        }
        public bool KiemTraDiemHopLe()
        {
            for (int i = 0; i < DanhSachDiem.Count; i++)
            {
                if (DanhSachDiem[i].Diem15Phut < 0 || DanhSachDiem[i].Diem15Phut > 10)
                    return false;
                if (DanhSachDiem[i].Diem1Tiet < 0 || DanhSachDiem[i].Diem1Tiet > 10)
                    return false;
            }
            return true;
        }
    }
}
