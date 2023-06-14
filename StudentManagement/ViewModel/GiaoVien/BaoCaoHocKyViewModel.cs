using LiveCharts;
using LiveCharts.Wpf;
using StudentManagement.Model;
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

namespace StudentManagement.ViewModel.GiaoVien
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

        public Views.GiaoVien.BaoCaoTongKetHocKy BaoCaoHocKyWD;

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
                    BaoCaoHocKyWD = parameter as Views.GiaoVien.BaoCaoTongKetHocKy;
                    LoadComboboxData();
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
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
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
                catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
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
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
                        return; 
                    }
                    string cmdString = "select distinct k.MaKhoi,Khoi from BaoCaoHocKy bc join Lop l on bc.MaLop = l.MaLop join Khoi k on k.MaKhoi = l.MaKhoi " +
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



        public async Task LoadDanhSachBaoCaoHocKy()
        {
            DanhSachBaoCaoHocKy.Clear();
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
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
                        return;
                    }
                    string CmdString;
                    CmdString = "select bc.MaLop,l.TenLop,l.SiSo,bc.SoLuongDat,bc.TiLe from BaoCaoHocKy bc join Lop l on bc.MaLop = l.MaLop " +
                        " where NienKhoa = '" + NienKhoaQueries + "' and HocKy = " + HocKyQueries + " " + " and MaKhoi = " + KhoiQueries;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
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
                    con.Close();
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
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
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
                } catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                }
            }
        }
    }
}

