using MaterialDesignThemes.Wpf;
using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
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

namespace StudentManagement.ViewModel.GiamHieu
{
    public class QuanLiDiemSoViewModel:BaseViewModel
    {
        // khai báo biến
        private int _idUser;
        public int IdUser { get { return _idUser; } set { _idUser = value; } }
     
        public string NienKhoaQueries { get; set; }
        public int HocKyQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }
        public string MonHocQueries { get; set; }
        public string NienKhoaQueries2 { get; set; }
        public int HocKyQueries2 { get; set; }
        public string KhoiQueries2 { get; set; }
        public string LopQueries2 { get; set; }
        public QuanLiDiemSo QuanLiDiemSoWD { get; set; }
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
        private ObservableCollection<string> _nienKhoaCmb2;
        public ObservableCollection<string> NienKhoaCmb2 { get => _nienKhoaCmb2; set { _nienKhoaCmb2 = value; OnPropertyChanged(); } }
        private ObservableCollection<Lop> _lopDataCmb2;
        public ObservableCollection<Lop> LopDataCmb2 { get => _lopDataCmb2; set { _lopDataCmb2 = value; OnPropertyChanged(); } }
        private ObservableCollection<Khoi> _khoiDataCmb2;
        public ObservableCollection<Khoi> KhoiDataCmb2 { get => _khoiDataCmb2; set { _khoiDataCmb2 = value; OnPropertyChanged(); } }

        // khai báo ICommand
        public ICommand LoadWindow { get; set; }
        public ICommand MouseEnterComboBox { get; set; }
        public ICommand MouseLeaveComboBox { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterHocKy { get; set; }
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterLop { get; set; }
        public ICommand FilterMonHoc { get; set; }
        public ICommand FilterNienKhoa2 { get; set; }
        public ICommand FilterHocKy2 { get; set; }
        public ICommand FilterKhoi2 { get; set; }
        public ICommand FilterLop2 { get; set; }
        public ICommand MoKhoaDiem { get; set; }
        public QuanLiDiemSoViewModel()
        {
            IdUser = 100000;
            DanhSachDiem = new ObservableCollection<HeThongDiem>();
            LoadWindow = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                QuanLiDiemSoWD = parameter as QuanLiDiemSo;
                LoadDuLieuComboBox();
                LoadDanhSachBangDiem();
            });
            MouseEnterComboBox = new RelayCommand<ComboBox>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Focus();
            });
            MouseLeaveComboBox = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                QuanLiDiemSoWD.btnTrick.Focus();
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
                    LoadDanhSachBangDiem();
                }
            });
            FilterNienKhoa2 = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    NienKhoaQueries2 = cmb.SelectedItem.ToString();
                    FilterLopFromSelection2();
                }
            });
            FilterHocKy2 = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    if (cmb.SelectedItem.ToString().Contains("1"))
                        HocKyQueries2 = 1;
                    else
                        HocKyQueries2 = 2;
                }
            });
            FilterKhoi2 = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    Khoi item = cmb.SelectedItem as Khoi;
                    KhoiQueries2 = item.MaKhoi.ToString();
                    FilterLopFromSelection2();
                }
            });
            FilterLop2 = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    Lop item = cmb.SelectedItem as Lop;
                    if (item != null)
                    {
                        LopQueries2 = item.MaLop.ToString();
                    }
                }
            });
            MoKhoaDiem = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn mở khóa bảng điểm lớp này.","Thông báo",MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    MoKhoaBangDiem();
                }    
            });

        }
        public void LoadDuLieuComboBox()
        {
            NienKhoaQueries = KhoiQueries = LopQueries = MonHocQueries = "";
            HocKyQueries = 1;
            HocKyQueries2 = 1;
            QuanLiDiemSoWD.cmbHocKy.Items.Add("Học kỳ 1");
            QuanLiDiemSoWD.cmbHocKy.Items.Add("Học kỳ 2");
            QuanLiDiemSoWD.cmbHocKy2.Items.Add("Học kỳ 1");
            QuanLiDiemSoWD.cmbHocKy2.Items.Add("Học kỳ 2");
            QuanLiDiemSoWD.cmbHocKy.SelectedIndex = 0;
            QuanLiDiemSoWD.cmbHocKy2.SelectedIndex = 0;
            NienKhoaCmb = new ObservableCollection<string>();
            MonDataCmb = new ObservableCollection<MonHoc>();
            LopDataCmb = new ObservableCollection<Lop>();
            KhoiDataCmb = new ObservableCollection<Khoi>();
            NienKhoaCmb2 = new ObservableCollection<string>();
            LopDataCmb2 = new ObservableCollection<Lop>();
            KhoiDataCmb2 = new ObservableCollection<Khoi>();
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
                        NienKhoaCmb2.Add(reader.GetString(0));
                        if (String.IsNullOrEmpty(NienKhoaQueries))
                        {
                            NienKhoaQueries = reader.GetString(0);
                            QuanLiDiemSoWD.cmbNienKhoa.SelectedIndex = 0;
                        }
                        if (String.IsNullOrEmpty(NienKhoaQueries2))
                        {
                            NienKhoaQueries2 = reader.GetString(0);
                            QuanLiDiemSoWD.cmbNienKhoa2.SelectedIndex = 0;
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
                        KhoiDataCmb.Add(item);
                        KhoiDataCmb2.Add(item);
                        if (String.IsNullOrEmpty(KhoiQueries))
                        {
                            KhoiQueries = reader.GetInt32(0).ToString();
                            QuanLiDiemSoWD.cmbKhoi.SelectedIndex = 0;
                        }
                        if (String.IsNullOrEmpty(KhoiQueries2))
                        {
                            KhoiQueries2 = reader.GetInt32(0).ToString();
                            QuanLiDiemSoWD.cmbKhoi2.SelectedIndex = 0;
                        }
                    }
                    reader.NextResult();
                }
                con.Close();

                if (!String.IsNullOrEmpty(NienKhoaQueries))
                {
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
                            LopDataCmb.Add(item);
                            if (String.IsNullOrEmpty(LopQueries))
                            {
                                LopQueries = reader.GetInt32(0).ToString();
                                QuanLiDiemSoWD.cmbLop.SelectedIndex = 0;
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                if (!String.IsNullOrEmpty(NienKhoaQueries2))
                {
                    con.Open();
                    CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries2 + "' and MaKhoi = " + KhoiQueries2;
                    cmd = new SqlCommand(CmdString, con);
                    reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Lop item = new Lop();
                            item.MaLop = reader.GetInt32(0);
                            item.TenLop = reader.GetString(1);
                            LopDataCmb2.Add(item);
                            if (String.IsNullOrEmpty(LopQueries2))
                            {
                                LopQueries2 = reader.GetInt32(0).ToString();
                                QuanLiDiemSoWD.cmbLop2.SelectedIndex = 0;
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }

                con.Open();
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
                            QuanLiDiemSoWD.cmbMonHoc.SelectedIndex = 0;
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
                con.Open();
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
                            SoThuTu = sothuthu,
                            MaDiem = reader.GetInt32(0),
                            NienKhoa = reader.GetString(1),
                            HocKy = reader.GetInt32(2),
                            MaLop = reader.GetInt32(3),
                            TenLop = reader.GetString(4),
                            MaMon = reader.GetInt32(5),
                            TenMon = reader.GetString(6),
                            MaHocSinh = reader.GetInt32(7),
                            TenHocSinh = reader.GetString(8),
                            Diem15Phut = (float)reader.GetDecimal(9),
                            Diem1Tiet = (float)reader.GetDecimal(10),
                            DiemTB = (float)reader.GetDecimal(11),
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
            QuanLiDiemSoWD.cmbLop.Focus();
            QuanLiDiemSoWD.btnTrick.Focus();
        }
        public void FilterLopFromSelection()
        {
            LopDataCmb.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
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
                QuanLiDiemSoWD.cmbLop.SelectedIndex = 0;
            }
        }
        public void FilterLopFromSelection2()
        {
            LopDataCmb2.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries2 + "' and MaKhoi = " + KhoiQueries2;
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
                        LopDataCmb2.Add(item);
                    }
                    reader.NextResult();
                }
                con.Close();
                QuanLiDiemSoWD.cmbLop2.SelectedIndex = 0;
            }
        }
        public void MoKhoaBangDiem()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string hocky ="";
                if (HocKyQueries2 == 1)
                {
                    hocky = "1";
                }
                else hocky = "2";
                string CmdString = "Update HeThongDiem "
                                  +"set TrangThai = 0 "
                                  +"where MaLop = "+LopQueries2+" and NienKhoa = '"+NienKhoaQueries2+"' and HocKy = "+hocky;
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.ExecuteScalar();
                MessageBox.Show(CmdString);
                con.Close();
            }
        }
    }
}
