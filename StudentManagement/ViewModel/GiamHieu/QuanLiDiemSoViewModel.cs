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
using System.Threading;
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
        private ObservableCollection<StudentManagement.Model.ThanhTich> _danhSachThanhTich;
        public ObservableCollection<StudentManagement.Model.ThanhTich> DanhSachThanhTich { get => _danhSachThanhTich; set { _danhSachThanhTich = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.BaoCaoMon> _danhSachBaoCaoMon;
        public ObservableCollection<StudentManagement.Model.BaoCaoMon> DanhSachBaoCaoMon { get => _danhSachBaoCaoMon; set { _danhSachBaoCaoMon = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.BaoCaoHocKy> _danhSachBaoCaoHocKy;
        public ObservableCollection<StudentManagement.Model.BaoCaoHocKy> DanhSachBaoCaoHocKy { get => _danhSachBaoCaoHocKy; set { _danhSachBaoCaoHocKy = value; OnPropertyChanged();} }
        private ObservableCollection<string> _nienKhoaCmb;
        public ObservableCollection<string> NienKhoaCmb { get => _nienKhoaCmb; set { _nienKhoaCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<Lop> _lopDataCmb;
        public ObservableCollection<Lop> LopDataCmb { get => _lopDataCmb; set { _lopDataCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<Khoi> _khoiDataCmb;
        public ObservableCollection<Khoi> KhoiDataCmb { get => _khoiDataCmb; set { _khoiDataCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<Model.MonHoc> _monDataCmb;
        public ObservableCollection<Model.MonHoc> MonDataCmb { get => _monDataCmb; set { _monDataCmb = value; OnPropertyChanged(); } }
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
        public ICommand KhoaDiem { get; set; }
        public QuanLiDiemSoViewModel()
        {
            DanhSachDiem = new ObservableCollection<HeThongDiem>();
            DanhSachThanhTich = new ObservableCollection<ThanhTich>();
            DanhSachBaoCaoMon = new ObservableCollection<BaoCaoMon>();
            DanhSachBaoCaoHocKy = new ObservableCollection<BaoCaoHocKy>();
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
                    Model.MonHoc item = cmb.SelectedItem as Model.MonHoc;
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
            KhoaDiem = new RelayCommand<object>((parameter) =>
            // yêu cầu pick toàn bộ combobox mới được khóa
            {
                if (QuanLiDiemSoWD == null) return false;
                if (QuanLiDiemSoWD.cmbHocKy2.SelectedIndex >= 0 && QuanLiDiemSoWD.cmbLop2.SelectedIndex >= 0
                    && QuanLiDiemSoWD.cmbKhoi2.SelectedIndex >= 0 && QuanLiDiemSoWD.cmbNienKhoa2.SelectedIndex >= 0)
                    return true;
                else return false;
            }, (parameter) =>
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn khóa bảng điểm lớp này.", "Thông báo", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    KhoaBangDiem();
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
            MonDataCmb = new ObservableCollection<Model.MonHoc>();
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
                        Model.MonHoc item = new Model.MonHoc();
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
                MessageBox.Show("Mở khóa thành công.");
                con.Close();
            }
        }
        public void KhoaBangDiem()
        {
            DanhSachThanhTich.Clear();
            DanhSachBaoCaoMon.Clear();
            DanhSachBaoCaoHocKy.Clear();


            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                // khóa trạng thái
                string CmdString = "update HeThongDiem set TrangThai = 1 where MaLop = " + LopQueries2 +
                                   " and NienKhoa = '" + NienKhoaQueries2 + "' and HocKy = " + HocKyQueries2.ToString();
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.ExecuteScalar();
                con.Close();

                con.Open();
                // cập nhật database thành tích
                CmdString = "select NienKhoa,HocKy,MaLop,TenLop,MaHocSinh,TenHocSinh,avg(DiemTrungBinh) TBHK "+
                            "from HeThongDiem " +
                            "where MaLop = "+LopQueries2+" and NienKhoa = '"+NienKhoaQueries2+"' and HocKy = "+HocKyQueries2.ToString()+
                            " group by NienKhoa,HocKy,MaLop,TenLop,MaHocSinh,TenHocSinh";
                cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.ThanhTich thanhtich = new StudentManagement.Model.ThanhTich
                        {
                            NienKhoa = reader.GetString(0),
                            HocKy = reader.GetInt32(1),
                            MaLop = reader.GetInt32(2),
                            TenLop = reader.GetString(3),
                            MaHocSinh = reader.GetInt32(4),
                            TenHocSinh = reader.GetString(5),
                            TBHK = (float)reader.GetDecimal(6),
                        };
                        DanhSachThanhTich.Add(thanhtich);
                    }
                    reader.NextResult();
                }
                con.Close();

                con.Open();
                for (int i = 0; i < DanhSachThanhTich.Count; i++)
                {
                    // chia 2 trường hợp cập nhật lại hoặc thêm mới
                    int xeploai = 0;
                    if (DanhSachThanhTich[i].TBHK >= 5)
                        xeploai = 1;
                    else xeploai = 0;
                    string hocky = "";
                    if (DanhSachThanhTich[i].HocKy == 1)
                        hocky = "TBHK1";
                    else hocky = "TBHK2";
                    CmdString = "if (exists(select * from ThanhTich " +
                                            "where MaHocSinh = "+ DanhSachThanhTich[i].MaHocSinh.ToString()+" and HocKy = "+DanhSachThanhTich[i].HocKy.ToString()+" and NienKhoa = '"+DanhSachThanhTich[i].NienKhoa+"')) " +
                                "begin " +
                                "update ThanhTich " +
                                "set TrungBinhHocKy = "+DanhSachThanhTich[i].TBHK.ToString()+", XepLoai = "+xeploai.ToString() +" " +
                                "where MaHocSinh = " + DanhSachThanhTich[i].MaHocSinh.ToString() + " and HocKy = " + DanhSachThanhTich[i].HocKy.ToString() + " and NienKhoa = '" + DanhSachThanhTich[i].NienKhoa + "' " +
                                "end " +
                                "else " +
                                "begin " +
                                "insert into ThanhTich(NienKhoa,HocKy,MaLop,TenLop,MaHocSinh,TenHocSinh,TrungBinhHocKy,XepLoai) values('" +
                                DanhSachThanhTich[i].NienKhoa+"',"+DanhSachThanhTich[i].HocKy.ToString()+","+DanhSachThanhTich[i].MaLop.ToString()+",'"+DanhSachThanhTich[i].TenLop+"'," +
                                DanhSachThanhTich[i].MaHocSinh.ToString()+",N'"+DanhSachThanhTich[i].TenHocSinh+"',"+DanhSachThanhTich[i].TBHK.ToString()+","+xeploai.ToString()+") " +
                                "end " +
                                "Update HocSinh " +
                                "set "+hocky+" = "+DanhSachThanhTich[i].TBHK.ToString()+" " +
                                "where MaHocSinh ="+DanhSachThanhTich[i].MaHocSinh.ToString();
                    cmd = new SqlCommand(CmdString, con);
                    cmd.ExecuteScalar();
                }
                con.Close();

                // cập nhật database báo cáo môn học
                con.Open();
                CmdString = "select MaLop,TenLop,MaMon,TenMon,HocKy,NienKhoa,count(MaDiem) SLD,SiSo = (select SiSo from Lop l where l.MaLop = htd.MaLop) " +
                            "from HeThongDiem htd " +
                            "where XepLoai = 1 and MaLop = "+LopQueries2+" and HocKy = "+HocKyQueries2.ToString()+" and NienKhoa = '"+NienKhoaQueries2+"' " +
                            "group by MaLop,TenLop,MaMon,TenMon,HocKy,NienKhoa";
                cmd = new SqlCommand(CmdString, con);
                reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.BaoCaoMon baocaomon = new StudentManagement.Model.BaoCaoMon
                        {
                            MaLop = reader.GetInt32(0),
                            TenLop = reader.GetString(1),
                            MaMon = reader.GetInt32(2),
                            TenMon = reader.GetString(3),
                            HocKy = reader.GetInt32(4),
                            NienKhoa = reader.GetString(5),
                            SoLuongDat = reader.GetInt32(6),
                            TiLe = ((double)reader.GetInt32(6)/reader.GetInt32(7)*100).ToString()+ "%",
                        };
                        DanhSachBaoCaoMon.Add(baocaomon);
                    }
                    reader.NextResult();
                }
                con.Close();

                con.Open();
                for (int i = 0; i < DanhSachBaoCaoMon.Count; i++)
                {
                    // chia 2 trường hợp cập nhật lại hoặc thêm mới
                    CmdString = "if (exists(select * from BaoCaoMon " +
                                            "where MaLop = " + DanhSachBaoCaoMon[i].MaLop.ToString() + " and HocKy = " + DanhSachBaoCaoMon[i].HocKy.ToString() + " and NienKhoa = '" + DanhSachBaoCaoMon[i].NienKhoa + "' and MaMon = "+DanhSachBaoCaoMon[i].MaMon.ToString()+")) " +
                                "begin " +
                                "update BaoCaoMon " +
                                "set SoLuongDat = " + DanhSachBaoCaoMon[i].SoLuongDat.ToString() + ", TiLe = '" + DanhSachBaoCaoMon[i].TiLe+ "' " +
                                "where MaLop = " + DanhSachBaoCaoMon[i].MaLop.ToString() + " and HocKy = " + DanhSachBaoCaoMon[i].HocKy.ToString() + " and NienKhoa = '" + DanhSachBaoCaoMon[i].NienKhoa + "' " +
                                "end " +
                                "else " +
                                "begin " +
                                "insert into BaoCaoMon(MaLop,TenLop,MaMon,TenMon,HocKy,NienKhoa,SoLuongDat,TiLe) values("+DanhSachBaoCaoMon[i].MaLop.ToString()+",N'"+DanhSachBaoCaoMon[i].TenLop+
                                "',"+DanhSachBaoCaoMon[i].MaMon.ToString()+",N'"+DanhSachBaoCaoMon[i].TenMon+"',"+DanhSachBaoCaoMon[i].HocKy.ToString()+",'"+DanhSachBaoCaoMon[i].NienKhoa+"',"+
                                DanhSachBaoCaoMon[i].SoLuongDat.ToString()+",'"+DanhSachBaoCaoMon[i].TiLe+"') "+
                                "end";
                    cmd = new SqlCommand(CmdString, con);
                    cmd.ExecuteScalar();
                }
                con.Close();

                // cập nhật database báo cáo tổng kết học kỳ
                con.Open();
                CmdString = "select MaLop, TenLop, SiSo = (select SiSo from Lop l where l.MaLop = tt.MaLop), HocKy, NienKhoa,count(MaThanhTich) SLD " +
                            "from ThanhTich tt " +
                            "where XepLoai = 1 and MaLop = "+LopQueries2+" and HocKy = "+HocKyQueries2.ToString()+" and NienKhoa ='"+NienKhoaQueries2+"' " +
                            "group by MaLop,TenLop,HocKy,NienKhoa";
                cmd = new SqlCommand(CmdString, con);
                reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.BaoCaoHocKy baocaohocky = new StudentManagement.Model.BaoCaoHocKy
                        {
                            MaLop = reader.GetInt32(0),
                            TenLop = reader.GetString(1),
                            SiSo = reader.GetInt32(2),
                            HocKy = reader.GetInt32(3),
                            NienKhoa = reader.GetString(4),
                            SoLuongDat = reader.GetInt32(5),
                            TiLe = ((double)reader.GetInt32(5) / reader.GetInt32(2) * 100).ToString() + "%",
                        };
                        DanhSachBaoCaoHocKy.Add(baocaohocky);
                    }
                    reader.NextResult();
                }
                con.Close();

                con.Open();
                for (int i = 0; i < DanhSachBaoCaoHocKy.Count; i++)
                {
                    // chia 2 trường hợp cập nhật lại hoặc thêm mới
                    CmdString = "if (exists(select * from BaoCaoHocKy " +
                                            "where MaLop = " + DanhSachBaoCaoHocKy[i].MaLop.ToString() + " and HocKy = " + DanhSachBaoCaoHocKy[i].HocKy.ToString() + " and NienKhoa = '" + DanhSachBaoCaoHocKy[i].NienKhoa + "')) " +
                                "begin " +
                                "update BaoCaoHocKy " +
                                "set SoLuongDat = " + DanhSachBaoCaoHocKy[i].SoLuongDat.ToString() + ", TiLe = '" + DanhSachBaoCaoHocKy[i].TiLe + "' " +
                                "where MaLop = " + DanhSachBaoCaoHocKy[i].MaLop.ToString() + " and HocKy = " + DanhSachBaoCaoHocKy[i].HocKy.ToString() + " and NienKhoa = '" + DanhSachBaoCaoHocKy[i].NienKhoa + "' " +
                                "end " +
                                "else " +
                                "begin " +
                                "insert into BaoCaoHocKy(MaLop,TenLop,SiSo,HocKy,NienKhoa,SoLuongDat,TiLe) values(" + DanhSachBaoCaoHocKy[i].MaLop.ToString() + ",N'" + DanhSachBaoCaoHocKy[i].TenLop +
                                "'," + DanhSachBaoCaoHocKy[i].SiSo.ToString() + "," + DanhSachBaoCaoHocKy[i].HocKy.ToString() + ",'" + DanhSachBaoCaoHocKy[i].NienKhoa + "'," +
                                DanhSachBaoCaoHocKy[i].SoLuongDat.ToString() + ",'" + DanhSachBaoCaoHocKy[i].TiLe + "') " +
                                "end";
                    cmd = new SqlCommand(CmdString, con);
                    cmd.ExecuteScalar();
                }
                con.Close();
                MessageBox.Show("Khóa điểm thành công, các báo cáo tổng kết đã được cập nhật.");
            }
        }
    }
}
