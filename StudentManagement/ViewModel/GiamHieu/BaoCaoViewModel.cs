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

namespace StudentManagement.ViewModel.GiamHieu
{
    internal class BaoCaoViewModel:BaseViewModel
    {
        public string NienKhoaQueries { get; set; }
        public int HocKyQueries { get; set; }
        public string MonHocQueries { get; set; }
        public string LopQueries { get; set; }

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
        private ObservableCollection<string> _lopComboBox;
        public ObservableCollection<string> LopComboBox
        {
            get => _lopComboBox;
            set { _lopComboBox = value; OnPropertyChanged(); }
        }

        public ObservableCollection<int> _hocKyComboBox;
        public ObservableCollection<int> HocKyComboBox
        {
            get => _hocKyComboBox;
            set { _hocKyComboBox = value; OnPropertyChanged();}
        }

        public List<string> _tenMon { get; set; }

        public List<int> _soLuongDatChartVal { get; set; }

        public List<string> TenMon
        {
            get => _tenMon;
            set { _tenMon = value; OnPropertyChanged(); }
        }

        public List<int> SoLuongDatChartVal
        {
            get => _soLuongDatChartVal;
            set { _soLuongDatChartVal = value; OnPropertyChanged(); }
        }

        public SeriesCollection SoLuongDat { get; set; }

        public ICommand LoadBaoCao { get; set; }


        public BaoCaoViewModel()
        {
            LoadBaoCao = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                BaoCaoWD = parameter as BaoCao;
                LoadComboboxData();
            });

            LoadDanhSachBaoCaoMon();
            LoadChart();
        }



        public void LoadComboboxData()
        {
            NienKhoaComboBox = new ObservableCollection<string>();
            LopComboBox = new ObservableCollection<string>();
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
                        }
                    }
                    reader.NextResult();
                }
                con.Close();



                con.Open();
                cmdString = "select distinct HocKy from BaoCaoMon";
                cmd = new SqlCommand(cmdString, con);
                reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HocKyComboBox.Add(reader.GetInt32(0));
                        if (HocKyQueries != null)
                        {
                            HocKyQueries = reader.GetInt32(0);
                            BaoCaoWD.cmbHocKy.SelectedIndex = 0;
                        }
                    }
                    reader.NextResult();
                }
                con.Close();

                con.Open();
                cmdString = "select distinct TenLop from BaoCaoMon";
                cmd = new SqlCommand(cmdString, con);
                reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        LopComboBox.Add(reader.GetString(0));
                        if (string.IsNullOrEmpty(LopQueries))
                        {
                            LopQueries = reader.GetString(0);
                            BaoCaoWD.cmbLop.SelectedIndex = 0;
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
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select * from BaoCaoMon";
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

        public void LoadChart()
        {
            TenMon = new List<string>();
            SoLuongDatChartVal = new List<int>();
            SoLuongDat = new SeriesCollection();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "SELECT DISTINCT TenMon from BaoCaoMon";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TenMon.Add(reader.GetString(0));
                    }
                    reader.NextResult();
                }
                con.Close();




                con.Open();
                CmdString = "SELECT SoLuongDat from BaoCaoMon";
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
                    Values = new ChartValues<int> (SoLuongDatChartVal)
                });

                con.Close();
            }
        }
    }
}
