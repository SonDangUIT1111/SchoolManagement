using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using StudentManagement.Model;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System.Windows;
using StudentManagement.Views.GiamHieu;
using System.Windows.Input;
using System.Windows.Controls;
using System.Runtime.Remoting.Messaging;
using MaterialDesignThemes.Wpf;

namespace StudentManagement.ViewModel.GiamHieu
{
    internal class BaoCaoViewModel:BaseViewModel
    {
        public string NienKhoaQueries { get; set; }
        public string HocKyQueries { get; set; }
        public string MonHocQueries { get; set; }
        public string LopQueries { get; set; }
        public int TongSiSoLop { get; set; }

        public BaoCao BaoCaoWD;

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
            set { _hocKyComboBox = value; OnPropertyChanged();}
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

        public List<string> _tenLop { get; set; }

        public List<int> _soLuongDatChartVal { get; set; }

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

        public LiveCharts.SeriesCollection _soLuongDat { get; set; }
        public LiveCharts.SeriesCollection SoLuongDat
        {
            get => _soLuongDat;
            set { _soLuongDat = value; OnPropertyChanged(); }
        }


        public LiveCharts.SeriesCollection _tiLeDat { get; set; }
        public LiveCharts.SeriesCollection TiLeDat
        {
            get => _tiLeDat;
            set { _tiLeDat = value; OnPropertyChanged(); }
        }

        public CartesianChart ReportChart { get; set; }

        public BaoCaoMon _gridSeletecdItem;

        public BaoCaoMon GridSelectedItem
        {
            get { return _gridSeletecdItem; }
            set
            {
                _gridSeletecdItem = value;
                OnPropertyChanged();
            }
        }

        public bool _cartesianChartVisibility;
        public bool CartersianChartVisibility
        {
            get { return _cartesianChartVisibility; }
            set
            {
                _cartesianChartVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool _pieChartVisibility;
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

        public BaoCaoViewModel()
        {
            LoadCartesianChart();
            LoadBaoCao = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                BaoCaoWD = parameter as BaoCao;
                LoadComboboxData();
                LoadDanhSachBaoCaoMon();
                LoadCartesianChart();
                CartersianChartVisibility = true;
                PieChartVisibility = false;
            });


            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    FilterHocKyFromNienKhoa();
                }
            });


            FilterHocKy = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    HocKyQueries = cmb.SelectedItem.ToString();
                    FilterMonHocFromHocKy();
                }
            });


            FilterMonHoc = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
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
            NienKhoaComboBox = new ObservableCollection<string>();
            MonHocComboBox = new ObservableCollection<string>();
            HocKyComboBox = new ObservableCollection<int>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string cmdString = "select distinct NienKhoa from BaoCaoMon";
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NienKhoaComboBox.Add(reader.GetString(0)); 
                        if(string.IsNullOrEmpty(NienKhoaQueries))
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
        }


        public void FilterHocKyFromNienKhoa()
        {
            HocKyComboBox.Clear();
            MonHocComboBox.Clear();
            HocKyQueries = null;
            MonHocQueries = "";
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string cmdString = "select distinct HocKy from BaoCaoMon where NienKhoa = '"+NienKhoaQueries+"'";
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
        }

        public void FilterMonHocFromHocKy()
        {
            MonHocComboBox.Clear();
            MonHocQueries = "";
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string cmdString = "select distinct TenMon from BaoCaoMon where NienKhoa = '"+NienKhoaQueries+"' and HocKy = '"+HocKyQueries+"'";
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
        }

        public void LoadDanhSachBaoCaoMon()
        {
            DanhSachBaoCaoMon = new ObservableCollection<Model.BaoCaoMon>();
            DanhSachBaoCaoMon.Clear();
            if (!String.IsNullOrEmpty(NienKhoaQueries) && HocKyQueries != null && !String.IsNullOrEmpty(MonHocQueries))
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
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
            }
        }
        public void LoadCartesianChart()
        {
            TenLop = new List<string>();
            SoLuongDatChartVal = new List<int>();
            SoLuongDat = new SeriesCollection();
            TenLop.Clear();
            SoLuongDatChartVal.Clear();
            SoLuongDat.Clear();
            if (!String.IsNullOrEmpty(NienKhoaQueries) && HocKyQueries != null && !String.IsNullOrEmpty(MonHocQueries))
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
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




                    con.Open();
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
            }
        }
        public void LoadPieChart()
        {
            Dat = new int();
            KhongDat = new int();
            TiLeDat = new SeriesCollection();
            TiLeDat.Clear();
            if (!String.IsNullOrEmpty(NienKhoaQueries) && HocKyQueries != null && !String.IsNullOrEmpty(MonHocQueries) && GridSelectedItem != null)
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
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


                    con.Open();
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
