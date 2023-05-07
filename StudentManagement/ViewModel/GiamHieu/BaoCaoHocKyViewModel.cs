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
using StudentManagement.Views.GiaoVien;

namespace StudentManagement.ViewModel.GiamHieu
{
    internal class BaoCaoHocKyViewModel : BaseViewModel
    {
        public string NienKhoaQueries { get; set; }
        public string HocKyQueries { get; set; }
        public string MonHocQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }
        public int TongSiSoLop { get; set; }

        public BaoCaoTongKetHocKy BaoCaoHocKyWD;

        private ObservableCollection<StudentManagement.Model.BaoCaoHocKy> _danhSachBaoCaoHocKy;
        public ObservableCollection<StudentManagement.Model.BaoCaoHocKy> DanhSachBaoCaoHocKy
        {
            get => _danhSachBaoCaoHocKy;
            set { _danhSachBaoCaoHocKy = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> _nienKhoaComboBox;
        public ObservableCollection<string> NienKhoaComboBox
        {
            get => _nienKhoaComboBox;
            set { _nienKhoaComboBox = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _hocKyComboBox;
        public ObservableCollection<string> HocKyComboBox
        {
            get => _hocKyComboBox;
            set { _hocKyComboBox = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> _khoiComboBox;
        public ObservableCollection<string> KhoiComboBox
        {
            get => _khoiComboBox;
            set { _khoiComboBox = value; OnPropertyChanged(); }
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

        public BaoCaoHocKy _gridSeletecdItem;

        public BaoCaoHocKy GridSelectedItem
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
        public ICommand FilterKhoi { get; set; }

        public ICommand BaoCaoHocKySelectionChanged { get; set; }


        public BaoCaoHocKyViewModel()
        {

            LoadBaoCao = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                BaoCaoHocKyWD = parameter as BaoCaoTongKetHocKy;
                LoadComboboxData();
                LoadDanhSachBaoCaoHocKy();
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
                    FilterKhoiFromHocKy();
                }
                LoadDanhSachBaoCaoHocKy();
            });


            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    KhoiQueries = cmb.SelectedItem.ToString();
                }
                LoadDanhSachBaoCaoHocKy();
            });

            BaoCaoHocKySelectionChanged = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                LoadPieChart();
                CartersianChartVisibility = false;
                PieChartVisibility = true;
            });
        }



        public void LoadComboboxData()
        {
            NienKhoaComboBox = new ObservableCollection<string>();
            HocKyComboBox = new ObservableCollection<string>();
            KhoiComboBox = new ObservableCollection<string>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string cmdString = "select distinct NienKhoa from BaoCaoHocKy";
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
                            BaoCaoHocKyWD.cmbNienKhoa.SelectedIndex = 0;
                            FilterHocKyFromNienKhoa();
                            BaoCaoHocKyWD.cmbHocKy.SelectedIndex = 0;
                            HocKyQueries = BaoCaoHocKyWD.cmbHocKy.Text;
                            FilterKhoiFromHocKy();
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
            KhoiComboBox.Clear();
            HocKyQueries = null;
            KhoiQueries = "";
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string cmdString = "select distinct HocKy from BaoCaoHocKy where NienKhoa = '" + NienKhoaQueries + "'";
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HocKyComboBox.Add(reader.GetInt32(0).ToString());
                        if (string.IsNullOrEmpty(NienKhoaQueries))
                        {
                            HocKyQueries = reader.GetInt32(0).ToString();
                        }
                     }
                    reader.NextResult();
                }
                con.Close();
            }
        }


        public void FilterKhoiFromHocKy()
        {
            KhoiComboBox.Clear();
            KhoiQueries = null;
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string cmdString = "select distinct left(TenLop,2) from BaoCaoHocKy where NienKhoa = '" + NienKhoaQueries + "' and HocKy = '"+HocKyQueries+"'";
                MessageBox.Show(cmdString);
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        KhoiComboBox.Add(reader.GetString(0));
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }



        public void LoadDanhSachBaoCaoHocKy()
        {
            DanhSachBaoCaoHocKy = new ObservableCollection<Model.BaoCaoHocKy>();
            DanhSachBaoCaoHocKy.Clear();
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString;
                if (String.IsNullOrEmpty(KhoiQueries))
                {
                    CmdString = "select * from BaoCaoHocKy where NienKhoa = '" + NienKhoaQueries + "' and HocKy ='" + HocKyQueries + "'";
                    MessageBox.Show(CmdString);
                } else
                {
                    CmdString = "select * from BaoCaoHocKy where NienKhoa = '" + NienKhoaQueries + "' and HocKy ='" + HocKyQueries + "' and Khoi = '"+KhoiQueries+"'";
                }
                SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.BaoCaoHocKy baocaohocky = new StudentManagement.Model.BaoCaoHocKy();
                            baocaohocky.TenLop = reader.GetString(2);
                            baocaohocky.SiSo = reader.GetInt32(3);
                            baocaohocky.SoLuongDat = reader.GetInt32(6);
                            baocaohocky.TiLe = reader.GetString(7);
                            DanhSachBaoCaoHocKy.Add(baocaohocky);
                        }
                        reader.NextResult();
                    }
                    con.Close();
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
            if (!String.IsNullOrEmpty(NienKhoaQueries) && !String.IsNullOrEmpty(HocKyQueries))
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString = "SELECT DISTINCT TenLop from BaoCaoHocKy where NienKhoa = '" + NienKhoaQueries + "' and HocKy ='" + HocKyQueries + "'";
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
                    CmdString = "SELECT SoLuongDat from BaoCaoHocKy where NienKhoa = '" + NienKhoaQueries + "' and HocKy ='" + HocKyQueries + "'";
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
            if (!String.IsNullOrEmpty(NienKhoaQueries) && !String.IsNullOrEmpty(HocKyQueries))
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString = "SELECT SoLuongDat from BaoCaoHocKy where NienKhoa = '" + NienKhoaQueries + "' and HocKy ='" + HocKyQueries + "' and TenLop = '" + GridSelectedItem.TenLop + "'";
                    MessageBox.Show(CmdString);
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
                    CmdString = "SELECT SiSo from BaoCaoHocKy where NienKhoa = '" + NienKhoaQueries + "' and HocKy ='" + HocKyQueries + "' and TenLop = '" + GridSelectedItem.TenLop + "'";
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

