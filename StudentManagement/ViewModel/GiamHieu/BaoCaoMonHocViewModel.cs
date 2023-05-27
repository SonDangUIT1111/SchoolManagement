using LiveCharts;
using LiveCharts.Wpf;
using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
        public bool everLoaded { get; set; }

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

       
        public ObservableCollection<Model.MonHoc> _monHocComboBox;

        public ObservableCollection<Model.MonHoc> MonHocComboBox
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

        public Func<double, string> Formatter { get; set; }

        public BaoCaoMon _gridSeletecdItem;

        public BaoCaoMon GridSelectedItem
        {
            get { return _gridSeletecdItem; }
            set
            {
                _gridSeletecdItem = value;
                OnPropertyChanged();
                if (_gridSeletecdItem != null)
                {
                    LoadPieChart();
                    CartersianChartVisibility = false;
                    PieChartVisibility = true;
                }
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
        public ICommand FilterMonHoc { get; set; }

        public BaoCaoMonHocViewModel()
        {
            everLoaded = false;
            NienKhoaComboBox = new ObservableCollection<string>();
            MonHocComboBox = new ObservableCollection<Model.MonHoc>();
            DanhSachBaoCaoMon = new ObservableCollection<Model.BaoCaoMon>();
            TenLop = new List<string>();
            SoLuongDatChartVal = new List<int>();
            SoLuongDat = new SeriesCollection();
            Dat = new int();
            KhongDat = new int();
            TiLeDat = new SeriesCollection();

         
            LoadBaoCao = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                if (everLoaded == false)
                {
                    ProgressBarVisibility = true;
                    DataGridVisibility = false;
                    BaoCaoWD = parameter as BaoCaoMonHoc;
                    LoadComboboxData();
                    await LoadDanhSachBaoCaoMon();
                    ProgressBarVisibility = false;
                    DataGridVisibility = true;
                    LoadCartesianChart();
                    CartersianChartVisibility = true;
                    PieChartVisibility = false;
                    everLoaded = true;
                }
            });

            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    ProgressBarVisibility = true;
                    DataGridVisibility = false;
                    await LoadDanhSachBaoCaoMon();
                    ProgressBarVisibility = false;
                    DataGridVisibility = true;
                }
            });


            FilterHocKy = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    if (cmb.SelectedItem.ToString().Contains("1"))
                    {
                        HocKyQueries = "1";
                    }
                    else if (cmb.SelectedItem.ToString().Contains("2"))
                    {
                        HocKyQueries= "2";
                    }
                    ProgressBarVisibility = true;
                    DataGridVisibility = false;
                    await LoadDanhSachBaoCaoMon();
                    ProgressBarVisibility = false;
                    DataGridVisibility = true;
                }
            });


            FilterMonHoc = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    Model.MonHoc item = cmb.SelectedItem as Model.MonHoc;
                    MonHocQueries = item.MaMon.ToString();
                }
                ProgressBarVisibility = true;
                DataGridVisibility = false;
                await LoadDanhSachBaoCaoMon();
                ProgressBarVisibility = false;
                DataGridVisibility = true;
                LoadCartesianChart();
                CartersianChartVisibility = true;
                PieChartVisibility = false;
            });


        }



        public async Task LoadComboboxData()
        {
            BaoCaoWD.cmbHocKy.Items.Add("Học kỳ 1");
            BaoCaoWD.cmbHocKy.Items.Add("Học kỳ 2");
            HocKyQueries = "1";
            BaoCaoWD.cmbHocKy.SelectedIndex = 0;

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
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return;
                    }
                    string cmdString = "select distinct NienKhoa from BaoCaoMon bc join Lop l on bc.MaLop = l.MaLop";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        NienKhoaComboBox.Add(reader.GetString(0));
                        if (string.IsNullOrEmpty(NienKhoaQueries))
                        {
                            NienKhoaQueries = reader.GetString(0);
                            BaoCaoWD.cmbNienKhoa.SelectedIndex = 0;
                            FilterDataMonHoc();
                            BaoCaoWD.cmbMonHoc.SelectedIndex = 0;
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }




        public async Task FilterDataMonHoc()
        {
            MonHocComboBox.Clear();
            MonHocQueries = "";

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
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return;
                    }
                    string cmdString = "select distinct bc.MaMon,TenMon from BaoCaoMon bc join Lop l on bc.MaLop = l.MaLop join MonHoc mh on mh.MaMon = bc.MaMon" +
                                        " where NienKhoa = '" + NienKhoaQueries + "' and HocKy = " + HocKyQueries + " ";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        MonHocComboBox.Add(new Model.MonHoc()
                        {
                            MaMon = reader.GetInt32(0),
                            TenMon = reader.GetString(1),
                        });
                        if (string.IsNullOrEmpty(MonHocQueries))
                        {
                            MonHocQueries = reader.GetInt32(0).ToString();
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }




        public async Task LoadDanhSachBaoCaoMon()
        {
            DanhSachBaoCaoMon.Clear();
            if (!String.IsNullOrEmpty(NienKhoaQueries) && !String.IsNullOrEmpty(MonHocQueries))
            {
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
                            MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                            return;
                        }
                        string CmdString = "select bc.MaLop,TenLop,SiSo,TenMon,SoLuongDat,TiLe from BaoCaoMon bc join Lop l on bc.MaLop = l.MaLop join MonHoc mh on mh.MaMon = bc.MaMon " +
                            " where NienKhoa = '" + NienKhoaQueries + "' and HocKy = " + HocKyQueries + " and bc.MaMon = " + MonHocQueries + " ";
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            StudentManagement.Model.BaoCaoMon baocaomon = new StudentManagement.Model.BaoCaoMon();
                            baocaomon.MaLop = reader.GetInt32(0);
                            baocaomon.TenLop = reader.GetString(1);
                            baocaomon.SiSo = reader.GetInt32(2);
                            baocaomon.TenMon = reader.GetString(3);
                            baocaomon.SoLuongDat = reader.GetInt32(4);
                            baocaomon.TiLe = reader.GetString(5);
                            DanhSachBaoCaoMon.Add(baocaomon);
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        public void LoadCartesianChart()
        {
            TenLop.Clear();
            SoLuongDatChartVal.Clear();
            SoLuongDat.Clear();
            if (!String.IsNullOrEmpty(NienKhoaQueries) && !String.IsNullOrEmpty(MonHocQueries))
            {
                try
                {
                    for (int i = 0; i < DanhSachBaoCaoMon.Count; i++)
                    {
                        TenLop.Add(DanhSachBaoCaoMon[i].TenLop);
                        SoLuongDatChartVal.Add(DanhSachBaoCaoMon[i].SoLuongDat);
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
            if (!String.IsNullOrEmpty(NienKhoaQueries) && !String.IsNullOrEmpty(MonHocQueries) && GridSelectedItem != null)
            {
                try
                {

                    if (BaoCaoWD.BaoCaoMonDataGrid.SelectedIndex >= 0 && BaoCaoWD.BaoCaoMonDataGrid.SelectedIndex < DanhSachBaoCaoMon.Count)
                    {
                        Dat = DanhSachBaoCaoMon[BaoCaoWD.BaoCaoMonDataGrid.SelectedIndex].SoLuongDat;
                        TongSiSoLop = DanhSachBaoCaoMon[BaoCaoWD.BaoCaoMonDataGrid.SelectedIndex].SiSo;




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
                    } };
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
