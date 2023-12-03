using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
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
    public class QuanLiDiemSoViewModel : BaseViewModel
    {
        // khai báo biến
        const string mot = "1";
        const string hai = "2";
        private int _idUser;
        public int IdUser { get { return _idUser; } set { _idUser = value; } }
        public bool everLoaded { get; set; }

        public string NienKhoaQueries { get; set; }
        public int HocKyQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }
        public string MonHocQueries { get; set; }
        public string NienKhoaQueries2 { get; set; }
        public int HocKyQueries2 { get; set; }
        public string KhoiQueries2 { get; set; }
        public string LopQueries2 { get; set; }


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
                
            }
        }

        public QuanLiDiemSo QuanLiDiemSoWD { get; set; }
        private ObservableCollection<StudentManagement.Model.HeThongDiem> _danhSachDiem;
        public ObservableCollection<StudentManagement.Model.HeThongDiem> DanhSachDiem { get => _danhSachDiem; set { _danhSachDiem = value;  } }
        private ObservableCollection<StudentManagement.Model.ThanhTich> _danhSachThanhTich;
        public ObservableCollection<StudentManagement.Model.ThanhTich> DanhSachThanhTich { get => _danhSachThanhTich; set { _danhSachThanhTich = value; } }
        private ObservableCollection<StudentManagement.Model.BaoCaoMon> _danhSachBaoCaoMon;
        public ObservableCollection<StudentManagement.Model.BaoCaoMon> DanhSachBaoCaoMon { get => _danhSachBaoCaoMon; set { _danhSachBaoCaoMon = value;  } }
        private ObservableCollection<StudentManagement.Model.BaoCaoHocKy> _danhSachBaoCaoHocKy;
        public ObservableCollection<StudentManagement.Model.BaoCaoHocKy> DanhSachBaoCaoHocKy { get => _danhSachBaoCaoHocKy; set { _danhSachBaoCaoHocKy = value;  } }
        private ObservableCollection<string> _nienKhoaCmb;
        public ObservableCollection<string> NienKhoaCmb { get => _nienKhoaCmb; set { _nienKhoaCmb = value;  } }
        private ObservableCollection<Lop> _lopDataCmb;
        public ObservableCollection<Lop> LopDataCmb { get => _lopDataCmb; set { _lopDataCmb = value;  } }
        private ObservableCollection<Khoi> _khoiDataCmb;
        public ObservableCollection<Khoi> KhoiDataCmb { get => _khoiDataCmb; set { _khoiDataCmb = value;  } }
        private ObservableCollection<Model.MonHoc> _monDataCmb;
        public ObservableCollection<Model.MonHoc> MonDataCmb { get => _monDataCmb; set { _monDataCmb = value;  } }
        private ObservableCollection<string> _nienKhoaCmb2;
        public ObservableCollection<string> NienKhoaCmb2 { get => _nienKhoaCmb2; set { _nienKhoaCmb2 = value;  } }
        private ObservableCollection<Lop> _lopDataCmb2;
        public ObservableCollection<Lop> LopDataCmb2 { get => _lopDataCmb2; set { _lopDataCmb2 = value;  } }
        private ObservableCollection<Khoi> _khoiDataCmb2;
        public ObservableCollection<Khoi> KhoiDataCmb2 { get => _khoiDataCmb2; set { _khoiDataCmb2 = value;  } }

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

        public async Task LoadDanhSachBangDiem()
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open();
                    string wherecommand = "";
                    if (!String.IsNullOrEmpty(NienKhoaQueries))
                    {
                        wherecommand = wherecommand + " where ht.MaLop = " + LopQueries + " and ht.HocKy = " + HocKyQueries.ToString();
                    }
                    if (!String.IsNullOrEmpty(MonHocQueries))
                    {
                        wherecommand = wherecommand + " and ht.MaMon = " + MonHocQueries;
                    }

                    string CmdString = "select ht.MaHocSinh,TenHocSinh,Diem15Phut,Diem1Tiet,DiemTrungBinh,XepLoai,TrangThai from HeThongDiem ht join HocSinh hs on ht.MaHocSinh = hs.MaHocSinh " + wherecommand;
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            StudentManagement.Model.HeThongDiem diem = new StudentManagement.Model.HeThongDiem
                            {
                                MaHocSinh = reader.GetInt32(0),
                                TenHocSinh = reader.GetString(1),
                                TrangThai = reader.GetBoolean(6),
                            };
                            try
                            {
                                diem.Diem15Phut = (decimal)reader.GetDecimal(2);
                                diem.Diem1Tiet = (decimal)reader.GetDecimal(3);
                                diem.DiemTB = (decimal)reader.GetDecimal(4);
                                diem.XepLoai = reader.GetBoolean(5);
                            }
                            catch (Exception)
                            {
                                // Handle the exception if needed
                            }
                            DanhSachDiem.Add(diem);
                        }
                        await reader.NextResultAsync();
                    }
                }
                catch (Exception)
                {
                    // Handle the exception if needed
                }
            }

        }

        public void LoadDanhSachMon()
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                sqlConnectionWrapper.Open();
                string CmdString = "select MaMon,TenMon from MonHoc where ApDung = 1";
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                SqlDataReader reader3 = cmd.ExecuteReader();

                while (reader3.HasRows)
                {
                    while (reader3.Read())
                    {
                        Model.MonHoc item = new Model.MonHoc();
                        item.MaMon = reader3.GetInt32(0);
                        item.TenMon = reader3.GetString(1);
                        MonDataCmb.Add(item);
                        if (String.IsNullOrEmpty(MonHocQueries))
                        {
                            MonHocQueries = reader3.GetInt32(0).ToString();
                            //try
                            //{
                            //    QuanLiDiemSoWD.cmbMonHoc.SelectedIndex = 0;
                            //}
                            //catch (Exception)
                            //{

                            //}
                        }
                    }
                    reader3.NextResult();
                }
            }
        }


        public void LoadDanhSachNienKhoa()
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
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
                        NienKhoaCmb2.Add(reader.GetString(0));
                        if (String.IsNullOrEmpty(NienKhoaQueries))
                        {
                            NienKhoaQueries = reader.GetString(0);
                            //try
                            //{
                            //    QuanLiDiemSoWD.cmbNienKhoa.SelectedIndex = 0;
                            //}
                            //catch(Exception)
                            //{ }
                        }
                        if (String.IsNullOrEmpty(NienKhoaQueries2))
                        {
                            NienKhoaQueries2 = reader.GetString(0);
                            //try
                            //{
                            //    QuanLiDiemSoWD.cmbNienKhoa2.SelectedIndex = 0;
                            //} catch (Exception) { }
                        }
                    }
                    reader.NextResult();
                }

            }
        }
        public void LoadDanhSachKhoi()
        {
            using (var sqlConnectionWrap  = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                sqlConnectionWrap.Open();
                string CmdString = "select distinct MaKhoi,Khoi from Khoi";
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                SqlDataReader reader = cmd.ExecuteReader();

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
                            //try
                            //{
                            //    QuanLiDiemSoWD.cmbKhoi.SelectedIndex = 0;
                            //}catch (Exception) { }
                        }
                        if (String.IsNullOrEmpty(KhoiQueries2))
                        {
                            KhoiQueries2 = reader.GetInt32(0).ToString();
                            //try
                            //{
                            //    QuanLiDiemSoWD.cmbKhoi2.SelectedIndex = 0;
                            //}catch (Exception) { }
                        }
                    }
                    reader.NextResult();
                }
            }
        }

        public void LoadDanhSachLop1()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                sqlConnectionWrap.Open();
                string CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries + "' and MaKhoi = " + KhoiQueries;
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                SqlDataReader reader1 = cmd.ExecuteReader();

                while (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        Lop item = new Lop();
                        item.MaLop = reader1.GetInt32(0);
                        item.TenLop = reader1.GetString(1);
                        LopDataCmb.Add(item);
                        if (String.IsNullOrEmpty(LopQueries))
                        {
                            LopQueries = reader1.GetInt32(0).ToString();
                            //try
                            //{
                            //    QuanLiDiemSoWD.cmbLop.SelectedIndex = 0;
                            //}catch (Exception) { }
                        }
                    }
                    reader1.NextResult();
                }
            }
        }

        public void LoadDanhSachLop2()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                sqlConnectionWrap.Open();
                string CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries2 + "' and MaKhoi = " + KhoiQueries2;
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                SqlDataReader reader2 = cmd.ExecuteReader();

                while (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        Lop item = new Lop();
                        item.MaLop = reader2.GetInt32(0);
                        item.TenLop = reader2.GetString(1);
                        LopDataCmb2.Add(item);
                        if (String.IsNullOrEmpty(LopQueries2))
                        {
                            LopQueries2 = reader2.GetInt32(0).ToString();
                            //try
                            //{
                            //    QuanLiDiemSoWD.cmbLop2.SelectedIndex = 0;
                            //}catch (Exception) { }
                        }
                    }
                    reader2.NextResult();
                }
            }
        }
        public void LocLop()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {

                    sqlConnectionWrap.Open();
                    string CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries + "' and MaKhoi = " + KhoiQueries;
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
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
                    //QuanLiDiemSoWD.cmbLop.SelectedIndex = 0;

            }
        }

        public void LocLop2()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries2 + "' and MaKhoi = " + KhoiQueries2;
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
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
                    //QuanLiDiemSoWD.cmbLop2.SelectedIndex = 0;
                }
                catch (Exception)
                {
                }
            }
        }

        public int MoKhoa()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                sqlConnectionWrap.Open();
                string hocky;
                if (HocKyQueries2 == 1)
                {
                    hocky = mot;
                }
                else hocky = hai;
                string CmdString = "Update HeThongDiem "
                                  + "set TrangThai = 0 "
                                  + "where MaLop = " + LopQueries2 + " and HocKy = " + hocky;
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                return cmd.ExecuteNonQuery();
                //MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                //messageBoxSuccessful.ShowDialog();
            }
        }

        public int[] Khoa()
        {
            int[] result = new int[4];
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    // khóa trạng thái
                    string CmdString = "update HeThongDiem set TrangThai = 1 where MaLop = " + LopQueries2 +
                                       " and HocKy = " + HocKyQueries2.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    result[0] = cmd.ExecuteNonQuery();
                    sqlConnectionWrap.Close();

                    sqlConnectionWrap.Open();
                    // cập nhật database thành tích
                    CmdString = "select HocKy,MaLop,MaHocSinh,avg(DiemTrungBinh) TBHK " +
                                "from HeThongDiem " +
                                "where MaLop = " + LopQueries2 + " and HocKy = " + HocKyQueries2.ToString() +
                                " group by HocKy,MaLop,MaHocSinh";
                    cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.ThanhTich thanhtich = new StudentManagement.Model.ThanhTich
                            {
                                HocKy = reader.GetInt32(0),
                                MaLop = reader.GetInt32(1),
                                MaHocSinh = reader.GetInt32(2),
                                TBHK = (float)reader.GetDecimal(3),
                            };
                            DanhSachThanhTich.Add(thanhtich);
                        }
                        reader.NextResult();
                    }
                    sqlConnectionWrap.Close();

                    sqlConnectionWrap.Open();
                    for (int i = 0; i < DanhSachThanhTich.Count; i++)
                    {
                        // chia 2 trường hợp cập nhật lại hoặc thêm mới
                        int xeploai = 0;
                        if (DanhSachThanhTich[i].TBHK >= 5)
                            xeploai = 1;
                        CmdString = "if (exists(select * from ThanhTich " +
                                                "where MaHocSinh = " + DanhSachThanhTich[i].MaHocSinh.ToString() + " and HocKy = " + DanhSachThanhTich[i].HocKy.ToString() + " and MaLop = " + DanhSachThanhTich[i].MaLop.ToString() + ")) " +
                                    "begin " +
                                    "update ThanhTich " +
                                    "set TrungBinhHocKy = " + DanhSachThanhTich[i].TBHK.ToString() + ", XepLoai = " + xeploai.ToString() + " " +
                                    "where MaHocSinh = " + DanhSachThanhTich[i].MaHocSinh.ToString() + " and HocKy = " + DanhSachThanhTich[i].HocKy.ToString() + " and MaLop = " + DanhSachThanhTich[i].MaLop.ToString() + " " +
                                    "end " +
                                    "else " +
                                    "begin " +
                                    "insert into ThanhTich(HocKy,MaLop,MaHocSinh,TrungBinhHocKy,XepLoai) values("
                                    + DanhSachThanhTich[i].HocKy.ToString() + "," + DanhSachThanhTich[i].MaLop.ToString() + "," +
                                    DanhSachThanhTich[i].MaHocSinh.ToString() + "," + DanhSachThanhTich[i].TBHK.ToString() + "," + xeploai.ToString() + ") end";

                        cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                        result[1] = cmd.ExecuteNonQuery();
                    }
                    sqlConnectionWrap.Close();

                    // cập nhật database báo cáo môn học
                    sqlConnectionWrap.Open();
                    CmdString = "select MaLop,MaMon,HocKy,count(MaDiem) SLD,SiSo = (select SiSo from Lop l where l.MaLop = htd.MaLop) " +
                                "from HeThongDiem htd " +
                                "where XepLoai = 1 and MaLop = " + LopQueries2 + " and HocKy = " + HocKyQueries2.ToString() +
                                " group by MaLop,MaMon,HocKy";
                    cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.BaoCaoMon baocaomon = new StudentManagement.Model.BaoCaoMon
                            {
                                MaLop = reader.GetInt32(0),
                                MaMon = reader.GetInt32(1),
                                HocKy = reader.GetInt32(2),
                                SoLuongDat = reader.GetInt32(3),
                                TiLe = CountPercentage(reader.GetInt32(3), reader.GetInt32(4)),
                            };
                            DanhSachBaoCaoMon.Add(baocaomon);
                        }
                        reader.NextResult();
                    }
                    sqlConnectionWrap.Close();

                    sqlConnectionWrap.Open();
                    for (int i = 0; i < DanhSachBaoCaoMon.Count; i++)
                    {
                        // chia 2 trường hợp cập nhật lại hoặc thêm mới
                        CmdString = "if (exists(select * from BaoCaoMon " +
                                                "where MaLop = " + DanhSachBaoCaoMon[i].MaLop.ToString() + " and HocKy = " + DanhSachBaoCaoMon[i].HocKy.ToString() + " and MaMon = " + DanhSachBaoCaoMon[i].MaMon.ToString() + ")) " +
                                    "begin " +
                                    "update BaoCaoMon " +
                                    "set SoLuongDat = " + DanhSachBaoCaoMon[i].SoLuongDat.ToString() + ", TiLe = '" + DanhSachBaoCaoMon[i].TiLe + "' " +
                                    "where MaLop = " + DanhSachBaoCaoMon[i].MaLop.ToString() + " and HocKy = " + DanhSachBaoCaoMon[i].HocKy.ToString() +
                                    " end " +
                                    "else " +
                                    "begin " +
                                    "insert into BaoCaoMon(MaLop,MaMon,HocKy,SoLuongDat,TiLe) values(" + DanhSachBaoCaoMon[i].MaLop.ToString() + ","
                                    + DanhSachBaoCaoMon[i].MaMon.ToString() + "," + DanhSachBaoCaoMon[i].HocKy.ToString() + "," +
                                    DanhSachBaoCaoMon[i].SoLuongDat.ToString() + ",'" + DanhSachBaoCaoMon[i].TiLe + "') " +
                                    "end";
                        cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                        result[2] = cmd.ExecuteNonQuery();
                    }
                    sqlConnectionWrap.Close();

                    // cập nhật database báo cáo tổng kết học kỳ
                    sqlConnectionWrap.Open();
                    CmdString = "select MaLop, SiSo = (select SiSo from Lop l where l.MaLop = tt.MaLop), HocKy,count(MaThanhTich) SLD " +
                                "from ThanhTich tt " +
                                "where XepLoai = 1 and MaLop = " + LopQueries2 + " and HocKy = " + HocKyQueries2.ToString() + " " +
                                "group by MaLop,HocKy";
                    cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.BaoCaoHocKy baocaohocky = new StudentManagement.Model.BaoCaoHocKy
                            {
                                MaLop = reader.GetInt32(0),
                                SiSo = reader.GetInt32(1),
                                HocKy = reader.GetInt32(2),
                                SoLuongDat = reader.GetInt32(3),
                                TiLe = CountPercentage(reader.GetInt32(3), reader.GetInt32(1)),
                            };
                            DanhSachBaoCaoHocKy.Add(baocaohocky);
                        }
                        reader.NextResult();
                    }
                    sqlConnectionWrap.Close();

                    sqlConnectionWrap.Open();
                    for (int i = 0; i < DanhSachBaoCaoHocKy.Count; i++)
                    {
                        // chia 2 trường hợp cập nhật lại hoặc thêm mới
                        CmdString = "if (exists(select * from BaoCaoHocKy " +
                                                "where MaLop = " + DanhSachBaoCaoHocKy[i].MaLop.ToString() + " and HocKy = " + DanhSachBaoCaoHocKy[i].HocKy.ToString() + ")) " +
                                    "begin " +
                                    "update BaoCaoHocKy " +
                                    "set SoLuongDat = " + DanhSachBaoCaoHocKy[i].SoLuongDat.ToString() + ", TiLe = '" + DanhSachBaoCaoHocKy[i].TiLe + "' " +
                                    "where MaLop = " + DanhSachBaoCaoHocKy[i].MaLop.ToString() + " and HocKy = " + DanhSachBaoCaoHocKy[i].HocKy.ToString() + " " +
                                    "end " +
                                    "else " +
                                    "begin " +
                                    "insert into BaoCaoHocKy(MaLop,HocKy,SoLuongDat,TiLe) values(" + DanhSachBaoCaoHocKy[i].MaLop.ToString() +
                                    "," + DanhSachBaoCaoHocKy[i].HocKy.ToString() + "," +
                                    DanhSachBaoCaoHocKy[i].SoLuongDat.ToString() + ",'" + DanhSachBaoCaoHocKy[i].TiLe + "') " +
                                    "end";
                        cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                        result[3] = cmd.ExecuteNonQuery();
                    }
                    //MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                    //messageBoxSuccessful.ShowDialog();
                }
                catch (Exception)
                {
                    //MessageBoxOK messageBoxOK = new MessageBoxOK();
                    //MessageBoxOKViewModel dataOK = messageBoxOK.DataContext as MessageBoxOKViewModel;
                    //dataOK.Content = ex.Message;
                    //messageBoxOK.ShowDialog();
                }
            }
            return result;
        }
        public string CountPercentage(int a,int b)
        {
            return (Math.Round((double)a / b * 100)).ToString() + "%";
        }

        public QuanLiDiemSoViewModel()
        {
            // Stryker disable all
            everLoaded = false;
            DanhSachDiem = new ObservableCollection<HeThongDiem>();
            DanhSachThanhTich = new ObservableCollection<ThanhTich>();
            DanhSachBaoCaoMon = new ObservableCollection<BaoCaoMon>();
            DanhSachBaoCaoHocKy = new ObservableCollection<BaoCaoHocKy>();
            NienKhoaCmb = new ObservableCollection<string>();
            MonDataCmb = new ObservableCollection<Model.MonHoc>();
            LopDataCmb = new ObservableCollection<Lop>();
            KhoiDataCmb = new ObservableCollection<Khoi>();
            NienKhoaCmb2 = new ObservableCollection<string>();
            LopDataCmb2 = new ObservableCollection<Lop>();
            KhoiDataCmb2 = new ObservableCollection<Khoi>();
            LoadWindow = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                if (everLoaded == false)
                {
                    QuanLiDiemSoWD = parameter as QuanLiDiemSo;
                    NienKhoaQueries = KhoiQueries = LopQueries = MonHocQueries = "";
                    NienKhoaQueries2 = KhoiQueries2 = LopQueries2 = "";
                    HocKyQueries = 1;
                    HocKyQueries2 = 1;
                    KhoiDataCmb.Clear();
                    KhoiDataCmb2.Clear();
                    LopDataCmb.Clear();
                    LopDataCmb2.Clear();
                    MonDataCmb.Clear();
                    NienKhoaCmb.Clear();
                    NienKhoaCmb2.Clear();
                    if (QuanLiDiemSoWD != null)
                    {
                        QuanLiDiemSoWD.cmbHocKy.Items.Clear();
                        QuanLiDiemSoWD.cmbHocKy2.Items.Clear();
                        QuanLiDiemSoWD.cmbHocKy.Items.Add("Học kỳ 1");
                        QuanLiDiemSoWD.cmbHocKy.Items.Add("Học kỳ 2");
                        QuanLiDiemSoWD.cmbHocKy2.Items.Add("Học kỳ 1");
                        QuanLiDiemSoWD.cmbHocKy2.Items.Add("Học kỳ 2");
                        QuanLiDiemSoWD.cmbHocKy.SelectedIndex = 0;
                        QuanLiDiemSoWD.cmbHocKy2.SelectedIndex = 0;
                    }
                    try
                    {
                        LoadDanhSachNienKhoa();
                        LoadDanhSachKhoi();
                    }
                    catch (Exception)
                    {
                    }
                    if (!String.IsNullOrEmpty(NienKhoaQueries))
                    {
                        try
                        {
                            LoadDanhSachLop1();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (!String.IsNullOrEmpty(NienKhoaQueries2))
                    {
                        try
                        {
                            LoadDanhSachLop2();
                        }
                        catch (Exception)
                        {
                        }
                    }

                    try
                    {
                        LoadDanhSachMon();
                    }
                    catch (Exception)
                    {
                    }
                    ProgressBarVisibility = true;
                    DataGridVisibility = false;
                    DanhSachDiem.Clear();
                    await LoadDanhSachBangDiem();
                    QuanLiDiemSoWD.cmbLop.Focus();
                    QuanLiDiemSoWD.btnTrick.Focus();
                    ProgressBarVisibility = false;
                    DataGridVisibility = true;
                    everLoaded = true;
                }

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
                if (cmb != null && cmb.SelectedItem != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    LopDataCmb.Clear();
                    LocLop();
                }
            });
            FilterHocKy = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    if (cmb.SelectedItem.ToString().Contains("1"))
                        HocKyQueries = 1;
                    else
                        HocKyQueries = 2;
                    ProgressBarVisibility = true;
                    DataGridVisibility = false;
                    DanhSachDiem.Clear();
                    await LoadDanhSachBangDiem();
                    QuanLiDiemSoWD.cmbLop.Focus();
                    QuanLiDiemSoWD.btnTrick.Focus();
                    ProgressBarVisibility = false;
                    DataGridVisibility = true;
                    everLoaded = true;
                }
            });
            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    Khoi item = cmb.SelectedItem as Khoi;
                    KhoiQueries = item.MaKhoi.ToString();
                    LopDataCmb.Clear();
                    LocLop();
                }
            });
            FilterLop = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    Lop item = cmb.SelectedItem as Lop;
                    if (item != null && cmb.SelectedItem != null)
                    {
                        LopQueries = item.MaLop.ToString();
                        ProgressBarVisibility = true;
                        DataGridVisibility = false;
                        DanhSachDiem.Clear();
                        await LoadDanhSachBangDiem();
                        QuanLiDiemSoWD.cmbLop.Focus();
                        QuanLiDiemSoWD.btnTrick.Focus();
                        ProgressBarVisibility = false;
                        DataGridVisibility = true;
                        everLoaded = true;
                    }
                }
            });
            FilterMonHoc = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    Model.MonHoc item = cmb.SelectedItem as Model.MonHoc;
                    MonHocQueries = item.MaMon.ToString();
                    ProgressBarVisibility = true;
                    DataGridVisibility = false;
                    DanhSachDiem.Clear();
                    await LoadDanhSachBangDiem();
                    QuanLiDiemSoWD.cmbLop.Focus();
                    QuanLiDiemSoWD.btnTrick.Focus();
                    ProgressBarVisibility = false;
                    DataGridVisibility = true;
                    everLoaded = true;
                }
            });
            FilterNienKhoa2 = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    NienKhoaQueries2 = cmb.SelectedItem.ToString();
                    LopDataCmb2.Clear();
                    LocLop2();
                }
            });
            FilterHocKy2 = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
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
                if (cmb != null && cmb.SelectedItem != null)
                {
                    Khoi item = cmb.SelectedItem as Khoi;
                    KhoiQueries2 = item.MaKhoi.ToString();
                    LopDataCmb2.Clear();
                    LocLop2();
                }
            });
            FilterLop2 = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
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
                MessageBoxYesNo wd = new MessageBoxYesNo();

                var data = wd.DataContext as MessageBoxYesNoViewModel;
                data.Title = "Xác nhận!";
                data.Question = "Bạn có chắc chắn muốn mở khóa bảng điểm lớp này?";
                wd.ShowDialog();

                var result = wd.DataContext as MessageBoxYesNoViewModel;
                if (result.IsYes == true)
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                    {
                        try
                        {
                            MoKhoa();
                        }
                        catch (Exception)
                        {
                            MessageBoxFail messageBoxFail = new MessageBoxFail();
                            messageBoxFail.ShowDialog();
                        }
                    }
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
                MessageBoxYesNo wd = new MessageBoxYesNo();

                var data = wd.DataContext as MessageBoxYesNoViewModel;
                data.Title = "Xác nhận!";
                data.Question = "Bạn có chắc chắn muốn khóa bảng điểm lớp này?";
                wd.ShowDialog();

                var result = wd.DataContext as MessageBoxYesNoViewModel;
                if (result.IsYes == true)
                {
                    DanhSachThanhTich.Clear();
                    DanhSachBaoCaoMon.Clear();
                    DanhSachBaoCaoHocKy.Clear();
                    MoKhoa();
                }
            });

        }
    }
}
