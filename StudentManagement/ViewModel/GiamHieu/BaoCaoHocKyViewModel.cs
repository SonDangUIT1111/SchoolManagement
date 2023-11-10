using LiveCharts;
using LiveCharts.Wpf;
using StudentManagement.Model;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class BaoCaoHocKyViewModel : BaseViewModel
    {
        public string NienKhoaQueries { get; set; }
        public string HocKyQueries { get; set; }
        public string KhoiQueries { get; set; }
        public int TongSiSoLop { get; set; }
        public bool everLoaded { get; set; }

        public bool IsTesting { get; set; }

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


        public ICommand LoadBaoCao { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterHocKy { get; set; }
        public ICommand FilterKhoi { get; set; }



        private readonly ISqlConnectionWrapper sqlConnection;
        public BaoCaoHocKyViewModel(ISqlConnectionWrapper sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
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
            LoadBaoCao = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                if (everLoaded == false)
                {
                    DataGridVisibility = true;
                    ProgressBarVisibility = true;
                    BaoCaoHocKyWD = parameter as BaoCaoTongKetHocKy;
                    BaoCaoHocKyWD.cmbHocKy.Items.Add("Học kỳ 1");
                    BaoCaoHocKyWD.cmbHocKy.Items.Add("Học kỳ 2");
                    HocKyQueries = "1";
                    BaoCaoHocKyWD.cmbHocKy.SelectedIndex = 0;
                    LoadComboboxData();
                    FilterKhoiFromNienKhoa();
                    await LoadDanhSachBaoCaoHocKy();
                    ProgressBarVisibility = false;
                    LoadCartesianChart();
                    CartersianChartVisibility = true;
                    PieChartVisibility = false;
                    everLoaded = true;
                }
            });


            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    FilterKhoiFromNienKhoa();
                    ProgressBarVisibility = true;
                    await LoadDanhSachBaoCaoHocKy();
                    ProgressBarVisibility = false;
                    LoadCartesianChart();
                    CartersianChartVisibility = true;
                    PieChartVisibility = false;
                }
            });


            FilterHocKy = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    if (cmb.SelectedItem.ToString().Contains("1"))
                        HocKyQueries = "1";
                    else
                        HocKyQueries = "2";
                    ProgressBarVisibility = true;
                    await LoadDanhSachBaoCaoHocKy();
                    ProgressBarVisibility = false;
                    LoadCartesianChart();
                    CartersianChartVisibility = true;
                    PieChartVisibility = false;
                }
            });


            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    Model.Khoi item = cmb.SelectedItem as Model.Khoi;
                    KhoiQueries = item.MaKhoi.ToString();
                    ProgressBarVisibility = true;
                    await LoadDanhSachBaoCaoHocKy();
                    ProgressBarVisibility = false;
                    LoadCartesianChart();
                    CartersianChartVisibility = true;
                    PieChartVisibility = false;
                }
            });
        }



        public void LoadComboboxData()
        {    
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    try
                    { 
                        sqlConnectionWrapper.Open(); 
                    } catch (Exception)
                    {
                        //MessageBoxFail messageBoxFail = new MessageBoxFail();
                        //messageBoxFail.ShowDialog();
                        return;
                    }
                    string cmdString = "select distinct NienKhoa from Lop";
                    SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            NienKhoaComboBox.Add(reader.GetString(0));
                            if (string.IsNullOrEmpty(NienKhoaQueries))
                            {
                                NienKhoaQueries = reader.GetString(0);
                                //BaoCaoHocKyWD.cmbNienKhoa.SelectedIndex = 0;
                            }
                        }
                        reader.NextResult();
                    }
                    sqlConnectionWrapper.Close();
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }




        
        public void FilterKhoiFromNienKhoa()
        {
            KhoiComboBox.Clear();
            KhoiQueries = null;
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    try
                    { 
                        sqlConnectionWrapper.Open(); 
                    } catch (Exception) 
                    {
                        //MessageBoxFail messageBoxFail = new MessageBoxFail();
                        //messageBoxFail.ShowDialog();
                        return; 
                    }
                    string cmdString = "select distinct k.MaKhoi,Khoi from BaoCaoHocKy bc join Lop l on bc.MaLop = l.MaLop join Khoi k on k.MaKhoi = l.MaKhoi " +
                                        " where NienKhoa = '" + NienKhoaQueries
                                        + "' and HocKy =  " + HocKyQueries + " ";
                    SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrapper.GetSqlConnection());
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
                                //BaoCaoHocKyWD.cmbKhoi.SelectedIndex = 0;
                            }
                        }
                        reader.NextResult();
                    }
                    sqlConnectionWrapper.Close();
                }
                catch (Exception )
                {
                }
            }
        }



        public async Task LoadDanhSachBaoCaoHocKy()
        {
            DanhSachBaoCaoHocKy.Clear();
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        sqlConnectionWrapper.OpenAsync();
                    }
                    catch (Exception)
                    {
                        //MessageBoxFail messageBoxFail = new MessageBoxFail();
                        //messageBoxFail.ShowDialog();
                        return;
                    }
                    string CmdString;
                    CmdString = "select bc.MaLop,l.TenLop,l.SiSo,bc.SoLuongDat,bc.TiLe from BaoCaoHocKy bc join Lop l on bc.MaLop = l.MaLop " +
                        " where NienKhoa = '" + NienKhoaQueries + "' and HocKy = " + HocKyQueries + " " + " and MaKhoi = " + KhoiQueries;
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        StudentManagement.Model.BaoCaoHocKy baocaohocky = new StudentManagement.Model.BaoCaoHocKy();
                        baocaohocky.MaLop = reader.GetInt32(0);
                        baocaohocky.TenLop = reader.GetString(1);
                        baocaohocky.SiSo = reader.GetInt32(2);
                        baocaohocky.SoLuongDat = reader.GetInt32(3);
                        baocaohocky.TiLe = reader.GetString(4);
                        DanhSachBaoCaoHocKy.Add(baocaohocky);
                    }
                    sqlConnectionWrapper.Close();
                }
                catch (Exception)
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
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
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
                    int SelectingItem = BaoCaoHocKyWD.BaoCaoHocKyDataGrid.SelectedIndex;
                    if (IsTesting == true)
                    {
                        SelectingItem = 1;
                    }
                    if (SelectingItem >= 0)
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
                } catch (Exception e)
                {
                    Console.WriteLine(e);
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }
    }
}

