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
    internal class BaoCaoHocKyViewModel : BaseViewModel
    {
        public string NienKhoaQueries { get; set; }
        public string HocKyQueries { get; set; }
        public string MonHocQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }
        public int TongSiSoLop { get; set; }
        public bool everLoaded { get; set; }

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


        private ObservableCollection<Model.Khoi> _khoiComboBox;
        public ObservableCollection<Model.Khoi> KhoiComboBox
        {
            get => _khoiComboBox;
            set { _khoiComboBox = value; OnPropertyChanged(); }
        }



        private int _dat;
        public int Dat
        {
            get => _dat;
            set { _dat = value; OnPropertyChanged(); }
        }
        private int _khongDat;
        public int KhongDat
        {
            get => _khongDat;
            set { _khongDat = value; OnPropertyChanged(); }
        }

        private List<string> _tenLop;

        private List<int> _soLuongDatChartVal;

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

        private BaoCaoHocKy _gridSeletecdItem;

        public BaoCaoHocKy GridSelectedItem
        {
            get { return _gridSeletecdItem; }
            set
            {
                _gridSeletecdItem = value;
                OnPropertyChanged();
                LoadPieChart();
                CartersianChartVisibility = false;
                PieChartVisibility = true;
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
        public ICommand FilterKhoi { get; set; }



        public BaoCaoHocKyViewModel()
        {
            everLoaded = false;
            NienKhoaComboBox = new ObservableCollection<string>();
            KhoiComboBox = new ObservableCollection<Model.Khoi>();
            TenLop = new List<string>();
            SoLuongDatChartVal = new List<int>();
            SoLuongDat = new SeriesCollection();
            DanhSachBaoCaoHocKy = new ObservableCollection<Model.BaoCaoHocKy>();
            Dat = new int();
            KhongDat = new int();
            TiLeDat = new SeriesCollection();
            LoadBaoCao = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (everLoaded == false)
                {
                    BaoCaoHocKyWD = parameter as BaoCaoTongKetHocKy;
                    LoadComboboxData();
                    LoadDanhSachBaoCaoHocKy();
                    LoadCartesianChart();
                    CartersianChartVisibility = true;
                    PieChartVisibility = false;
                    everLoaded = true;
                }
            });


            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    FilterKhoiFromNienKhoa();
                    LoadDanhSachBaoCaoHocKy();
                    LoadCartesianChart();
                }
            });


            FilterHocKy = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    if (cmb.SelectedItem.ToString().Contains("1"))
                        HocKyQueries = "1";
                    else
                        HocKyQueries = "2";
                    LoadDanhSachBaoCaoHocKy();
                    LoadCartesianChart();
                }
            });


            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    Model.Khoi item = cmb.SelectedItem as Model.Khoi;
                    KhoiQueries = item.MaKhoi.ToString();
                    LoadDanhSachBaoCaoHocKy();
                    LoadCartesianChart();
                }
            });
        }



        public void LoadComboboxData()
        {
            BaoCaoHocKyWD.cmbHocKy.Items.Add("Học kỳ 1");
            BaoCaoHocKyWD.cmbHocKy.Items.Add("Học kỳ 2");
            HocKyQueries = "1";
            BaoCaoHocKyWD.cmbHocKy.SelectedIndex = 0;
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
                    string cmdString = "select distinct NienKhoa from Lop";
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
                                FilterKhoiFromNienKhoa();
                            }
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




        
        public void FilterKhoiFromNienKhoa()
        {
            KhoiComboBox.Clear();
            KhoiQueries = null;
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
                    string cmdString = "select k.MaKhoi,Khoi from BaoCaoHocKy bc join Lop l on bc.MaLop = l.MaLop join Khoi k on k.MaKhoi = l.MaKhoi " +
                                        " where NienKhoa = '" + NienKhoaQueries
                                        + "' and HocKy =  " + HocKyQueries + " ";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            KhoiComboBox.Add(new Khoi()
                            {
                                MaKhoi = reader.GetInt32(0),
                                TenKhoi = reader.GetString(1),
                            });
                            if (String.IsNullOrEmpty(KhoiQueries))
                            {
                                KhoiQueries = reader.GetInt32(0).ToString();
                                BaoCaoHocKyWD.cmbKhoi.SelectedIndex = 0;
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception )
                {
                }
            }
        }



        public void LoadDanhSachBaoCaoHocKy()
        {
            DanhSachBaoCaoHocKy.Clear();
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
                    string CmdString;
                    CmdString = "select bc.MaLop,l.TenLop,l.SiSo,bc.SoLuongDat,bc.TiLe from BaoCaoHocKy bc join Lop l on bc.MaLop = l.MaLop " +
                                    " where NienKhoa = '" + NienKhoaQueries + "' and HocKy = " + HocKyQueries + " " + " and MaKhoi = "+ KhoiQueries;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.BaoCaoHocKy baocaohocky = new StudentManagement.Model.BaoCaoHocKy();
                            baocaohocky.MaLop = reader.GetInt32(0);
                            baocaohocky.TenLop = reader.GetString(1);
                            baocaohocky.SiSo = reader.GetInt32(2);
                            baocaohocky.SoLuongDat = reader.GetInt32(3);
                            baocaohocky.TiLe = reader.GetString(4);
                            DanhSachBaoCaoHocKy.Add(baocaohocky);
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception )
                {
                }
            }
        }

        public void LoadCartesianChart()
        {
            TenLop.Clear();
            SoLuongDatChartVal.Clear();
            SoLuongDat.Clear();
            if (!String.IsNullOrEmpty(NienKhoaQueries))
            {
                try
                {
                    for (int i = 0;i<DanhSachBaoCaoHocKy.Count;i++)
                    {
                        TenLop.Add(DanhSachBaoCaoHocKy[i].TenLop);
                        SoLuongDatChartVal.Add(DanhSachBaoCaoHocKy[i].SoLuongDat);
                    }    
                    SoLuongDat.Add(new ColumnSeries
                    {
                        Title = "Số lượng đạt",
                        Values = new ChartValues<int>(SoLuongDatChartVal)
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }



        public void LoadPieChart()
        {
            TiLeDat.Clear();
            if (!String.IsNullOrEmpty(NienKhoaQueries) && !String.IsNullOrEmpty(HocKyQueries))
            {
                try
                {
                    if (BaoCaoHocKyWD.BaoCaoHocKyDataGrid.SelectedIndex >= 0 && DanhSachBaoCaoHocKy.Count > BaoCaoHocKyWD.BaoCaoHocKyDataGrid.SelectedIndex)
                    {
                        Dat = DanhSachBaoCaoHocKy[BaoCaoHocKyWD.BaoCaoHocKyDataGrid.SelectedIndex].SoLuongDat;
                        TongSiSoLop = DanhSachBaoCaoHocKy[BaoCaoHocKyWD.BaoCaoHocKyDataGrid.SelectedIndex].SiSo;



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
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

