using LiveCharts;
using LiveCharts.Wpf;
using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    internal class BaoCaoMonHocViewModel : BaseViewModel
    {
        public string NienKhoaQueries { get; set; }
        public string HocKyQueries { get; set; }
        public string MonHocQueries { get; set; }
        public string LopQueries { get; set; }
        public int TongSiSoLop { get; set; }

        public BaoCaoMonHoc BaoCaoWD;

        private ObservableCollection<StudentManagement.Model.BaoCaoMon> _danhSachBaoCaoMon;
        public ObservableCollection<StudentManagement.Model.BaoCaoMon> DanhSachBaoCaoMon
        {
            get => _danhSachBaoCaoMon;
            set { _danhSachBaoCaoMon = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> _nienKhoaComboBox;
        public ObservableCollection<string> NienKhoaComboBox
        {
            get => _nienKhoaComboBox;
            set { _nienKhoaComboBox = value; OnPropertyChanged(); }
        }

        public ObservableCollection<int> _hocKyComboBox;
        public ObservableCollection<int> HocKyComboBox
        {
            get => _hocKyComboBox;
            set { _hocKyComboBox = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> _monHocComboBox;

        public ObservableCollection<string> MonHocComboBox
        {
            get => _monHocComboBox;
            set { _monHocComboBox = value; OnPropertyChanged(); }
        }

        public int _dat;
        public int Dat
        {
            get => _dat;
            set { _dat = value; OnPropertyChanged(); }
        }
        public int _khongDat;
        public int KhongDat
        {
            get => _khongDat;
            set { _khongDat = value; OnPropertyChanged(); }
        }

        private List<string> _tenLop;

        public List<int> _soLuongDatChartVal;

        public List<string> TenLop
        {
            get => _tenLop;
            set { _tenLop = value; OnPropertyChanged(); }
        }

        public List<int> SoLuongDatChartVal
        {
            get => _soLuongDatChartVal;
            set { _soLuongDatChartVal = value; OnPropertyChanged(); }
        }

        //public SeriesCollection SoLuongDat { get; set; }

        private LiveCharts.SeriesCollection _soLuongDat;
        public LiveCharts.SeriesCollection SoLuongDat
        {
            get => _soLuongDat;
            set { _soLuongDat = value; OnPropertyChanged(); }
        }


        private LiveCharts.SeriesCollection _tiLeDat;
        public LiveCharts.SeriesCollection TiLeDat
        {
            get => _tiLeDat;
            set { _tiLeDat = value; OnPropertyChanged(); }
        }

        public CartesianChart ReportChart { get; set; }

        private BaoCaoMon _gridSeletecdItem;

        public BaoCaoMon GridSelectedItem
        {
            get { return _gridSeletecdItem; }
            set
            {
                _gridSeletecdItem = value;
                OnPropertyChanged();
            }
        }

        private bool _cartesianChartVisibility;
        public bool CartersianChartVisibility
        {
            get { return _cartesianChartVisibility; }
            set
            {
                _cartesianChartVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _pieChartVisibility;
        public bool PieChartVisibility
        {
            get { return _pieChartVisibility; }
            set
            {
                _pieChartVisibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadBaoCao { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterHocKy { get; set; }
        public ICommand FilterMonHoc { get; set; }
        public ICommand BaoCaoMonSelectionChanged { get; set; }

        public BaoCaoMonHocViewModel()
        {
            NienKhoaComboBox = new ObservableCollection<string>();
            MonHocComboBox = new ObservableCollection<string>();
            HocKyComboBox = new ObservableCollection<int>();
            DanhSachBaoCaoMon = new ObservableCollection<Model.BaoCaoMon>();
            TenLop = new List<string>();
            SoLuongDatChartVal = new List<int>();
            SoLuongDat = new SeriesCollection();
            Dat = new int();
            KhongDat = new int();
            TiLeDat = new SeriesCollection();


            LoadDanhSachBaoCaoMon();
            LoadCartesianChart();
            CartersianChartVisibility = true;
            PieChartVisibility = false;
            LoadBaoCao = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                BaoCaoWD = parameter as BaoCaoMonHoc;
                LoadComboboxData();
            });

            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    FilterHocKyFromNienKhoa();
                }
            });


            FilterHocKy = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    HocKyQueries = cmb.SelectedItem.ToString();
                    FilterMonHocFromHocKy();
                }
            });


            FilterMonHoc = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    MonHocQueries = cmb.SelectedItem.ToString();
                }
                LoadDanhSachBaoCaoMon();
                LoadCartesianChart();
                CartersianChartVisibility = true;
                PieChartVisibility = false;
            });

            BaoCaoMonSelectionChanged = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                LoadPieChart();
                CartersianChartVisibility = false;
                PieChartVisibility = true;
            });
        }



        public void LoadComboboxData()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                    string cmdString = "select distinct NienKhoa from BaoCaoMon";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            NienKhoaComboBox.Add(reader.GetString(0));
                            if (string.IsNullOrEmpty(NienKhoaQueries))
                            {
                                NienKhoaQueries = reader.GetString(0);
                                BaoCaoWD.cmbNienKhoa.SelectedIndex = 0;
                                FilterHocKyFromNienKhoa();
                                BaoCaoWD.cmbHocKy.SelectedIndex = 0;
                                HocKyQueries = BaoCaoWD.cmbHocKy.Text;
                                FilterMonHocFromHocKy();
                                BaoCaoWD.cmbMonHoc.SelectedIndex = 0;
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();

                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra đường truyền");
                }
                NienKhoaQueries = BaoCaoWD.cmbNienKhoa.Text;
                HocKyQueries = BaoCaoWD.cmbHocKy.Text;
                MonHocQueries = BaoCaoWD.cmbMonHoc.Text;
            }
        }


        public void FilterHocKyFromNienKhoa()
        {
            HocKyComboBox.Clear();
            MonHocComboBox.Clear();
            HocKyQueries = null;
            MonHocQueries = "";
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                    string cmdString = "select distinct HocKy from BaoCaoMon where NienKhoa = '" + NienKhoaQueries + "'";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            HocKyComboBox.Add(reader.GetInt32(0));
                            if (HocKyQueries != null)
                            {
                                HocKyQueries = reader.GetInt32(0).ToString();
                            }
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


        public void FilterMonHocFromHocKy()
        {
            MonHocComboBox.Clear();
            MonHocQueries = "";
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                    string cmdString = "select distinct TenMon from BaoCaoMon where NienKhoa = '" + NienKhoaQueries + "' and HocKy = '" + HocKyQueries + "'";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MonHocComboBox.Add(reader.GetString(0));
                            if (string.IsNullOrEmpty(LopQueries))
                            {
                                MonHocQueries = reader.GetString(0);
                            }
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



        public void LoadDanhSachBaoCaoMon()
        {
            DanhSachBaoCaoMon.Clear();
            if (!String.IsNullOrEmpty(NienKhoaQueries) && HocKyQueries != null && !String.IsNullOrEmpty(MonHocQueries))
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try
                    {
                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                        string CmdString = "select * from BaoCaoMon where NienKhoa = '" + NienKhoaQueries + "' and HocKy ='" + HocKyQueries + "' and TenMon = '" + MonHocQueries + "'";
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                StudentManagement.Model.BaoCaoMon baocaomon = new StudentManagement.Model.BaoCaoMon();
                                baocaomon.TenLop = reader.GetString(2);
                                baocaomon.TenMon = reader.GetString(4);
                                baocaomon.SoLuongDat = reader.GetInt32(7);
                                baocaomon.TiLe = reader.GetString(8);
                                DanhSachBaoCaoMon.Add(baocaomon);
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
        }
        public void LoadCartesianChart()
        {
            TenLop.Clear();
            SoLuongDatChartVal.Clear();
            SoLuongDat.Clear();
            if (!String.IsNullOrEmpty(NienKhoaQueries) && HocKyQueries != null && !String.IsNullOrEmpty(MonHocQueries))
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try
                    {
                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                        string CmdString = "SELECT DISTINCT TenLop from BaoCaoMon where NienKhoa = '" + NienKhoaQueries + "' and HocKy ='" + HocKyQueries + "' and TenMon = '" + MonHocQueries + "'";
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                TenLop.Add(reader.GetString(0));
                            }
                            reader.NextResult();
                        }
                        con.Close();




                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                        CmdString = "SELECT SoLuongDat from BaoCaoMon where NienKhoa = '" + NienKhoaQueries + "' and HocKy ='" + HocKyQueries + "' and TenMon = '" + MonHocQueries + "'";
                        cmd = new SqlCommand(CmdString, con);
                        reader = cmd.ExecuteReader();

                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                SoLuongDatChartVal.Add(reader.GetInt32(0));
                            }

                            reader.NextResult();
                        }

                        SoLuongDat.Add(new ColumnSeries
                        {
                            Title = "Số lượng đạt",
                            Values = new ChartValues<int>(SoLuongDatChartVal)
                        });

                        con.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                    }
                }
            }
        }
        public void LoadPieChart()
        {
            TiLeDat.Clear();
            if (!String.IsNullOrEmpty(NienKhoaQueries) && HocKyQueries != null && !String.IsNullOrEmpty(MonHocQueries) && GridSelectedItem != null)
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try
                    {
                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                        string CmdString = "SELECT SoLuongDat from BaoCaoMon where NienKhoa = '" + NienKhoaQueries + "' and HocKy ='" + HocKyQueries + "' and TenMon = '" + MonHocQueries + "' and TenLop = '" + GridSelectedItem.TenLop + "'";
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Dat = reader.GetInt32(0);
                            }
                            reader.NextResult();
                        }
                        con.Close();


                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                        CmdString = "SELECT SiSo from Lop where NienKhoa = '" + NienKhoaQueries + "' and TenLop = '" + GridSelectedItem.TenLop + "'";
                        cmd = new SqlCommand(CmdString, con);
                        reader = cmd.ExecuteReader();

                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                TongSiSoLop = reader.GetInt32(0);
                            }
                            reader.NextResult();
                        }
                        con.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                    }
                    KhongDat = TongSiSoLop - Dat;
                    TiLeDat = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Tỉ lệ đạt",
                        Values = new ChartValues<int> {Dat}
                    },
                    new PieSeries
                    {
                        Title = "Tỉ lệ không đạt",
                        Values = new ChartValues<int> {KhongDat}
                    }
                };
                }
            }
        }
    }
}
