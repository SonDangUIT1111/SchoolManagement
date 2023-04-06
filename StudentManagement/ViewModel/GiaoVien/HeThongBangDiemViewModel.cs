using MaterialDesignThemes.Wpf;
using StudentManagement.Model;
using StudentManagement.Views.GiaoVien;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiaoVien
{
    public class HeThongBangDiemViewModel:BaseViewModel
    {

        // khai báo biến
        public string NienKhoaQueries { get; set; }
        public int HocKyQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }
        public string MonHocQueries { get; set; }
        public HeThongBangDiem HeThongBangDiemWD { get; set; }
        private ObservableCollection<StudentManagement.Model.HeThongDiem> _danhSachDiem;
        public ObservableCollection<StudentManagement.Model.HeThongDiem> DanhSachDiem { get => _danhSachDiem; set { _danhSachDiem = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _nienKhoaCmb;
        public ObservableCollection<string> NienKhoaCmb { get => _nienKhoaCmb; set { _nienKhoaCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<Lop> _lopCmb;
        public ObservableCollection<Lop> LopCmb { get => _lopCmb; set { _lopCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<Khoi> _khoiCmb;
        public ObservableCollection<Khoi> KhoiCmb { get => _khoiCmb; set { _khoiCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<MonHoc> _monCmb;
        public ObservableCollection<MonHoc> MonCmb { get => _monCmb; set { _monCmb = value; OnPropertyChanged(); } }

        // khai báo ICommand
        public ICommand LoadWindow { get; set; }
        public ICommand MouseEnterComboBox { get; set; }
        public ICommand MouseLeaveComboBox { get; set; }
        public HeThongBangDiemViewModel()
        {
            LoadWindow = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                HeThongBangDiemWD = parameter as HeThongBangDiem;
                LoadDuLieuComboBox();
            });
            MouseEnterComboBox = new RelayCommand<ComboBox>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Focus();
            });
            MouseLeaveComboBox = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                HeThongBangDiemWD.btnTrick.Focus();
            });
        }
        public void LoadDuLieuComboBox()
        {
            HeThongBangDiemWD.cmbHocKy.Items.Add("Học kỳ 1");
            HeThongBangDiemWD.cmbHocKy.Items.Add("Học kỳ 2");
            MessageBox.Show(HeThongBangDiemWD.cmbHocKy.Items[0].ToString());
            NienKhoaCmb = new ObservableCollection<string>();
            MonCmb = new ObservableCollection<MonHoc>();
            LopCmb = new ObservableCollection<Lop>();
            KhoiCmb = new ObservableCollection<Khoi>();
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
                        if (String.IsNullOrEmpty(NienKhoaQueries))
                        {
                            NienKhoaQueries = reader.GetString(0);
                            HeThongBangDiemWD.cmbNienKhoa.SelectedIndex = 0;
                        }
                    }
                    reader.NextResult();
                }
                con.Close();

                con.Open();
                CmdString = "select distinct Khoi from Khoi";
                cmd = new SqlCommand(CmdString, con);
                reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Khoi item = new Khoi();
                        item.MaKhoi = reader.GetInt32(0);
                        item.TenKhoi = reader.GetString(1);
                        KhoiCmb.Add(item);
                        if (String.IsNullOrEmpty(KhoiQueries))
                        {
                            KhoiQueries = reader.GetInt32(0).ToString();
                            HeThongBangDiemWD.cmbKhoi.SelectedIndex = 0;
                        }
                    }
                    reader.NextResult();
                }
                con.Close();

                if (!String.IsNullOrEmpty(NienKhoaQueries))
                {
                    con.Open();
                    CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries + "' and MaKhoi = "+KhoiQueries;
                    cmd = new SqlCommand(CmdString, con);
                    reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Lop item = new Lop();
                            item.MaLop = reader.GetInt32(0);
                            item.TenLop = reader.GetString(1);
                            LopCmb.Add(item);
                            if (String.IsNullOrEmpty(LopQueries))
                            {
                                LopQueries = reader.GetInt32(0).ToString();
                                HeThongBangDiemWD.cmbLop.SelectedIndex = 0;
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
                        MonHoc item = new MonHoc();
                        item.MaMon = reader.GetInt32(0);
                        item.TenMon = reader.GetString(1);
                        MonCmb.Add(item);
                        if (String.IsNullOrEmpty(MonHocQueries))
                        {
                            MonHocQueries = reader.GetInt32(0).ToString();
                            HeThongBangDiemWD.cmbMonHoc.SelectedIndex = 0;
                        }
                    }
                    reader.NextResult();
                }
                con.Close();
            }


        }

        public void KhoiDong()
        {
            
        }
    }
}
